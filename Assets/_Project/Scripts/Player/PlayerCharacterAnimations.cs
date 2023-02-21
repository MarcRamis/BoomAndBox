using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAnimations : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private Animator playerAnimator;
    private PlayerMovementSystem playerMovementSystem;
    private ThrowingSystem throwingSystem;
    private Rigidbody playerRb;
    
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerMovementSystem = GetComponent<PlayerMovementSystem>();
        throwingSystem = GetComponent<ThrowingSystem>();
    }

    private void Update()
    {
        playerAnimator.SetBool("Grounded", playerMovementSystem.isGrounded);
        playerAnimator.SetFloat("Speed", playerRb.velocity.magnitude);
        playerAnimator.SetInteger("CurrentDoubleJumps", playerMovementSystem.currentDoubleJumps);
        playerAnimator.SetBool("isFalling", playerMovementSystem.isFalling);
        playerAnimator.SetBool("isLanding", playerMovementSystem.landing);
        playerAnimator.SetBool("isDashing", playerMovementSystem.isDashing);
        playerAnimator.SetBool("JustHitGround", playerMovementSystem.justHitGround);
        playerAnimator.SetBool("isDoubleJump", playerMovementSystem.isDoubleJumping);
        playerAnimator.SetBool("ReadyToJump", playerMovementSystem.readyToJump);
        playerAnimator.SetBool("isAiming", playerMovementSystem.isAiming);
        
        // this variable works as the inverse because is setted to false when you press the input. 
        // I make it negative in order to get the current "input pressed"
        playerAnimator.SetBool("PressedInputJump", !playerMovementSystem.readyToJump);


        
        playerAnimator.SetBool("ReadyToThrow", throwingSystem.readyToThrow);
        playerAnimator.SetBool("JustThrow", throwingSystem.justThrow);
    }
}
