using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using System;

public struct TransformRecording
{
    public Vector3 position;
    public Quaternion rotation;
    public bool shoot;

    public TransformRecording(Vector3 position, Quaternion rotation, bool shoot) : this()
    {
        this.position = position;
        this.rotation = rotation;
        this.shoot = shoot;
    }
}

public class GunManager : MonoBehaviour
{

    [SerializeField] ParticleSystem muzzleParticle;
    [SerializeField] ParticleSystem shellParticle;
    Item item;
    public float shootCoolDownTimeLeft;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject muzzle;

    bool isGrabbed;
    bool shootCoolDown;
    bool hasShot;
    float shootThreshold = 0.5f;

    [SerializeField] AudioClip gunShotAudioClip;
    AudioSource m_audioSorce;

    void Start()
    {
        item = GetComponent<Item>();
        m_audioSorce = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (item.m_itemState == ItemState.unholstered && !AvatarGameManager.paused && !AvatarHealth.isDead && shootCoolDownTimeLeft <= 0)
        {
            OnTriggerShoot();
        }

        ShootCountDown();
    }

    void OnTriggerShoot()
    {
        if (item.m_hand == Hand.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > shootThreshold && !hasShot)
        {
            TryShoot();
        }
        else if (item.m_hand == Hand.Right && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > shootThreshold && !hasShot)
        {
            TryShoot();
        }
        else if(
                (((item.m_hand == Hand.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) < shootThreshold) ||
                (item.m_hand == Hand.Right && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) < shootThreshold))) &&
                hasShot
                )
        {
            hasShot = false;
        }
    }

    void TryShoot()
    {
        if (!hasShot)
        {
            Shoot(muzzle.transform.position, muzzle.transform.rotation);
            muzzleParticle.Play();
            shellParticle.Play();
            m_audioSorce.PlayOneShot(gunShotAudioClip);

            if (AvatarGameManager.bulletTime)
            {
                m_audioSorce.pitch = 0.5f;
            }
            else
            {
                m_audioSorce.pitch = 1f;
            }
            hasShot = true;
        }
    }

    public void Shoot(Vector3 pos, Quaternion rot)
    {
        GameObject bulletClone = Instantiate(bullet, pos, rot);
        bulletClone.transform.localScale *= transform.lossyScale.magnitude;
    }

    private void ShootCountDown()
    {
        if (shootCoolDownTimeLeft > 0)
        {
            shootCoolDownTimeLeft -= Time.unscaledDeltaTime;
        }
    }

}
