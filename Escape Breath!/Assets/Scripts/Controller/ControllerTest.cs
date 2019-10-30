using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerTest : MonoBehaviour
{
    // temporary
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean touchPadAction;
    public SteamVR_Action_Vector2 touchPositionAction;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean grabGripAction;

    private void Update()
    {
        //if (touchPadAction.GetState(handType))
        //{
        //    Time.timeScale = (touchPositionAction.GetAxis(handType).y + 1) / 2;
        //}
        //else Time.timeScale = 1;
        if (grabGripAction.GetStateDown(handType))
        {
            GameManager.inst.chessBoard.pieceList.ForEach((Piece p) =>
            {
                p.AutoAttack();
            });
        }
    }
}
