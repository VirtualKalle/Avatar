using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public float currenctTime;
    private float travelTime;
    private float maxRotError = 0.001f;
    private float maxPosError = 0.001f;
    private float replaySpeed = 10;

    public Vector3 startPosition;
    public Quaternion startRotation;

    public Vector3 recordedTargetPosition;
    public Quaternion recordedTargetRotation;

    [SerializeField] GameObject muzzle;
    public Transform liveTargetTransform;

    public WarpManager m_warpManager;
    public GunManager m_gunManager;

    TransformRecording recordedTarget;

    void Start()
    {
        travelTime = m_warpManager.recordTimeInterval;
    }

    private void OnEnable()
    {
        SetFirstTransform();
    }

    void Update()
    {
        LerpTransform();
    }

    void LerpTransform()
    {
        CheckDistanceToTarget();

        if (m_warpManager.recording)
        {
            currenctTime += (Time.deltaTime) / travelTime;
        }
        else
        {
            currenctTime += (Time.deltaTime * replaySpeed) / travelTime;
        }

        if (startPosition == null || startRotation == null)
        {
            startPosition = liveTargetTransform.position;
            startRotation = liveTargetTransform.rotation;
        }

        transform.position = Vector3.Lerp(startPosition, recordedTarget.position, currenctTime);
        transform.rotation = Quaternion.Lerp(startRotation, recordedTarget.rotation, currenctTime);

    }

    void CheckDistanceToTarget()
    {
        float posError = Vector3.Magnitude(transform.position - recordedTarget.position);
        float rotError = Quaternion.Angle(transform.rotation, recordedTarget.rotation);

        if (rotError < maxRotError && posError < maxPosError && currenctTime >= 1)
        {
            if (recordedTarget.shoot)
            {
                m_gunManager.Shoot(muzzle.transform.position, muzzle.transform.rotation);
                m_warpManager.RemoveAction(0);
            }
            SetNextTransform();
        }
    }

    public void SetFirstTransform()
    {
        recordedTarget.position = liveTargetTransform.position;
        recordedTarget.rotation = liveTargetTransform.rotation;
        startPosition = liveTargetTransform.position;
        startRotation = liveTargetTransform.rotation;
        currenctTime = 0;
    }

    public void SetNextTransform()
    {
        recordedTarget = m_warpManager.GetNextTargetTransform();
        startPosition = transform.position;
        startRotation = transform.rotation;
        currenctTime = 0;
    }
}
