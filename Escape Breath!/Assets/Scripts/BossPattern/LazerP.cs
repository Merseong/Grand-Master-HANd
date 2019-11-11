using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerP : BossPattern
{
    public GameObject lazerPiece;
    Boss boss = GameManager.inst.boss;
    ChessBoard board = GameManager.inst.chessBoard;
    int phase;
    int checkKing;

    public override void StartPattern()
    {
        phase = boss.phase;
        SelectTarget();
        AttackReady();
    }
    protected override void SelectTarget()
    {
        Vector2Int kingPos = board.GetRandomPiece(PieceType.King);

        if (kingPos.x <0&& kingPos.y<0)
        {
            Debug.Log("king is out!");
            checkKing = 0;
        }
        else
        {
            targets.Enqueue(kingPos);
            checkKing = 1;
        }
        for (int i = 0; i < (phase + 1) * 4; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = Random.Range(1, 8);
            pos.y = Random.Range(1, 4);
            if (board.CheckPiece(pos))
            {
                targets.Enqueue(pos);
            }
            else
            {
                i--;
            }
        }
    }
    public void AttackReady()
    {
        for(int i =0; i < (phase + 1) * 4 + checkKing; i++)
        {
            var atkPos = targets.Dequeue();
            float disappearTime = Random.Range(2, 2.5f);
            GameObject obj = Instantiate(lazerPiece, board.IndexToGlobalPos(atkPos.x, atkPos.y), Quaternion.identity);
            boss.AttackOnBoard(atkPos, disappearTime, true);
            boss.AttackOnBoard(new Vector2Int((atkPos.x + 1), atkPos.y), disappearTime, false);
            boss.AttackOnBoard(new Vector2Int((atkPos.x - 1), atkPos.y), disappearTime, false);
            boss.AttackOnBoard(new Vector2Int(atkPos.x, (atkPos.y + 1)), disappearTime, false);
            boss.AttackOnBoard(new Vector2Int(atkPos.x, (atkPos.y - 1)), disappearTime, false);
            obj.GetComponent<LazerPiece>().Throw(disappearTime);
        }
    }
}
