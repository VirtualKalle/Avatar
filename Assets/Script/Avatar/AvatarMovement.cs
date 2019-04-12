using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovement : MonoBehaviour
{

    private bool rotated;
    [SerializeField] Animator m_animator;
    private Vector2 move;
    Vector2 moveInput;
    float rotate;
    private bool moveKey;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Twist();
        Rotate();
    }

    void GetInput()
    {

        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).magnitude > 0.1)
        {
            move = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
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
        GetInput();

        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).magnitude > 0.1f || moveKey)
        {
            Debug.Log("Input " + move);

            Vector3 fwd = Vector3.ProjectOnPlane((transform.position - Camera.main.transform.position), Vector3.up).normalized;
            transform.Translate(move.y * fwd * Time.deltaTime, Space.World);


            Vector3 right = Vector3.Cross((transform.position - Camera.main.transform.position), Vector3.up).normalized;
            transform.Translate(-move.x * right * Time.deltaTime, Space.World);
        }

        m_animator.SetFloat("MoveFwd", move.y);
        m_animator.SetFloat("MoveRight", move.x);
        if (move.magnitude > 0.01)
        {
            m_animator.SetBool("Moving", true);
        }
        else if (m_animator.GetBool("Moving"))
        {
            m_animator.SetBool("Moving", false);
        }

    }

    void Rotate()
    {


        if (rotate > 0.1f && !rotated)
        {
            transform.Rotate(Vector3.up, 90);
            rotated = true;
        }
        else if (rotate < -0.1f && !rotated)
        {
            transform.Rotate(Vector3.up, -90);
            rotated = true;
        }
        else if (rotate > -0.1f && rotate < 0.1f && rotated)
        {
            rotated = false;
        }

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
