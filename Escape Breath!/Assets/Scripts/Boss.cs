﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    public int health;
    public int phase;
    public List<GameObject> phasePatterns = new List<GameObject>();
    public int[] phasePatternsLimit = new int[1];
    public Transform patternStarter;

    public int attackType;
    public Transform bossTarget;
    public TextMesh testTextMesh;
    public Object attackArea;

    public MeteorP meteorPS;
    public Transform attackPoint;

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

    private BossPattern SelectPattern()
    {
        // select random pattern or biased pattern
        int idx = Random.Range(0, phasePatternsLimit[phase] + 1);
        return Instantiate(phasePatterns[idx], patternStarter).GetComponent<BossPattern>();
    }

    public void StartPattern()
    {
        var pattern = SelectPattern();
        pattern.StartPattern();
    }

    // 이 아랫부분은 남겨둘테니 알아서 옮기셈

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
                        Debug.Log("attackType:" + attackType);
                    }
                    else if (ran == 1)
                    {
                        lazer(1);
                        attackType = 11;
                        Debug.Log("attackType:" + attackType);
                    }
                    else if (ran == 2)
                    {
                        attackType = 12;
                        Debug.Log("attackType:" + attackType);
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
                var obj = Instantiate(meteorPS.gameObject, attackPoint);
                obj.GetComponent<MeteorP>().Throw(Board.IndexToGlobalPos(x, y)); //how to transform from vec3? 
                //StartCoroutine(throwAttack(obj, Board.IndexToGlobalPos(x, y)));
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
        //랜덤으로 공격할 말 선택 -> 있는지 확인 -> 위치 가져옴 => 일자 공격
        //페이즈에 따른 변화 : 공격 말 개수 & 공격 해당 말
    }
}
