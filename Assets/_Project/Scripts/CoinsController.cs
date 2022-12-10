using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventsSystem.current.onCoinCollected += OnCoinCollected;
    }

    private void OnCoinCollected()
    {
        score++;
        textScore.text= score.ToString();
    }

    private void OnDestroy()
    {
        EventsSystem.current.onCoinCollected -= OnCoinCollected;
    }
}
