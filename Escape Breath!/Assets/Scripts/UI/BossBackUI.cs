using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BossBackUI : MonoBehaviour
{
    [Header("UIs")]
    public GameObject promotionUI;
    private GameObject[] promotionUIinst = new GameObject[2]
        {
            null, null
        };

    [Header("Controller Assign")]
    public SteamVR_Action_Vector2 touchPositionAction;
    public SteamVR_Action_Boolean touchClickAction;
    public SteamVR_Action_Vibration hapticAction;


    public void ShowPromotionUI(bool isRight)
    {
        int instIdx = isRight ? 1 : 0;
        if (promotionUIinst[instIdx] != null) return;
        else
        {
            promotionUIinst[instIdx] = Instantiate(promotionUI, transform);
            promotionUIinst[instIdx].transform.localPosition = new Vector2(isRight ? 60 : -60, 10);
            promotionUIinst[instIdx].transform.localScale = new Vector2(0, 0);
            StartCoroutine(PromotionUICoroutine(promotionUIinst[instIdx].transform, isRight));
        }
    }
    IEnumerator HapticCoroutine(bool isRight)
    {
        while (true)
        {
            hapticAction.Execute(1, 0.1f, 64, 2, isRight ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand);
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator PromotionUICoroutine(Transform tr, bool isRight)
    {
        var haptic = StartCoroutine(HapticCoroutine(isRight));
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            tr.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, timer);
            yield return null;
        }
        tr.localScale = Vector2.one;

        // children num: 0: queen, 1: knight, 2: bishop, 3: rook
        Transform selector = tr.GetChild(4);
        int targetNum = -1;
        SteamVR_Input_Sources handType = isRight ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
        while (true)
        {
            hapticAction.Execute(0, 0.1f, 2, 2, handType);
            selector.localPosition = touchPositionAction.GetAxis(handType) * 12f;
            int xIdx = selector.localPosition.x > 7 ? 1 : (selector.localPosition.x < -7 ? -1 : 0);
            int yIdx = selector.localPosition.y > 7 ? 1 : (selector.localPosition.y < -7 ? -1 : 0);
            if (xIdx == 0 && yIdx == 0)
            {
                targetNum = -1;
            }
            else if (xIdx == 0)
            {
                if (yIdx > 0) targetNum = 0;
                else if (yIdx < 0) targetNum = 3;
            }
            else if (yIdx == 0)
            {
                if (xIdx > 0) targetNum = 1;
                else if (xIdx < 0) targetNum = 2;
            }
            else targetNum = -1;

            //Debug.LogError(targetNum);

            if (targetNum > -1)
            {
                for (int i = 0; i < 4; ++i) if (i != targetNum) tr.GetChild(i).localScale = Vector3.one;
                tr.GetChild(targetNum).localScale = Vector3.one * 1.5f;
            }
            else
            {
                for (int i = 0; i < 4; ++i) tr.GetChild(i).localScale = Vector3.one;
            }

            if (touchClickAction.GetStateDown(handType) && targetNum > -1)
            {
                GameManager.inst.chessBoard.PromotePawns(isRight, targetNum);
                StopCoroutine(haptic);
                break;
            }
            yield return null;
        }

        Destroy(tr.gameObject);
        promotionUIinst[isRight ? 1 : 0] = null;
    }
}
