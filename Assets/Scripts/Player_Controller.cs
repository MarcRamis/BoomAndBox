using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody rbPlayer;
    private Vector2 plRotation = Vector2.zero;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float mouseRotation = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private Camera plCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        //Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;

        rbPlayer = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        LookPlayer();
    }

    void MovePlayer()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            float velocityH = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            float velocityY = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            Vector3 direction = (transform.forward * velocityY + transform.right * velocityH).normalized;
            rbPlayer.velocity = direction * Time.deltaTime * speed;
        }

        if (Input.GetButton("Jump"))
        {
            rbPlayer.AddForce(new Vector3(0,1,0) * jumpForce, ForceMode.Acceleration);
        }
    }

    void LookPlayer()
    {
        plRotation.x += Input.GetAxis("Mouse Y") * mouseRotation;

        if (plRotation.x > 60)
        {
            plRotation.x = 60;
        }
        else if(plRotation.x < -60)
        {
            plRotation.x = -60;
        }
        plRotation.y += Input.GetAxis("Mouse X") * mouseRotation;
        this.transform.localRotation = Quaternion.Euler(0, plRotation.y, 0);
        plCamera.transform.localRotation = Quaternion.Euler(-plRotation.x, 0, 0);
    }

}
