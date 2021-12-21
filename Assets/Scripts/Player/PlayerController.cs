using System;
using UnityEngine;
using Photon.Pun;

public class PlayerController: MonoBehaviour
{
    //VARIABLES

    //Photon
    public PhotonView PV;

    //PLAYER
        private Rigidbody rb;
        public CapsuleCollider body;

    //SENSITIVITY
        public float baseSpeed = 10f;
        public float moveSpeed = 10f;
        public float lookSensitivity = 10f;

    //WASD
        public float xMove;
        public float zMove;
        public Vector3 vel;

    //JUMP
        public bool isJumping = false;
        public float jumpForce = 8f;
        public float extraJumpsMax = 1f;
        private float extraJumps;
        public bool isGrounded;
        public LayerMask groundMask;
        public GameObject groundCheck;

    //WALLRUN
        public LayerMask wallMask;
        public bool isWallLeft, isWallRight, isWallRunning;
        public float wallRunCameraTiltMax = 25f;
        private float wallRunCameraTilt;
        public float wallRunForce = 10f;
        public float wallRunMaxSpeed = 20f;
        public float wallRunFallSpeed = -2f;

    //SPRINT
        public bool isSprinting = false;
        public float sprintSpeed = 20f;

    //CROUCH/SLIDE
        public bool isCrouching = false;
        public bool isSliding = false;
        public float crouchSpeed = 5f;
        public float slideSpeed = 15f;
        private float baseGroundCheckHeight;
        public float crouchHeightMultiplier = 0.5f;

        //CAMERA
        public Camera cam;
        private CameraMove camMove;
        public Transform camStandHeight;
        public Transform camCrouchHeight;
        public Transform camSlideHeight;
        public Transform camCrouchWalkHeight;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = baseSpeed;
        camMove = cam.GetComponent<CameraMove>();
        baseGroundCheckHeight = groundCheck.transform.localPosition.y;

        //setting flag for game being paused, otherwise will start the game with pause canvas open
        PauseMenu.GameIsPaused = false;

    }
    
    void Awake()
    {
            PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV != null && !PV.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }
        if (!PauseMenu.GameIsPaused)
        {
            vel = rb.velocity;
            //CHECK STATES
            isGrounded = Physics.CheckSphere(new Vector3(groundCheck.transform.position.x, groundCheck.transform.position.y, transform.position.z), 0.5f, groundMask);
            isWallLeft = Physics.Raycast(transform.position, -transform.right, 2f, wallMask);
            isWallRight = Physics.Raycast(transform.position, transform.right, 2f, wallMask);

            //WASD
            xMove = Input.GetAxisRaw("Horizontal");
            zMove = Input.GetAxisRaw("Vertical");

            //WALLRUN
            if (!isWallLeft && !isWallRight)
            {
                rb.useGravity = true;
                isWallRunning = false;
                if (wallRunCameraTilt > 0)
                    wallRunCameraTilt -= Time.deltaTime * wallRunCameraTiltMax * 2;
                if (wallRunCameraTilt < 0)
                    wallRunCameraTilt += Time.deltaTime * wallRunCameraTiltMax * 2;
            }
            else if (Input.GetKey(KeyCode.W) && !isGrounded)
            {
                rb.useGravity = false;
                isWallRunning = true;
                if (rb.velocity.magnitude <= wallRunMaxSpeed)
                {
                    rb.AddForce(transform.forward * wallRunForce * 5f);
                    if (isWallLeft)
                        rb.AddForce(-transform.right * wallRunForce / 5);
                    if (isWallRight)
                        rb.AddForce(transform.right * wallRunForce / 5);
                }
                rb.AddForce(transform.up * wallRunFallSpeed);
                if (Math.Abs(wallRunCameraTilt) < wallRunCameraTiltMax && isWallRunning && isWallRight)
                    wallRunCameraTilt += Time.deltaTime * wallRunCameraTiltMax * 2;
                if (Math.Abs(wallRunCameraTilt) < wallRunCameraTiltMax && isWallRunning && isWallLeft)
                    wallRunCameraTilt -= Time.deltaTime * wallRunCameraTiltMax * 2;

                //Wall Jumping
                if (isWallRunning && Input.GetKeyDown(KeyCode.Space))
                {
                    if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) rb.AddForce(-transform.up * jumpForce * 50f);
                    if (isWallRight && Input.GetKey(KeyCode.A)) rb.AddForce(-transform.right * jumpForce * 25f);
                    if (isWallLeft && Input.GetKey(KeyCode.D)) rb.AddForce(transform.right * jumpForce * 25f);

                    //Always add forward force
                    rb.AddForce(transform.forward * jumpForce * 5f);

                    //reset velocity
                    rb.velocity = Vector3.zero;
                }
            }        

            //JUMP
            if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !isSliding && !isCrouching)
            {
                isJumping = true;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                extraJumps--;
            }
            if (isGrounded || isWallRunning)
                extraJumps = extraJumpsMax;


            //CROUCH
            if (Input.GetKeyDown(KeyCode.C) && isGrounded && isSprinting && Math.Abs(vel.x) + Math.Abs(vel.z) > 1)
            {
                
                
                if (!isSliding)
                {
                    rb.AddForce(transform.forward * slideSpeed, ForceMode.Impulse);
                }

                isSliding = true;
                isCrouching = false;

                body.height = 1.25f;
                body.transform.position = new Vector3(body.transform.position.x, groundCheck.transform.position.y + (body.height / 2), body.transform.position.z);

                float gcHeight = baseGroundCheckHeight * crouchHeightMultiplier;
                groundCheck.transform.localPosition = new Vector3(0, gcHeight, 0);
                


            }
            else if (Input.GetKey(KeyCode.C) && isGrounded && !isSprinting && Math.Abs(vel.x) + Math.Abs(vel.z) > 1)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camCrouchWalkHeight.position, Time.deltaTime * 5.0f);
            }
            else if (Input.GetKey(KeyCode.C) && isGrounded && !isSprinting)
            {
                isCrouching = true;
                isSliding = false;
                cam.transform.position = Vector3.Lerp(cam.transform.position, camCrouchHeight.position, Time.deltaTime * 5.0f);
                moveSpeed = crouchSpeed;
                
                body.height = 1.25f;
                body.transform.position = new Vector3(body.transform.position.x, groundCheck.transform.position.y + (body.height / 2), body.transform.position.z);

                float gcHeight = baseGroundCheckHeight * crouchHeightMultiplier;
                groundCheck.transform.localPosition = new Vector3(0, gcHeight, 0);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                isSliding = false;
                isCrouching = false;
                
              /*  if(body.height != 1.77)
                {
                    cam.transform.position = Vector3.Lerp(cam.transform.position, camStandHeight.position, Time.deltaTime * 5.0f);
                    camMove.MoveCam(camStandHeight);
                }*/

                body.height = 1.77f;
                body.transform.position = new Vector3(body.transform.position.x, groundCheck.transform.position.y + (body.height / 2), body.transform.position.z);

                groundCheck.transform.localPosition = new Vector3(0, baseGroundCheckHeight, 0);
            }
            if(!isSliding && !isCrouching)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camStandHeight.position, Time.deltaTime * 5.0f);
            }
            if (isSliding)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camSlideHeight.position, Time.deltaTime * 3.0f);
            }

            //SPRINT
            if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching && Math.Abs(vel.x) + Math.Abs(vel.z) > 1)
            {
                isSprinting = true;
                moveSpeed = sprintSpeed;
             
            }
            else
            {
                isSprinting = false;
            
            }

/*            //CAMERA FOR SPRINT
            if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                cam.fieldOfView = Mathf.Lerp(70, 60, Time.deltaTime * 1f);
            }
            else
            {
                cam.fieldOfView = Mathf.Lerp(60, 70, Time.deltaTime * 1f);
            }*/


            //MOVEMENT (Can only change direction if grounded or actively jumping)
            if ((isGrounded || isJumping) && !isSliding)
            {
                Vector2 moveHorizontal = new Vector2(xMove * transform.right.x, xMove * transform.right.z);
                Vector2 moveVertical = new Vector2(zMove * transform.forward.x, zMove * transform.forward.z);
                Vector2 velocity = (moveHorizontal + moveVertical).normalized * moveSpeed;
                rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
            }

            //CAMERA LOOK
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

            float xRotation = cam.transform.localEulerAngles.x;
            xRotation -= mouseY;

            rb.rotation *= Quaternion.Euler(0f, mouseX, 0f);
            cam.transform.localEulerAngles = new Vector3(xRotation, mouseX, wallRunCameraTilt);

            //RESET
            isJumping = false;
            if (!isWallRunning && !isSprinting && !isCrouching)
                moveSpeed = baseSpeed;
        }
    }

    private void MoveCamDown()
    {
        float timeSinceStarted = Time.time - 0;
        float percentageComplete = timeSinceStarted / 5.0f;
        cam.transform.forward = Vector3.Lerp(cam.transform.position, camCrouchHeight.position, percentageComplete);
    }
}
