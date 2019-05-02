using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarGameManager : MonoBehaviour
{

    public static bool debugMode;
    public static bool bulletTime;
    public static bool paused;
    public static float timeDivisor = 10f;
    public static float timeScaler;
    public static float worldScale;
    

    public GameObject gameSpace;

    public delegate void GameManagerDelegate();
    public static GameManagerDelegate bulletTimeEvent;
    public static GameManagerDelegate realTimeEvent;
    private static float previousTimeScale;
    [SerializeField] Slider bulletTimeSlider;

    private float bulletTimeLeft;


    // Use this for initialization
    private void Awake()
    {
        timeScaler = 1 / timeDivisor;
        worldScale = gameSpace.transform.lossyScale.magnitude / Mathf.Sqrt(3);
    }

    void Start()
    {
        //Debug.Log("worldScale " + worldScale);
    }

    void SetBulletTime()
    {

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

    public static void Pause()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0;
        paused = true;
    }

    public static void UnPause()
    {
        Time.timeScale = previousTimeScale;
        paused = false;
    }




    // Update is called once per frame
    void Update()
    {

        //if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick) || Input.GetKeyDown(KeyCode.O))
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.O))
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

        if (bulletTime)
        {
            bulletTimeSlider.value -= Time.unscaledDeltaTime * 0.2f;
            if (bulletTimeSlider.value <= 0)
            {
                SetRealTime();
            }
        }
        else
        {
            if (bulletTimeLeft < 1)
            {
                bulletTimeSlider.value += Time.unscaledDeltaTime * 0.1f;
                bulletTimeSlider.value = Mathf.Min(1, bulletTimeSlider.value);
            }

        }

    }
}
