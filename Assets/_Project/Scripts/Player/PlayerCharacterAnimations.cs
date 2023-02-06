using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAnimations : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private Animator animator;
    private PlayerMovementSystem playerMovementSystem;
    private Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerMovementSystem = GetComponent<PlayerMovementSystem>();
    }
}
