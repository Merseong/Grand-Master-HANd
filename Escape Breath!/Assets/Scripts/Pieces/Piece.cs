using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for test
using Valve.VR;

public abstract class Piece : MonoBehaviour
{
    public int damage = 3;
    public int moveLimit = 2;
    public Vector3 attackPos = new Vector3();
    public bool canMove = true;
    public bool isMoving = false;

    public void AutoAttack()
    {
        if (damage != 0)
        {
            // auto attack
            var attackObj = Instantiate(GameManager.inst.attackObj, transform.position + attackPos, Quaternion.identity).GetComponent<AttackObj>();
            attackObj.damage = damage;
            attackObj.Init();
        }
    }

    public abstract void PieceDestroy();

    // 들고있을때 이것저것 표시 (어디에 옮겨질지나, 그런거)
}
