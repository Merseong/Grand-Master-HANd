using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Attacker
{
    public override void BeforeAttack()
    {
        if (boardIdx.y == 7 && isAlive)
        {
            TurnOnPromotion();
        }
    }

    public void TurnOnPromotion()
    {
        Debug.Log("Ready Promotion on " + boardIdx);
        GameManager.inst.bossBackUI.ShowPromotionUI(boardIdx.x > 3);
    }
}
