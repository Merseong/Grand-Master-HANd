using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerP : BossPattern
{
    public GameObject lazerPiece;
    Boss boss = GameManager.inst.boss;
    ChessBoard board = GameManager.inst.chessBoard;
    int phase;

    public override void StartPattern()
    {

    }
    protected override void SelectTarget()
    {
        Vector2Int kingPos = board.GetRandomPiece(PieceType.King);

        if (kingPos.x <0&& kingPos.y<0)
        {
            Debug.Log("king is out!");
        }
        else
        {
            targets.Enqueue(kingPos);
        }
        for (int i = 0; i < (phase + 1) * 4; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = Random.Range(1, 8);
            pos.y = Random.Range(1, 4);
            targets.Enqueue(pos);
        }
    }
}
