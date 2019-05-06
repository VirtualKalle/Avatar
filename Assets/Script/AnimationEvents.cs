using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class AnimationEvents : MonoBehaviour {

    BipedIK m_BipedIK;

    private void Awake()
    {
        m_BipedIK = GetComponent<BipedIK>();
    }

    // Use this for initialization
    void Start () {

    }

    public void EnableIK()
    {
        m_BipedIK.enabled = true;
    }

    public void DisableIK()
    {
        m_BipedIK.enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
