using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    public int health;
    public int phase;
    public int attackType;
    public Transform bossTarget;
    public TextMesh testTextMesh;
    public Object attackArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            var obj = other.GetComponent<AttackObj>();
            health = Mathf.Max(0, health - obj.damage);
            if (health > 0) testTextMesh.text = health.ToString();
            else GameEnd(true);

            obj.rb.isKinematic = true;
            obj.rb.velocity = Vector3.zero;
            obj.transform.position = transform.position;
        }
    }

    private void GameEnd(bool isClear)
    {
        if (isClear)
        {
            testTextMesh.text = "클리어!";
        }
        else
        {

        }
    }
    public void readyAttack(int phase) //공격 준비 단계
    {
        int ran = Random.Range(0, 2);
        bool isClose = false;
        for (int b = 5; b < 8; b++)
        {
            for(int a = 0; a < 8; a++)
            {
                if (GameManager.inst.chessBoard.GetPiece(a, b) == null)
                    isClose = true;
            }
        }
        if (isClose == true)
        {

        }
        else
        {
            switch (phase)
            {
                case 1:
                    if (ran == 0)
                    {
                        meteor(1);
                        attackType = 10;
                    }
                    else if (ran == 1)
                    {
                        lazer(1);
                        attackType = 11;
                    }
                    else if (ran == 2)
                    {
                        attackType = 12;
                    }
                    break;
            }
        }
    }
    public void attack(int attackType) //ㄹㅇ 공격
    {
        switch (attackType){
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
        }
    }

    public void meteor(int phase)
    {
        int x, y;
        var Board = GameManager.inst.chessBoard;

        if(phase == 0)
        {
            for(int i =0; i <4; i++)
            {
                x = (int)Random.Range(1, 8);
                y = (int)Random.Range(1, 4);
                Instantiate(attackArea, Board.IndexToGlobalPos(x, y), Quaternion.identity); //제대로 생성되는지 확인 요망
            }
        }else
        {
            for (int i = 0; i < 8; i++)
            {
                x = (int)Random.Range(1, 8);
                y = (int)Random.Range(1, 4);
                Instantiate(attackArea, Board.IndexToGlobalPos(x, y), Quaternion.identity); //제대로 생성되는지 확인 요망
            }
        }
    }
    public void lazer(int phase)
    {

    }
    public 
}
