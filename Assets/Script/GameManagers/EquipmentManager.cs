using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {


    [SerializeField] List<Item> rightHand;
    [SerializeField] List<Item> leftHand;


    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            if (rightHand[0].m_itemState == ItemState.holstered && rightHand[1].m_itemState != ItemState.unholstering)
            {
                rightHand[0].Unholster();
                rightHand[1].Holster();
            }
            else if (rightHand[0].m_itemState == ItemState.unholstered)
            {

                rightHand[0].Holster();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            if (rightHand[1].m_itemState == ItemState.holstered && rightHand[0].m_itemState != ItemState.unholstering)
            {
                rightHand[1].Unholster();
                rightHand[0].Holster();
            }
            else if (rightHand[1].m_itemState == ItemState.unholstered)
            {

                rightHand[1].Holster();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            if (leftHand[0].m_itemState == ItemState.holstered)
            {
                leftHand[0].Unholster();
                leftHand[1].Holster();
            }
            else if (leftHand[0].m_itemState == ItemState.unholstered)
            {

                leftHand[0].Holster();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            if (leftHand[1].m_itemState == ItemState.holstered)
            {
                leftHand[1].Unholster();
                leftHand[0].Holster();
            }
            else if (leftHand[1].m_itemState == ItemState.unholstered)
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
