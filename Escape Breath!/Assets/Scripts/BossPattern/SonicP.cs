using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicP : BossPattern
{
    public GameObject sonicPiece;
    int phase;

    public override void StartPattern()
    {
        Boss boss = GameManager.inst.boss;
        phase = boss.phase;
    }
    protected override void SelectTarget()
    {
        for(int i = 0; i < (phase); i++)
        {

        }
    }

    public void Attack()
    {

    }
}
