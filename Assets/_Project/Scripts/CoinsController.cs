using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsController : MonoBehaviour
{
    [SerializeField] private int coinCuantity = 0;
    [SerializeField] private Image coinImage = null;
    [SerializeField] private float conversion = 0.5f;
    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventsSystem.current.onCoinCollected += OnCoinCollected;
        coinImage.fillAmount = 0;
        if(score == 0)
        {
            int count = 0;
            GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in objectsInScene)
            {
                if (obj.name.Contains("Coin"))
                {
                    count++;
                }
            }
            float cvr = count * conversion;
            coinCuantity = (int)cvr;
        }
        

    }

    private void OnCoinCollected()
    {
        score++;
        if(score / coinCuantity < 1)
        {
            coinImage.fillAmount = score / coinCuantity;
        }
        else
        {
            coinImage.fillAmount = 1;
        }
        
    }

    private void OnDestroy()
    {
        EventsSystem.current.onCoinCollected -= OnCoinCollected;
    }
}
