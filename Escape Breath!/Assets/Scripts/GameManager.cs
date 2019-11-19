﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (inst != null) Destroy(inst);
        inst = this;
    }

    public Boss boss;
    public ChessBoard chessBoard;
    public TurnSystem turnSystem;
    public BossBackUI bossBackUI;
    public HMDUI hmdUI;

    public SteamVR_Behaviour_Pose leftController;
    public SteamVR_Behaviour_Pose rightController;

    public GameObject attackObj;
    public Material whiteMat;
    public Material blackMat;
    public Light spotLight;

    public bool isPlaying = false;

    public void StartGame()
    {
        turnSystem.isGameEnd = false;
        isPlaying = true;
    }

    private void Update()
    {
        if (!isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void GameOver()
    {
        Debug.LogError("Game Over!");
        turnSystem.isGameEnd = true;
        isPlaying = false;
        hmdUI.turnText.text = "Game Over!";
        StartCoroutine(PieceFlyingCoroutine());
    }

    public void GameClear()
    {
        Debug.LogError("Game Clear!");
        turnSystem.isGameEnd = true;
        isPlaying = false;
        hmdUI.turnText.text = "Game Clear!";
        Time.timeScale = 1;
        boss.rb.isKinematic = false;
        boss.rb.AddExplosionForce(5f, Vector3.zero, 500f, 1, ForceMode.Impulse);
        StartCoroutine(LightWiderCoroutine());
    }

    IEnumerator PieceFlyingCoroutine()
    {
        yield return new WaitForFixedUpdate();
        Time.timeScale = 0.5f;
        chessBoard.allReset();
        for (int i = 0; i < chessBoard.pieceList.Count; ++i)
        {
            chessBoard.pieceList[i].rb.AddExplosionForce(5f, Vector3.zero, 500f, 1, ForceMode.Impulse);
        }
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
    }

    IEnumerator LightWiderCoroutine()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            spotLight.spotAngle = 30 + 150 * timer;
            spotLight.range = 10 + 14 * timer;
            yield return null;
        }
        spotLight.spotAngle = 180;
        spotLight.range = 24;
    }
}
