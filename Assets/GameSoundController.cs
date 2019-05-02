using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : MonoBehaviour
{

    [SerializeField] AudioSource m_AudioSource;
    private float pitchUpTarget = 3f;
    private float pitchDownTarget = 0.1f;

    enum Pitch { None, Up, Down };
    Pitch m_Pitch;

    void PitchUp()
    {
        if (m_Pitch == Pitch.Up)
        {

            if (m_AudioSource.pitch < pitchUpTarget)
            {

                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }
                m_AudioSource.pitch *= 1.2f;
            }
            else if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
                m_Pitch = Pitch.None;
            }
        }
    }

    void PitchDown()
    {
        if (m_Pitch == Pitch.Down)
        {
            if (m_AudioSource.pitch > pitchDownTarget)
            {

                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }

                m_AudioSource.pitch *= 0.9f;
            }
            else if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
                m_Pitch = Pitch.None;
            }
        }
    }

    void SetPitchDown()
    {
        m_Pitch = Pitch.Down;
    }

    void SetPitchUp()
    {
        m_Pitch = Pitch.Up;
    }

    private void OnEnable()
    {
        AvatarGameManager.bulletTimeEvent += SetPitchDown;
        AvatarGameManager.realTimeEvent += SetPitchUp;
    }

    private void OnDisable()
    {
        AvatarGameManager.bulletTimeEvent -= SetPitchDown;
        AvatarGameManager.realTimeEvent -= SetPitchUp;

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PitchUp();
        PitchDown();
    }
}
