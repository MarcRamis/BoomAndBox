using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TutorialText : MonoBehaviour
{

    [Serializable] public struct Text
    {
        public enum Face
        {
            Normal,
            Angry,
            NONE
        };

        public Face faceSelected;

        public string text;

        public float timeToWait;
    };

    [Header("Faces images")]
    [SerializeField] private Sprite normalFace;
    [SerializeField] private Sprite angryFace;

    [Header("Canvas elements")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Image image;
    
    [Header("Variables to edit")]
    [SerializeField] private Text[] textsToAppear = null;

    [Header("InputController")]
    [SerializeField] private MenuInputController inputsUI;
    [SerializeField] private Player player;

    [Header("Events")]
    [SerializeField] private UnityEvent EndTextEvent;

    private float timer = 0.0f;
    private bool startTimer = false;
    private int currentText = 0;

    private void OnEnable()
    {
        inputsUI.onContinueTextInput += ContinueText;
    }

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    private void Update()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && currentText == 0)
        {
            StartTutorialText();
        }
    }

    public void ContinueText()
    {
        if(gameObject != null && startTimer)
        {
            if (currentText > textsToAppear.Length - 1)
            {
                OnDisable();
                player.AllowInputs();
                EndTextEvent?.Invoke();
                Destroy(gameObject);
                
            }
            if (currentText < textsToAppear.Length && timer >= textsToAppear[currentText].timeToWait)
            {
                textMeshProUGUI.text = textsToAppear[currentText].text;
                switch (textsToAppear[currentText].faceSelected)
                {
                    case Text.Face.Normal:
                        image.sprite = normalFace;
                        break;
                    case Text.Face.Angry:
                        image.sprite = angryFace;
                        break;
                    case Text.Face.NONE:
                        image.sprite = normalFace;
                        break;
                    default:
                        image.sprite = normalFace;
                        break;
                }
                currentText++;
                timer = 0.0f;
            }
        }
        
    }

    public void StartTutorialText()
    {
        player.BlockInputsAndCamera();

        textMeshProUGUI.text = textsToAppear[currentText].text;
        switch (textsToAppear[currentText].faceSelected)
        {
            case Text.Face.Normal:
                image.sprite = normalFace;
                break;
            case Text.Face.Angry:
                image.sprite = angryFace;
                break;
            case Text.Face.NONE:
                image.sprite = normalFace;
                break;
            default:
                image.sprite = normalFace;
                break;
        }
        canvas.gameObject.SetActive(true);

        startTimer = true;
        currentText++;
    }

    private void OnDisable()
    {
        inputsUI.onContinueTextInput -= ContinueText;
    }

}
