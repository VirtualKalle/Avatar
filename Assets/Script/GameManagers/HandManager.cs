using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public enum Hand { Left,Right}

public class HandManager : MonoBehaviour {

    public Hand m_hand = Hand.Left;


	// Use this for initialization
	void Start () {
		
	}
	
    //bool isUsing()
    //{
    //    if (m_hand == Hand.Left && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
    //    {
    //        return
    //    }

    //}

	// Update is called once per frame
	void Update () {
		
	}
}
