using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerTest : MonoBehaviour
{
    // temporary
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean touchPadAction;
    public SteamVR_Action_Vector2 touchPositionAction;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean grabGripAction;
    public SteamVR_Action_Vibration hapticAction;

    public float frequency;
    public float amplitute;
    public float duration;

    private void Start()
    {
        StartCoroutine(HapticCoroutine());
        //if (touchPadAction.GetState(handType))
        //{
        //    Time.timeScale = (touchPositionAction.GetAxis(handType).y + 1) / 2;
        //}
        //else Time.timeScale = 1;
        //if (grabGripAction.GetStateDown(handType))
        //{
        //    GameManager.inst.chessBoard.pieceList.ForEach((Piece p) =>
        //    {
        //        p.AutoAttack();
        //    });
        //}
    }

    IEnumerator HapticCoroutine()
    {
        while (true)
        {
            hapticAction.Execute(1, 0.1f, 64, 2, handType);
            yield return new WaitForSeconds(1);
        }
    }
}
