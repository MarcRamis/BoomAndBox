using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceSingleton : SingletonMonobehaviour<ReferenceSingleton>
{ 
    public GameObject player;
    public Player playerScript;
    public GameObject companion;
    public Companion companionScript;

    public new void Awake()
    {
        if (instance == null)
        {
            //instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
