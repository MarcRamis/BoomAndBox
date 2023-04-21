using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public bool debugTrail = false;
    public struct BufferObj
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 size;
    }
    private LinkedList<BufferObj> trailList = new LinkedList<BufferObj>();
    private LinkedList<BufferObj> trailFillerList = new LinkedList<BufferObj>();
    [SerializeField] private LayerMask hittableLayers;
    [HideInInspector] public bool hitIsOn = false;
    
    [HideInInspector] private Player player;
    [HideInInspector] private RythmSystem rythmSystem;
    
    [Header("References")]
    [SerializeField] private GameObject weaponGameObject;
    [SerializeField] private BoxCollider weaponCollider;
    [SerializeField] private WeaponFeedbackController weaponFeedbackController;

    [Header("Settings")]
    [SerializeField] private float attackCd = 0.8f;
    [SerializeField] private float hitOnStartCd = 0.2f;
    [SerializeField] private float hitOnFinishCd = 0.5f;
    [SerializeField] private float returnControlsAfterAttackCd = 0.7f;
    [SerializeField] private float attackImpulse = 50f;
    [SerializeField] private Vector3 collisionSize; 
    [SerializeField] private float sizeSmall;
    
    [HideInInspector] public bool attackIsReady = true;
    [HideInInspector] private bool hasHit = false;
   
    private bool rythmMoment;
    private readonly int maxFrameBuffer = 10;

    private void Awake()
    {
        player = GetComponent<Player>();
        rythmSystem = GameObject.FindGameObjectWithTag("MainSoundtrack").GetComponent<RythmSystem>();
        
        player.myInputs.OnAttackPerformed += DoAttack;
    }
    
    private void Update()
    {
        collisionSize = weaponCollider.size;
    }

    private void FixedUpdate()
    {
        rythmMoment = rythmSystem.IsRythmMoment();

        if (hitIsOn)
        {
            CheckTrail();
        }
    }

    private void DoAttack()
    {
        if(player.CanAttack())
        {
            if (attackIsReady)
            {
                attackIsReady = false;
                
                if (rythmMoment)
                {
                    //Debug.Log("Attack on rythm");
                }
                else
                {
                    //Debug.Log("Attack NORMAL");
                    
                }

                player.feedbackController.PlayAttack();
                weaponFeedbackController.PlayAttack();
                player.playerRigidbody.AddForce(player.model.forward * attackImpulse * 10f, ForceMode.Acceleration);
                
                Invoke(nameof(AttackOn),hitOnStartCd);
                Invoke(nameof(AttackOff), hitOnFinishCd);
                player.BlockInputsWithTime(returnControlsAfterAttackCd);
                Invoke(nameof(ResetAttack), attackCd);
            }
        }
    }
    
    private void ResetAttack()
    {
        attackIsReady = true;
        player.feedbackController.StopAttack();
        weaponFeedbackController.StopAttack();
    }
    private void AttackOn()
    {
        hitIsOn = true;
    }
    private void AttackOff()
    {
        hitIsOn = false;
        trailList.Clear();
        trailFillerList.Clear();
    }

    private void CheckTrail()
    {
        BufferObj bo = new BufferObj();
        bo.size = weaponCollider.size;
        bo.rot = weaponCollider.transform.rotation;
        bo.pos = weaponCollider.transform.position + weaponCollider.transform.TransformDirection(weaponCollider.center);
        trailList.AddFirst(bo);

        if (trailList.Count > maxFrameBuffer)
        {
            trailList.RemoveLast();
        }
        //if (trailList.Count > 1)
        //{
        //    trailFillerList = FillTrail(trailList.First.Value, trailList.Last.Value);
        //}
        
        // Real collider
        Collider[] hits = Physics.OverlapBox(bo.pos, bo.size / 2, bo.rot, hittableLayers, QueryTriggerInteraction.Ignore);
        Dictionary<long, Collider> colliderList = new Dictionary<long, Collider>();

        CollectColliders(hits, colliderList);
        foreach (BufferObj cbo in trailFillerList)
        {
            hits = Physics.OverlapBox(cbo.pos, cbo.size / 2, cbo.rot, hittableLayers, QueryTriggerInteraction.Ignore);
            CollectColliders(hits, colliderList);
        }

        foreach(Collider other in colliderList.Values)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(1);
                player.feedbackController.PlayHit();
            }
            else
            {
                Debug.Log(other.name);
            }
        }
    }

    public void ShowWeapon()
    {
        weaponGameObject.SetActive(true);
    }

    public void HideWeapon()
    {
        weaponGameObject.SetActive(false);
    }

    private static void CollectColliders(Collider[] hits, Dictionary<long, Collider> colliderList)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (!colliderList.ContainsKey(hits[i].GetInstanceID()))
            {
                colliderList.Add(hits[i].GetInstanceID(), hits[i]);
            }
        }
    }

    private LinkedList<BufferObj> FillTrail(BufferObj from, BufferObj to)
    {
        LinkedList<BufferObj> fillerList = new LinkedList<BufferObj>();
        float distance = Math.Abs((from.pos - to.pos).magnitude);
        
        if (distance > sizeSmall)
        {
            int steps = Mathf.CeilToInt(distance / weaponCollider.size.z);
            float stepsAmount = 1 / (steps + 1);
            float stepValue = 0;

            for (int i = 0; i < steps; i++)
            {
                stepValue += stepsAmount;
                BufferObj tmpBo = new BufferObj();
                tmpBo.size = weaponCollider.size;
                tmpBo.pos = Vector3.Lerp(from.pos, to.pos, stepValue);
                tmpBo.rot = Quaternion.Lerp(from.rot, to.rot, stepValue);
                fillerList.AddFirst(tmpBo);
            }
        }
        return fillerList;
    }
    
    private void OnDrawGizmos()
    {
        if (debugTrail)
        {
            foreach (BufferObj bo in trailList)
            {
                Gizmos.color = Color.blue;
                Gizmos.matrix = Matrix4x4.TRS(bo.pos, bo.rot, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, bo.size);
            }

            foreach (BufferObj bo in trailFillerList)
            {
                Gizmos.color = Color.yellow;
                Gizmos.matrix = Matrix4x4.TRS(bo.pos, bo.rot, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, bo.size);
            }
        }
    }
}
 