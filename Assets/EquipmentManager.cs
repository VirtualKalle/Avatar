﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

    [SerializeField] Item[] equipment;

    List<Item> rightHand;
    List<Item> leftHand;


    // Use this for initialization
    void Start ()
    {
        leftHand = new List<Item>();
        rightHand = new List<Item>();

        foreach (Item item in equipment)
        {
            if (item.m_hand == Hand.Left)
            {
                leftHand.Add(item);
            }
            else
            {
                rightHand.Add(item);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            if (rightHand[0].holstered)
            {
                rightHand[0].Unholster();
                rightHand[1].Holster();
            }
            else if (!rightHand[0].holstered)
            {

                rightHand[0].Holster();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            if (rightHand[1].holstered)
            {
                rightHand[1].Unholster();
                rightHand[0].Holster();
            }
            else if (!rightHand[1].holstered)
            {

                rightHand[1].Holster();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            if (leftHand[0].holstered)
            {
                leftHand[0].Unholster();
                leftHand[1].Holster();
            }
            else if (!leftHand[0].holstered)
            {

                leftHand[0].Holster();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            if (leftHand[1].holstered)
            {
                leftHand[1].Unholster();
                leftHand[0].Holster();
            }
            else if (!leftHand[1].holstered)
            {

                leftHand[1].Holster();
            }
        }
    }

    void Switch()
    {
        //Deactivate current eq
        //Move current
        //Move new
        //Activate new
    }

}
