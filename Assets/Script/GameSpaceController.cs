using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpaceController : MonoBehaviour
{
    private bool rotated;
    private float previousTimeScale;
    [SerializeField] Transform rotateAround;

    // Use this for initialization
    void Start()
    {

    }

    public void Rotate(int degrees)
    {
        transform.RotateAround(rotateAround.position, new Vector3(0,1,0), degrees);
    }



    // Update is called once per frame
    void Update()
    {
        float horizontal = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        float vertical = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        if (!rotated && (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f))
        {

            if (horizontal > 0.1f)
            {
                Rotate(90);
            }
            else if (horizontal < -0.1f)
            {
                Rotate(-90);
            }
            else if (vertical < -0.1f)
            {
                Rotate(180);
            }
            else if (vertical > 0.1f)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            rotated = true;
            AvatarGameManager.Pause();
        }
        else if (horizontal == 0 && vertical == 0 && rotated)
        {
            rotated = false;
            AvatarGameManager.UnPause();
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && AvatarGameManager.paused)
        {
            Time.timeScale = previousTimeScale;
            AvatarGameManager.UnPause();
        }



    }
}
