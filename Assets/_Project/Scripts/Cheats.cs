using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private KeyCode godModeKey = KeyCode.Period;
    [SerializeField] private KeyCode restartLevel = KeyCode.Comma;

    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            if (Input.GetKeyDown(godModeKey))
            {
                player.GetComponent<Player>().SwitchGodMode();
            }
            else if (Input.GetKeyDown(restartLevel))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
    }
}
