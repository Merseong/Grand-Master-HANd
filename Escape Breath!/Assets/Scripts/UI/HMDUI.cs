using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMDUI : MonoBehaviour
{
    public Text turnText;
    public Image turnImage;

    private float turnTime = 0;

    public void ShowTurn(TurnType current, float time)
    {
        turnText.text = current.ToString();
        float widthScale;
        switch (current)
        {
            case TurnType.MovePiece:
                turnTime = 0;
                widthScale = 1 - time / GameManager.inst.turnSystem.turnTimers[current];
                turnImage.color = new Color(0, 1, 0.5f);
                break;
            case TurnType.Attack:
            case TurnType.AttackReady:
                turnTime = turnTime > 1 ? time + 1 : time;
                widthScale = turnTime * 0.5f;
                turnImage.color = Color.red;
                break;
            default:
                Debug.LogError("[ERR-ShowTurn] default called");
                widthScale = 1;
                break;
        }
        turnImage.fillAmount = widthScale;
    }
}
