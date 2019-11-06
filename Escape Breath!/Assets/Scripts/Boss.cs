﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    public int health;
    public int phase;
    public List<GameObject> phasePatterns = new List<GameObject>(); //list가 아니라 그냥 겜오브젝트 하나면 되지않냥
    //public int[] phasePatternsLimit = new int[1]; //날려야해
    public Transform patternStarter;

    public int attackType;
    public Transform bossTarget;
    public TextMesh testTextMesh;
    public Object attackArea;
    
    public Transform attackPoint;

    bool isClose = false;

    //test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("test Space");
            var A = Instantiate(phasePatterns[0], patternStarter).GetComponent<BossPattern>();
            A.StartPattern();
        }
    }
    //test
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            var obj = other.GetComponent<AttackObj>();
            Damaged(obj.damage);

            obj.rb.isKinematic = true;
            obj.rb.velocity = Vector3.zero;
            obj.transform.position = transform.position;
        }
        else if (other.CompareTag("Piece"))
        {
            var obj = other.GetComponent<Piece>();
            Damaged(obj.damage);

            GameManager.inst.chessBoard.PermanantRemovePiece(obj);
        }
    }

    private void Damaged(int damage)
    {
        health = Mathf.Max(0, health - damage * 3);
        if (health > 0) testTextMesh.text = health.ToString();
        else GameEnd(true);
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

        if (isClose)
            return Instantiate(phasePatterns[0], patternStarter).GetComponent<BossPattern>(); //근접 공격 phasepatterns 임시 숫자임
        else
        {
            int idx = Random.Range(0, phasePatterns.Count);
            return Instantiate(phasePatterns[idx], patternStarter).GetComponent<BossPattern>();
        }
    }

    public void StartPattern()
    {
        var pattern = SelectPattern();
        pattern.StartPattern();
    }
    
    public void CheckClose()
    {
        for (int b = 5; b < 8; b++)
        {
            for (int a = 0; a < 8; a++)
            {
                if (GameManager.inst.chessBoard.GetPiece(a, b) != null)
                    isClose = true;
            }
        }
    }

    public void AttackOnBoard(Vector2Int pos, float duration, bool isStrong = false)
    {
        StartCoroutine(AttackPiece(pos, duration, isStrong));
        GameManager.inst.chessBoard.ShowAttackArea(pos, duration - 0.1f, isStrong);
    }

    IEnumerator AttackPiece(Vector2Int pos, float time, bool isStrong)
    {
        //시간 지나야 연산 들어가는거 구현 필요

        /*float Ntime = 0;

        while(Ntime < time)
        {
            Ntime += Time.deltaTime;
            yield return null;
        }*/
        if (GameManager.inst.chessBoard.GetPiece(pos.x, pos.y) != null)
        {
            GameManager.inst.chessBoard.GetPiece(pos.x, pos.y).Damaged(); //강공격 약공격도 처리 해줘야 함
        }
        yield return null;
    }

    //갈갈
    //타겟 정하기, 데미지 입히기
    /*public void readyAttack(int phase) //공격 준비 단계
    {
        int ran = Random.Range(0, 2);
        bool isClose = false;
        for (int b = 5; b < 8; b++)
        {
            for(int a = 0; a < 8; a++)
            {
                if (!GameManager.inst.chessBoard.CheckPiece(a, b))
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
    public void lazer(int phase)
    {
        //랜덤으로 공격할 말 선택 -> 있는지 확인 -> 위치 가져옴 => 일자 공격
        //페이즈에 따른 변화 : 공격 말 개수 & 공격 해당 말
    }*/
}
