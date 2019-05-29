using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpaceController : MonoBehaviour
{
    private bool rotated;
    [SerializeField] Transform rotateAround;


    IEnumerator LerpRotate(float degrees)
    {

        rotated = true;
        AvatarGameManager.Pause();

        float endAngle = transform.eulerAngles.y + degrees;
        float speed = 2;

        for (int i = 0; i < Mathf.Abs(degrees / speed); i++)
        {
            int rotation = endAngle - transform.eulerAngles.y > 0 ? 1 : -1;
            transform.RotateAround(rotateAround.position, new Vector3(0, 1, 0), rotation * speed);
            yield return new WaitForSecondsRealtime(0.01f);
        }

        transform.eulerAngles = new Vector3(0, endAngle, 0);
        rotated = false;
        AvatarGameManager.UnPause();

    }

    void Update()
    {
        float horizontal = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        float vertical = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        if (!rotated && (Mathf.Abs(horizontal) > 0.5f || Mathf.Abs(vertical) > 0.5f))
        {

            if (horizontal > 0.5f)
            {
                StartCoroutine(LerpRotate(90));
            }
            else if (horizontal < -0.5f)
            {
                StartCoroutine(LerpRotate(-90));
            }
            else if (vertical < -0.5f)
            {
                StartCoroutine(LerpRotate(180));
            }
            else if (vertical > 0.5f)
            {
            }   
        }
    }
}
