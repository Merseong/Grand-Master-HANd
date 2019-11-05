using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorP : BossPattern
{
    private GameObject meteorPiece;
    int phase = GameManager.inst.boss.phase;

    protected Queue<Vector2Int> targets;


    public override void StartPattern()
    {
        AttackReady();
    }

    protected override void SelectTarget()
    {
        for (int i = 0; i < (phase + 1) * 4; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = (int)Random.Range(1, 8);
            pos.y = (int)Random.Range(1, 4);
            targets.Enqueue(pos);
        }
    }
    public void AttackReady()
    {
        var board = GameManager.inst.chessBoard;
        var boss = GameManager.inst.boss;

        for (int i = 0; i < (phase + 1) * 4; i++)
        {
            var atkPos = targets.Dequeue();
            board.ShowAttackArea(atkPos, 1, true); //지속시간 변경해야댐 임시 숫자임
            GameObject obj = Instantiate(meteorPiece, boss.attackPoint);
            obj.GetComponent<MeteorPiece>().Throw(board.IndexToGlobalPos(atkPos.x, atkPos.y)); //구현 확인 요망
        }
        
    }
    public void Attack()
    {

    }
}
