using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnType
{
    AttackReady,
    MovePiece,
    Attack
}

public class TurnSystem : MonoBehaviour
{
    public int turnCount = 0;
    public TurnType currentTurn;
    public Dictionary<TurnType, float> turnTimers = new Dictionary<TurnType, float>()
    {
        { TurnType.AttackReady, 1f }, { TurnType.MovePiece, 10f }, { TurnType.Attack, 1f }
    };

    private float timeCounter;
    public TextMesh timeText;

    private void Start()
    {
        currentTurn = TurnType.MovePiece;
        timeCounter = 0;
    }

    private void Update()
    {
        timeCounter += Time.unscaledDeltaTime;
        if (timeCounter > turnTimers[currentTurn])
        {
            // invoke when turn timer end
            switch(currentTurn)
            {
                // do next turn's action
                case TurnType.AttackReady:
                    currentTurn = TurnType.MovePiece;
                    MovePiecePhase();
                    break;
                case TurnType.MovePiece:
                    currentTurn = TurnType.Attack;
                    AttackPhase();
                    break;
                case TurnType.Attack:
                    currentTurn = TurnType.AttackReady;
                    AttackReadyPhase();
                    break;
                default:
                    break;
            }
            timeCounter = 0;
        }

        timeText.text = turnCount.ToString() + "th " + currentTurn.ToString() + "\n" + timeCounter.ToString("f1");
    }

    private void AttackReadyPhase()
    {
        // show boss pattern
        turnCount++;
    }

    private void MovePiecePhase()
    {
        GameManager.inst.chessBoard.allReset();
        Time.timeScale = 1 / turnTimers[TurnType.MovePiece];
    }

    private void AttackPhase()
    {
        GameManager.inst.chessBoard.allAttack();
        // Boss Attack
        Time.timeScale = 1;
    }
}
