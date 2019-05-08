using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvatarMovement : MonoBehaviour
{

    bool rotated;
    bool moveXKey;
    bool moveYKey;
    bool autoMove;
    bool startedMoving;
    bool isJumping;

    float rotate;
    float meanTimeLeft;

    Vector2 move;
    Vector2 moveInput;
    Vector2 direction;

    Vector3 currentVelocity;
    Vector3 distance;
    Vector3 lastPos;
    Vector3 jumpVelocity;
    Vector3 fwd;
    Vector3 right;

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Animator m_animator;
    [SerializeField] Collider m_Collider;
    [SerializeField] Transform destinationPointer;

    NavMeshAgent nav;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!AvatarHealth.isDead)
        {
            GetInput();
            Move();
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

    void OnTriggerEnter(Collider col)
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

    
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Floor") && isJumping && rb.velocity.y > 0)
        {
            Debug.Log("jumped");
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
        lastPos = transform.position;
    }

    void Jump()
    {

        if ((OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Space)) && !isJumping)
        {
            nav.enabled = false;
            rb.isKinematic = false;
            m_Collider.isTrigger = true;
            isJumping = true;
            rb.AddForce(Vector3.up * 50, ForceMode.Impulse);
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
                moveXKey = false;
                moveYKey = false;


            if (Input.GetKey(KeyCode.W))
            {
                move.y += 2 * Time.deltaTime;
                move.y = Mathf.Max(0, move.y);
                move.y = Mathf.Min(1, move.y);
                moveYKey = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                move.y -= 2 * Time.deltaTime;
                move.y = Mathf.Min(0, move.y);
                move.y = Mathf.Max(-1, move.y);
                moveYKey = true;
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
                moveXKey = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                move.x -= 2 * Time.deltaTime;
                move.x = Mathf.Min(0, move.x);
                move.x = Mathf.Max(-1, move.x);
                moveXKey = true;
            }

            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                move.x = 0;
            }

        }

    }

    void Move()
    {

        if ((OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).magnitude > 0.1f || moveYKey || moveXKey))
        {
            if (!startedMoving)
            {
                fwd = Vector3.ProjectOnPlane((transform.position - Camera.main.transform.position), Vector3.up).normalized;
                right = Vector3.Cross((transform.position - Camera.main.transform.position), Vector3.up).normalized;
                startedMoving = true;
                autoMove = false;
            }

            nav.velocity = move.y * fwd * AvatarGameManager.worldScale * moveSpeed + (-move.x * right * AvatarGameManager.worldScale * moveSpeed);

        }
        else if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).magnitude < 0.1f && moveYKey && moveXKey && startedMoving)
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
        float h1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        float v1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        if (h1 == 0f && v1 == 0f)
        {
            Vector3 curRot = transform.localEulerAngles; 
            Vector3 homeRot;
            if (curRot.y > 180f)
            {
                homeRot = new Vector3(0f, 359.999f, 0f);
            }
            else
            {
                homeRot = Vector3.zero;
            }
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(h1, v1) * 180 / Mathf.PI, 0f); // this does the actual rotaion according to inputs
        }
    }

}
