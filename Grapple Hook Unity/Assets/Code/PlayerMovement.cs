using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    //Public Set Movement Variables
    public float movementSpeed = 10;
    public float jumpHeight = 15000;

    //Relating to XZ Movement
    private float InputX, InputZ;
    private Camera cam;

    //Relating to Y Movement
    public float gravityScale = 1f;
    private float fallMultiplier = 1.75f;
    private bool jumpInput;
    private float jumpForce;

    //Repeatedly Accessed Components
    private Transform grabbedMaterial;
    private CapsuleCollider col;
    private Rigidbody rb;

    //Grounded Collision Detection Stuff
    private bool isGrounded;
    private float halfHeight;
    private Vector3 squareExtents;
    private LayerMask setLayer; // All objects considered jumpable will be on layer 0.

    //Constants
    private readonly float feetScale = 0.8f;
    private readonly float gravityValue = -9.81f;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        setLayer = LayerMask.GetMask("Default");
        jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        squareExtents = new Vector3(col.radius * feetScale, 0.15f, col.radius * feetScale);
        halfHeight = col.height / 2;
    }
    //Would move physics updates to FixedUpdate(), messes with the jumping for now.
    //Seems to function the same at various framerates, so it may not be necessary
    private void Update()
    {
        CheckGrounded();
        GatherMovementInputs();
    }

    //Runs raycasts every fixed timestep, likely helps performance
    private void FixedUpdate()
    {
        HandleMovement();
        MoveWithMaterial();
    }


    private void GatherMovementInputs()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        jumpInput = Input.GetButton("Jump");
    }

    //Combines the XZ and Y movement functions
    private void HandleMovement()
    {
        HandleXZMovement();
        HandleYMovement();
    }

    //Grabs the WASD Inputs, checks if sprinting, adjusts direction to camera, moves rigidbody.
    //May swap to MovePosition over AddForce, might be cleaner. Will test whenever we try to polish the game.
    private void HandleXZMovement()
    {
        Vector3 move = new Vector3(InputX, 0, InputZ);
        float moveSpeed = movementSpeed;
        float bobSpeed = 1;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 2.5f * movementSpeed;
            bobSpeed = 1.2f;
        }
        move = cam.transform.TransformDirection(move);
        move = new Vector3(move.x, 0, move.z).normalized;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.velocity += move * moveSpeed * Time.deltaTime;
    }

    private void HandleYMovement()
    {
        if (!isGrounded)
        {
            rb.AddForce(new Vector3(0, gravityScale * gravityValue * fallMultiplier * Time.deltaTime, 0), ForceMode.VelocityChange);
            return;
        }
        if (jumpInput)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(new Vector3(0, jumpForce * Time.deltaTime, 0), ForceMode.VelocityChange);
        }
    }

    //Casts a rectangular prism at the feet of the player, if any part of this is touching the ground, sets isGrounded to true.
    //Otherwise false.
    private void CheckGrounded()
    {
        isGrounded = Physics.BoxCast(col.bounds.center, squareExtents, new Vector3(0, -1, 0), out RaycastHit hit, Quaternion.identity, halfHeight, setLayer);
        if (isGrounded)
        {
            //if (hit.transform.CompareTag("Interactible"))
            //{
            //    grabbedMaterial = hit.transform;
            //    return;
            //}
        }
        grabbedMaterial = null;
    }



    private void MoveWithMaterial()
    {
        if (grabbedMaterial == null) return;
        Rigidbody materialRb = grabbedMaterial.GetComponent<Rigidbody>();
        rb.velocity += new Vector3(materialRb.velocity.x, 0, materialRb.velocity.z);
    }
}



