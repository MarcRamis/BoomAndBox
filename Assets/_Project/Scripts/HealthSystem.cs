using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class HealthSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private Vector2 jumpDestination = Vector2.zero;
    [SerializeField] private float jumpForce = 1.0f;
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float durationJump = 0.8f;
    [Space]
    [Header("Objects")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHealth;
    [SerializeField] private Sprite emptyHealth;
    [SerializeField] private RectTransform test;
    //Private variables
    private GameObject player;
    int healthUI;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        healthUI = player.GetComponent<Player>().Health;
        LivesAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLives();
    }

    private void UpdateLives()
    {
        healthUI = player.GetComponent<Player>().Health;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < healthUI)
            {
                hearts[i].sprite = fullHealth;
            }
            else
            {
                hearts[i].sprite = emptyHealth;
            }
        }
    }

    private void LivesAnimation()
    {
        Transform tempTrans = hearts[4].transform;

        Debug.Log("Global pos:" + tempTrans.position);
        Debug.Log("Local pos:" + tempTrans.localPosition);
        tempTrans = transform.parent.GetComponent<RectTransform>();
        Vector3 acumPos = Vector3.zero;
        while (tempTrans != null)
        {
            if (tempTrans.GetComponent<RectTransform>() == null)
                break;
            Debug.Log(tempTrans.transform.name);
            acumPos += tempTrans.GetComponent<RectTransform>().position;
            tempTrans = tempTrans.parent;
        }
        Debug.Log(acumPos);

    }

}
