using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Shooter : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private bool activated = false;
    [SerializeField] private float timeBetweenShoots = 1.2f;
    [SerializeField] [Range(0.0f, 10.0f)] private float timeToWaitBefStart = 0.0f;
    [Space]
    [Header("Obj")]
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private Transform projectileSpawnPoint = null;

    //Private variables
    private bool hasShot = false;
    private bool waitToStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if(timeToWaitBefStart > 0)
        {
            waitToStart = true;
            Invoke(nameof(StartShooting), timeToWaitBefStart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!activated && !hasShot && !waitToStart)
        {
            Shoot();
            hasShot = true;
            Invoke(nameof(ChangeBoolShot), timeBetweenShoots);
        }
    }

    private void Shoot()
    {
        Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    private void ChangeBoolShot()
    {
        hasShot = false;
    }

    private void StartShooting()
    {
        waitToStart = false;
    }

    public void TurnOff()
    {
        activated = false;
    }

    public void TurnOn()
    {
        activated = true;
    }
}
