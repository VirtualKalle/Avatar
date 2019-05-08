using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    [SerializeField] Item rightHand_Melee;
    [SerializeField] Item rightHand_Range;
    [SerializeField] Item leftHand_Melee;
    [SerializeField] Item leftHand_Range;


    void Update()
    {
        if (!AvatarGameManager.paused && !AvatarHealth.isDead)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            {
                rightHand_Melee.Unholster();
                rightHand_Range.Holster();
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            {
                rightHand_Range.Unholster();
                rightHand_Melee.Holster();
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {
                leftHand_Melee.Unholster();
                leftHand_Range.Holster();
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {
                leftHand_Melee.Holster();
                leftHand_Range.Unholster();
            }
        }
    }

}
