using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarGameManager : MonoBehaviour
{
    public static bool bulletTime;
    public static bool paused;

    private float bulletTimeLeft;
    public static float timeDivisor = 10f;
    public static float timeScaler;
    public static float worldScale;
    private static float previousTimeScale;
    
    public GameObject gameSpace;

    public delegate void GameManagerDelegate();
    public static GameManagerDelegate bulletTimeEvent;
    public static GameManagerDelegate realTimeEvent;

    [SerializeField] Slider bulletTimeSlider;



    private void Awake()
    {
        timeScaler = 1 / timeDivisor;
        worldScale = gameSpace.transform.lossyScale.magnitude / Mathf.Sqrt(3);
    }

    void Update()
    {
        CheckBulletTime();
        BulletTimeCounter();
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

    void CheckBulletTime()
    {
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
    }

    void BulletTimeCounter()
    {
        if (bulletTime && !paused)
        {
            bulletTimeSlider.value -= Time.unscaledDeltaTime * 0.1f;
            if (bulletTimeSlider.value <= 0)
            {
                SetRealTime();
            }
        }
        else
        {
            if (bulletTimeSlider.value < 1 && !paused)
            {
                bulletTimeSlider.value += Time.unscaledDeltaTime * 0.3f;
                bulletTimeSlider.value = Mathf.Min(1, bulletTimeSlider.value);
            }

        }
    }

}
