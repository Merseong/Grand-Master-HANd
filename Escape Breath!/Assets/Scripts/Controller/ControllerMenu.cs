using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerMenu : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean menuButtonAction;

    private bool isMenuOn = false;

    private void Update()
    {
        if (!isMenuOn && menuButtonAction.GetState(handType))
        {
            var menus = GameManager.inst.hmdUI.menuObjs;
            for (int i = 0; i < menus.Length; ++i)
            {
                menus[i].SetActive(true);
            }
            isMenuOn = true;
        }
        else if (isMenuOn && !menuButtonAction.GetState(handType))
        {
            var menus = GameManager.inst.hmdUI.menuObjs;
            for (int i = 0; i < menus.Length; ++i)
            {
                menus[i].SetActive(false);
            }
            isMenuOn = false;
        }
    }
}
