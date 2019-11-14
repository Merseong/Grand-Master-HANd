using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorP : BossPattern
{
    public GameObject meteorPiece;
    int phase;
    
    public override void StartPattern()
    {
        phase = GameManager.inst.boss.phase;
        SelectTarget();
        AttackReady();
    }

    protected override void SelectTarget()
    {
        for (int i = 0; i < (phase + 1) * 3; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = Random.Range(0, 8);
            pos.y = Random.Range(0, 4);
            targets.Enqueue(pos);
        }
    }
    public void AttackReady()
    {
        var board = GameManager.inst.chessBoard;
        var boss = GameManager.inst.boss;

        for (int i = 0; i < (phase + 1) * 3; i++)
        {
            var atkPos = targets.Dequeue();
            float disappearTime = Random.Range(2, 2.5f);
            boss.AttackOnBoard(atkPos, disappearTime, true);
            GameObject obj = Instantiate(meteorPiece, boss.attackPoint.position, Quaternion.identity);
            obj.GetComponent<MeteorPiece>().Throw(board.IndexToGlobalPos(atkPos.x, atkPos.y), disappearTime);
        }
    }
}
