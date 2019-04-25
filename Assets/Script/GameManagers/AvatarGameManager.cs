using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarGameManager : MonoBehaviour {

    public static bool DebugMode;
    public static bool bulletTime;
    public static float timeDivisor = 10f;
    public static float timeScaler;

    [SerializeField] Text time;
    [SerializeField] Text unscaledTime;

    public delegate void GameManagerDelegate();
    public static GameManagerDelegate bulletTimeEvent;
    public static GameManagerDelegate realTimeEvent;


    // Use this for initialization
    void Start () {
        timeScaler = 1/timeDivisor;

    }

    void SetBulletTime()
    {
        //Time.timeScale = 1 / timeDivisor;
        //Time.fixedDeltaTime = Time.fixedDeltaTime / timeDivisor;

        Time.timeScale = 1 * timeScaler;
        Time.fixedDeltaTime = Time.fixedDeltaTime * timeScaler;
        bulletTime = true;
        bulletTimeEvent();
    }

    void SetRealTime()
    {
        Time.timeScale = 1f;
        //Time.fixedDeltaTime *= timeDivisor;

        Time.fixedDeltaTime /= timeScaler;
        bulletTime = false;
        realTimeEvent();
    }

    // Update is called once per frame
    void Update () {

        //if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick) || Input.GetKeyDown(KeyCode.O))
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || Input.GetKeyDown(KeyCode.O))
        {
            if (bulletTime)
            {
                SetRealTime();
            }
            else if (!bulletTime)
            {
                SetBulletTime();
            }
        }
	}
}
