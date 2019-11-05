using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (inst != null) Destroy(inst);
        inst = this;
    }

    public Boss boss;
    public ChessBoard chessBoard;
    public TurnSystem turnSystem;

    public GameObject attackObj;
    public Material whiteMat;
    public Material blackMat;

    public void GameOver()
    {

    }

    public void GameClear()
    {

    }
}
