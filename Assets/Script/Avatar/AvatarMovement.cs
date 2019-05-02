using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvatarMovement : MonoBehaviour
{

    private bool rotated;
    [SerializeField] Animator m_animator;
    private Vector2 move;
    Vector2 moveInput;
    float rotate;
    private bool moveKey;
    Vector2 direction;
    [SerializeField] Transform destinationPointer;
    private Vector3 distance;
    Rigidbody rb;
    private float meanTimeLeft;
    private Vector3 currentVelocity;
    private float currentVelocityMag;
    private Vector3 lastPos;
    private float meanTimeInterval;
    private bool autoMove;
    private bool startedMoving;

    Camera mainCamera;

    Vector3 fwd;
    Vector3 right;
    [SerializeField] float moveSpeed = 2f;
    private NavMeshAgent nav;
    private bool isJumping;
    [SerializeField] Collider m_Collider;
    private Vector3 jumpVelocity;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!AvatarHealth.isDead)
        {
            GetInput();
            Move();
            //Twist();
            Rotate();
            AutoMoveToDestination();
            Velocity();
            UpdateAnimation();
            Jump();
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
        {
            autoMove = true;
            SetNewDestination();
        }
    }


    void SetNewDestination()
    {
        direction = move;
    }

    void AutoMoveToDestination()
    {

        if (autoMove)
        {
            nav.velocity = direction.y * fwd * AvatarGameManager.worldScale * moveSpeed + (-direction.x * right * AvatarGameManager.worldScale * moveSpeed);
        }

    }

    void Velocity()
    {

        currentVelocity = (transform.position - lastPos) / Time.deltaTime;

        currentVelocityMag = currentVelocity.magnitude;
        //Debug.Log("currentVelocity " + currentVelocity + "\n currentVelocityMag " + currentVelocity.magnitude);
        lastPos = transform.position;

        meanTimeLeft = meanTimeInterval;

    }


    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnCollisionExit(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Floor") && isJumping && rb.velocity.y < 0)
        {

            isJumping = false;
            m_Collider.isTrigger = false;
            nav.enabled = true;
            rb.isKinematic = true;
            m_animator.SetBool("Jumping", false);

        }
    }



    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Floor") && isJumping && rb.velocity.y > 0)
        {
            Debug.Log("jumped");
        }
    }

    void Jump()
    {
        if ((OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Space)) && !isJumping)
        {
            nav.enabled = false;
            rb.isKinematic = false;
            m_Collider.isTrigger = true;
            isJumping = true;
            rb.AddForce(Vector3.up * 60, ForceMode.Impulse);
            m_animator.SetBool("Jumping", true);


            jumpVelocity = currentVelocity;
        }
    }

    void UpdateAnimation()
    {
        m_animator.SetFloat("MoveFwd", currentVelocity.normalized.z);
        m_animator.SetFloat("MoveRight", currentVelocity.normalized.x);

        if (currentVelocity.magnitude > 0.01)
        {
            m_animator.SetBool("Moving", true);
        }
        else if (m_animator.GetBool("Moving"))
        {
            m_animator.SetBool("Moving", false);
        }

    }

    void GetInput()
    {

        if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).magnitude > 0.1)
        {
            move = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        }
        else
        {
            if (moveKey)
            {
                moveKey = false;
            }

            if (Input.GetKey(KeyCode.W))
            {
                move.y += 2 * Time.deltaTime;
                move.y = Mathf.Max(0, move.y);
                move.y = Mathf.Min(1, move.y);
                moveKey = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                move.y -= 2 * Time.deltaTime;
                move.y = Mathf.Min(0, move.y);
                move.y = Mathf.Max(-1, move.y);
                moveKey = true;
            }

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                move.y = 0;
            }

            if (Input.GetKey(KeyCode.D))
            {
                move.x += 2 * Time.deltaTime;
                move.x = Mathf.Max(0, move.x);
                move.x = Mathf.Min(1, move.x);
                moveKey = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                move.x -= 2 * Time.deltaTime;
                move.x = Mathf.Min(0, move.x);
                move.x = Mathf.Max(-1, move.x);
                moveKey = true;
            }

            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                move.x = 0;
            }


        }

        if (Mathf.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x) > 0.1)
        {
            rotate = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        }
        else
        {
            if (Input.GetKey(KeyCode.E))
            {
                rotate = 1;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                rotate = -1;
            }
            else
            {
                rotate = 0;
            }
        }
    }

    private void Move()
    {

        if ((OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).magnitude > 0.1f || moveKey))
        {
            //Debug.Log("Input " + move);
            if (!startedMoving)
            {
                fwd = Vector3.ProjectOnPlane((transform.position - Camera.main.transform.position), Vector3.up).normalized;
                right = Vector3.Cross((transform.position - Camera.main.transform.position), Vector3.up).normalized;
                startedMoving = true;
                autoMove = false;
            }

            nav.velocity = move.y * fwd * AvatarGameManager.worldScale * moveSpeed + (-move.x * right * AvatarGameManager.worldScale * moveSpeed);

        }
        else if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).magnitude < 0.1f && !moveKey && startedMoving)
        {
            startedMoving = false;
        }

        if (isJumping)
        {
            transform.Translate(jumpVelocity * Time.deltaTime, Space.World);
        }
    }

    void Rotate()
    {
        Camera camera = Camera.main;
        Vector3 direction = Vector3.ProjectOnPlane(transform.position - camera.transform.position, new Vector3(0, 1, 0));

        transform.Rotate(new Vector3(0, 1, 0), Vector3.SignedAngle(transform.forward, direction, new Vector3(0, 1, 0)), Space.World);

    }

    void Twist()
    {
        //h1 = CrossPlatformInputManager.GetAxis("Horizontal"); // set as your inputs 
        float h1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x; // set as your inputs 
        float v1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        if (h1 == 0f && v1 == 0f)
        { // this statement allows it to recenter once the inputs are at zero 
            Vector3 curRot = transform.localEulerAngles; // the object you are rotating
            Vector3 homeRot;
            if (curRot.y > 180f)
            { // this section determines the direction it returns home 
              //Debug.Log(curRot.y);
                homeRot = new Vector3(0f, 359.999f, 0f); //it doesnt return to perfect zero 
            }
            else
            {                                                                      // otherwise it rotates wrong direction 
                homeRot = Vector3.zero;
            }
            //transform.localEulerAngles = Vector3.Slerp(curRot, homeRot, Time.deltaTime * 2);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(h1, v1) * 180 / Mathf.PI, 0f); // this does the actual rotaion according to inputs
        }
    }

}
