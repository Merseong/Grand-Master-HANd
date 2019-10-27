using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AutoAttack();
        }
        if (Input.GetMouseButton(1))
        {
            Time.timeScale = 0.1f;
        }
        else Time.timeScale = 1;
    }
}
