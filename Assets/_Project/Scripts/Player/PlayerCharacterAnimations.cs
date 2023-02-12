using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAnimations : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private Animator playerAnimator;
    private PlayerMovementSystem playerMovementSystem;
    private Rigidbody playerRb;
    
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerMovementSystem = GetComponent<PlayerMovementSystem>();
    }

    private void Update()
    {
        playerAnimator.SetBool("Grounded", playerMovementSystem.isGrounded);
        playerAnimator.SetFloat("Speed", playerRb.velocity.magnitude);
        playerAnimator.SetInteger("CurrentDoubleJumps", playerMovementSystem.currentDoubleJumps);
        playerAnimator.SetBool("isFalling", playerMovementSystem.isFalling);
        playerAnimator.SetBool("isLanding", playerMovementSystem.landing);
        playerAnimator.SetBool("JustHitGround", playerMovementSystem.justHitGround);
        playerAnimator.SetBool("isDoubleJump", playerMovementSystem.isDoubleJumping);
        playerAnimator.SetBool("ReadyToJump", playerMovementSystem.readyToJump);
        
        // this variable works as the inverse because is setted to false when you press the input. 
        // I make it negative in order to get the current "input pressed"
        playerAnimator.SetBool("PressedInputJump", !playerMovementSystem.readyToJump);
    }
}
