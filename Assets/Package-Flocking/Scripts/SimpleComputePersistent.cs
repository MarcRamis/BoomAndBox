using System.Collections.Generic;
using UnityEngine;

public struct GameObjectInfo
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public Vector3 target;
    public Vector3 seek;
    public Vector3 obstacleAvoidance;
    public Vector3 separation;
    public Vector3 alignment;
    public float speed;
 
    public static int Size
    {
        get
        {
            return (sizeof(float) * 3 * 9) + sizeof(float);
        }
    }
}


public class SimpleComputePersistent : MonoBehaviour
{
    public GameObject prefab;
    public ComputeShader shader;

    public int maxObjectsSpawn = 68;
    public List<GameObject> objects;
    ComputeBuffer dataBuffer;
    GameObjectInfo[] data;
    public Transform[] targets;
    public Transform startTarget;
    [Range(0.1f, 50)] public float minVelocity;
    [Range(0.1f, 50)] public float maxVelocity;
    float[] randomVelocities;
    public float seekForce = 1.0f;
    public float cohesionForce = 0.5f;
    public float separationForce = 0.5f;
    public float alignForce = 0.5f;
    public float obstacleAvoidanceForce = 20f;
    public float rotationSpeed = 100f;
    public float neighbourRadius = 5f;
    public float rayLength = 5f;
    public float sensorSidesOffset = 10f;
    public bool drawGizmos = false;
    public float changeTargetDistance = 3;

    // Start is called before the first frame update
    void Start()
    {
        //We create the objects to move
        objects = new List<GameObject>(maxObjectsSpawn);
        for (int i = 0; i < maxObjectsSpawn; i++)
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.GetComponent<BoidA>().simpleComputePersistent = this;
            Vector3 randomPosition = new Vector3(Random.Range(0.0f, 3.0f), Random.Range(0.0f, 3.0f), Random.Range(0.0f, 3.0f));
            newObj.transform.position = transform.position + randomPosition;
            objects.Add(newObj);
        }
        //we generate the data array to pass data from CPU to GPU at the initialization, and don't release the buffer until destroy is called

        int numObjs = objects.Count;
        data = new GameObjectInfo[numObjs];
        randomVelocities = new float[numObjs];
        for (int i = 0; i < numObjs; i++)
        {
            randomVelocities[i] = Random.Range(minVelocity, maxVelocity);
            data[i].speed = randomVelocities[i];
            data[i].target = startTarget.transform.position;
            data[i].velocity = Vector3.zero;
            data[i].force = Vector3.zero;
            data[i].position = objects[i].transform.position;
            //data[i].force = Seek(i, data[i].target);
        }
        //We create the buffer to pass data to the GPU
        //The constructor asks for an ammount of objects for the buffer and the size of each object in bytes
        dataBuffer = new ComputeBuffer(numObjs, GameObjectInfo.Size);
        //We load the data into the buffer
        dataBuffer.SetData(data);
    }

    // Update is called once per frame
    void Update()
    {   
        //we generate the data array to pass data from CPU to GPU
        int numObjs = objects.Count;
        if (numObjs > 0)
        {
            int kernelHandle = shader.FindKernel("CSMain");
            shader.SetBuffer(kernelHandle, "CoolerResult", dataBuffer);
            shader.SetFloat("deltaTime", Time.deltaTime);
            shader.SetInt("numobjs", numObjs);
            shader.SetFloat("seekForce", seekForce);
            shader.SetFloat("cohesionForce", cohesionForce);
            shader.SetFloat("separateForce", separationForce);
            shader.SetFloat("alignForce", alignForce);
            shader.SetFloat("obstacleAvoidanceForce", obstacleAvoidanceForce);
            shader.SetFloat("neighbourRadius", neighbourRadius);

            int threadGroups = Mathf.CeilToInt(numObjs / 128.0f);

            shader.Dispatch(kernelHandle, threadGroups, 1, 1);

            dataBuffer.GetData(data);
            for (int i = 0; i < numObjs; i++)
            {
                if (Vector3.Distance(data[i].position, data[i].target) < changeTargetDistance)
                {
                    data[i].target = CalculateNewTarget(data[i].target);
                }

                data[i].separation = Separation(i);
                data[i].alignment = Alignment(i);
                data[i].obstacleAvoidance = ObstacleAvoidance(i);

                // Update gameobjects 
                objects[i].transform.position = data[i].position;
                Quaternion orientation = Quaternion.LookRotation(data[i].velocity, Vector3.up);
                objects[i].transform.rotation = Quaternion.RotateTowards(objects[i].transform.rotation, orientation, rotationSpeed * Time.deltaTime);
            }

            dataBuffer.SetData(data);
        }

        
    }

    private Vector3 CalculateNewTarget(Vector3 currentTarget)
    {
        if (currentTarget == targets[targets.Length - 1].position) return targets[0].transform.position;

        for (int i = 0; i < targets.Length; i++)
        {
            if (currentTarget == targets[i].position)
            {
                return targets[i + 1].transform.position;
            }
        }

        return startTarget.transform.position;
    }

    public void OnDestroy()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        dataBuffer.Release();
    }

    public Vector3 GetTarget(Vector3 ownPos, Vector3 newPos)
    {
        return ownPos - newPos;
    }

    public Vector3 Cohesion(int id)
    {
        Vector3 cohesionVector = Vector3.zero;

        for (int i = 0; i < objects.Count; i++)
        {
            if (i != id)
            {
                cohesionVector += data[i].position;
            }
        }
        cohesionVector /= objects.Count - 1;
        cohesionVector -= data[id].position;

        cohesionVector.Normalize();
        cohesionVector *= randomVelocities[id];
        Vector3 steeringForce = cohesionVector - data[id].velocity;
        steeringForce.Normalize();

        return steeringForce;
    }
    public Vector3 Separation(int id)
    {
        Vector3 separateVector = Vector3.zero;

        for (int i = 0; i < objects.Count; i++)
        {
            if (i != id)
            {
                separateVector += data[id].position - data[i].position;
            }
        }
        separateVector /= objects.Count - 1;

        separateVector.Normalize();
        separateVector *= randomVelocities[id];
        Vector3 steeringForce = separateVector - data[id].velocity;
        steeringForce.Normalize();

        return steeringForce;
    }
    public Vector3 Alignment(int id)
    {
        Vector3 alignVector = Vector3.zero;

        for (int i = 0; i < objects.Count; i++)
        {
            if (i != id)
            {
                alignVector += data[i].velocity;
            }
        }
        alignVector /= objects.Count - 1;

        alignVector.Normalize();
        alignVector *= randomVelocities[id];
        Vector3 steeringForce = alignVector - data[id].velocity;
        steeringForce.Normalize();

        return steeringForce;
    }
    public Vector3 Seek(int id, Vector3 target)
    {
        Vector3 desiredVelocity = target - data[id].position;
        desiredVelocity.Normalize();
        desiredVelocity *= data[id].speed;
        Vector3 steeringForce = desiredVelocity - data[id].velocity;
        steeringForce.Normalize();

        return steeringForce;
    }
    public Vector3 ObstacleAvoidance(int id)
    {
        Vector3 normal = Vector3.zero;

        // Looking forward
        if (Physics.Raycast(data[id].position, data[id].velocity.normalized, rayLength))
        {
            int randomSide = Random.Range(0,10);
            if (randomSide < 5)
            {
                normal += (objects[id].transform.TransformDirection(Vector3.right));
            }
            else
            {
                normal += (objects[id].transform.TransformDirection(Vector3.left));
            }
        }
        
        // Looking down/diagonal
        if (Physics.Raycast(data[id].position, (data[id].velocity + (objects[id].transform.TransformDirection(Vector3.down).normalized)), rayLength))
        {
            normal += (objects[id].transform.TransformDirection(Vector3.up));
        }


        return normal;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(data[i].position, neighbourRadius);
                
                Gizmos.color = Color.red;
                Gizmos.DrawRay(data[i].position, data[i].velocity.normalized * rayLength);
                
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(data[i].position, (data[i].velocity + (objects[i].transform.TransformDirection(Vector3.down)).normalized * rayLength));
            }
        }
    }
}