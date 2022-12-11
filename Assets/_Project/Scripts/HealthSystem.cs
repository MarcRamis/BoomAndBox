using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHealth;
    [SerializeField] private Sprite emptyHealth;
    //Private variables
    private GameObject player;
    int healthUI;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        healthUI = player.GetComponent<Player>().Health;
    }

    // Update is called once per frame
    void Update()
    {
        healthUI = player.GetComponent<Player>().Health;
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < healthUI)
            {
                hearts[i].sprite = fullHealth;
            }
            else
            {
                hearts[i].sprite = emptyHealth;
            }
        }
    }
}
