using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [SerializeField] List<GameObject> actionObjects;
    [SerializeField] GameObject trailObject;
    [SerializeField] GameObject ghostObject;
    [SerializeField] GameObject actionObject;
    GhostManager m_ghostManager;
    GunManager m_gunManager;

    public List<TransformRecording> transformRecordings { get; private set; }
    [HideInInspector] public float recordTimeInterval;
    float recordUnscaledTimeLeft;
    float trailTimeLeft;
    float trailInterval;
    private float spawnTrailInterval;
    private float spawnTrailTimeLeft;
    public bool recording;
    private TransformRecording transformRecording;
    private bool action;

    private void OnEnable()
    {
        AvatarGameManager.bulletTimeEvent += EnableGhostObject;
        AvatarGameManager.realTimeEvent += StopRecording;
    }

    private void OnDisable()
    {
        AvatarGameManager.bulletTimeEvent -= EnableGhostObject;
        AvatarGameManager.realTimeEvent -= StopRecording;
    }

    // Use this for initialization
    void Start()
    {
        recordTimeInterval = 0.1f;
        recordUnscaledTimeLeft = 0;
        spawnTrailInterval = 0.1f;
        m_gunManager = GetComponent<GunManager>();
        m_ghostManager = ghostObject.GetComponent<GhostManager>();
        m_ghostManager.m_warpManager = this;
        m_ghostManager.m_gunManager = m_gunManager;
        m_gunManager.m_warpManager = this;
        m_ghostManager.liveTargetTransform = transform;
        InitiateRecording();
    }

    // Update is called once per frame
    void Update()
    {

        if (AvatarGameManager.bulletTime)
        {
            RecordCountDown();
            SpawnTrail();
        }
    }

    void EnableGhostObject()
    {
        if (ghostObject != null)
        {
            m_ghostManager.SetFirstTransform();
            ghostObject.SetActive(true);

        }
    }

    void DisableGhostObject()
    {
        if (ghostObject != null)
        {
            ghostObject.SetActive(false);
        }
    }

    void InitiateRecording()
    {
        if (transformRecordings == null)
        {
            transformRecordings = new List<TransformRecording>();

        }
    }

    void StopRecording()
    {
        recording = false;
    }

    void RecordCountDown()
    {
        recordUnscaledTimeLeft -= Time.unscaledDeltaTime;

        if (recordUnscaledTimeLeft < 0)
        {
            RecordTransform();

            recordUnscaledTimeLeft = recordTimeInterval;

            //Debug.Log("RecordStates, recording.Count " + transformRecordings.Count);
        }
    }

    void RecordTransform()
    {
        transformRecordings.Add(new TransformRecording(transform.position, transform.rotation, action));

        if (action)
        {
            //SpawnAction();
            action = false;
        }

        if (recording == false)
        {
            recording = true;
        }
    }

    public void QueueAction()
    {
        action = true;
    }

    void SpawnAction()
    {
        actionObjects.Add(Instantiate(actionObject, transform.position, transform.rotation));
    }

    public void RemoveAction(int actionNr)
    {
        GameObject go = actionObjects[actionNr];
        actionObjects.RemoveAt(actionNr);
        Destroy(go);
    }

    void SpawnTrail()
    {
        spawnTrailTimeLeft -= Time.unscaledDeltaTime;
        if (spawnTrailTimeLeft < 0)
        {
            GameObject trailObjectClone = Instantiate(trailObject, transform.position, transform.rotation);
            Destroy(trailObjectClone, 0.1f);
            spawnTrailTimeLeft = spawnTrailInterval;
        }
    }


    public TransformRecording GetNextTargetTransform()
    {
        if (transformRecordings.Count == 0 && !AvatarGameManager.bulletTime)
        {
            DisableGhostObject();

        }

        if (transformRecordings.Count > 0)
        {
            transformRecording = transformRecordings[0];
            transformRecordings.RemoveAt(0);

            //Debug.Log("GetNextTargetTransform, recording.Count " + transformRecordings.Count);
        }


        return transformRecording;

    }

}

