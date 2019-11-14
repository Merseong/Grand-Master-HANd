using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardP : BossPattern
{
    public GameObject guardPiece;
    int phase;

    public override void StartPattern()
    {
        phase = GameManager.inst.boss.phase;
        SelectTarget();
        AttackReady();
    }

    protected override void SelectTarget()
    {
        targets.Enqueue(new Vector2Int(2, 5));
        targets.Enqueue(new Vector2Int(5, 5));
    }
    public void AttackReady()
    {
        var board = GameManager.inst.chessBoard;
        var boss = GameManager.inst.boss;

        for (int i = 0; i < 2; i++)
        {
            var atkPos = targets.Dequeue();
            float disappearTime = Random.Range(2, 2.5f);
            for(int j =-2;j<3; j++)
            {
                for(int k = -1; k <3; k++)
                {
                    boss.AttackOnBoard(new Vector2Int(atkPos.x + j, atkPos.y + k), disappearTime, true);
                }
            }
            GameObject obj = Instantiate(guardPiece, board.IndexToGlobalPos(atkPos.x, atkPos.y), Quaternion.identity);
            obj.GetComponent<GuardPiece>().Throw(disappearTime);
        }
    }
}
