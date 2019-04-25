using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour {

    Material m_material;

    private void OnEnable()
    {
        AvatarGameManager.bulletTimeEvent += FadeRed;
        AvatarGameManager.realTimeEvent += Transparent;
    }

    private void OnDisable()
    {
        AvatarGameManager.bulletTimeEvent -= FadeRed;
        AvatarGameManager.realTimeEvent -= Transparent;
    }

    void FadeRed()
    {
        m_material.color = new Color(255, 0, 0, 10f/255f);
    }


    void Transparent()
    {
        m_material.color = new Color(255, 0, 0, 0);
    }

    // Use this for initialization
    void Start () {
        m_material = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
