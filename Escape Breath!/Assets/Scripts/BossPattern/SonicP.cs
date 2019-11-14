using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicP : BossPattern
{
    public GameObject sonicPiece;
    Boss boss;
    ChessBoard board;
    int phase;

    public override void StartPattern()
    {
        Debug.Log("Start Sonic");
        boss = GameManager.inst.boss;
        board = GameManager.inst.chessBoard;
        phase = boss.phase;
        SelectTarget();
        AttackReady();
    }
    protected override void SelectTarget()
    {
        for(int i = 0; i < phase+1; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = Random.Range(0, 8);
            pos.y = 5;
            targets.Enqueue(pos);
        }
    }
    public void AttackReady()
    {
        for (int i =0; i<phase+1; i++)
        {
            var atkPos = targets.Dequeue();
            float disappearTime = Random.Range(2, 2.5f);
            for (int j = 0; j < 8; j++)
            {
                boss.AttackOnBoard(new Vector2Int(atkPos.x, j), disappearTime, true);
            }
            Debug.Log("AttackOnBoard done");
            GameObject obj = Instantiate(sonicPiece, board.IndexToGlobalPos(atkPos.x, 5), Quaternion.identity);
            obj.GetComponent<SonicPiece>().Fire(disappearTime);
        }
    }
}
