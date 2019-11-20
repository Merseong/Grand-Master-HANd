using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadP : BossPattern
{
    public List<GameObject> bossPattern;

    public void Awake()
    {
        bossPattern = GameManager.inst.boss.phasePatterns;
    }

    public override void StartPattern()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                var pattern = Instantiate(bossPattern[j].GetComponent<BossPattern>());
                pattern.StartPattern();
            }
        }
    }
    protected override void SelectTarget()
    {
         
    }

}
