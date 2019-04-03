using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static bool DebugMode;
    public static bool bulletTime;
    public static float timeDivisor = 10f;
    public static float timeScaler;

    [SerializeField] Text time;
    [SerializeField] Text unscaledTime;

    public delegate void GameManagerDelegate();
    public static GameManagerDelegate BulletTime;
    public static GameManagerDelegate RealTime;


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
        BulletTime();
    }

    void SetRealTime()
    {
        Time.timeScale = 1f;
        //Time.fixedDeltaTime *= timeDivisor;

        Time.fixedDeltaTime /= timeScaler;
        bulletTime = false;
        RealTime();
    }

    // Update is called once per frame
    void Update () {

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick) || Input.GetKeyDown(KeyCode.Q))
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
