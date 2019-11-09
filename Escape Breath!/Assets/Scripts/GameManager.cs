using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject attackObj;
    public Material whiteMat;
    public Material blackMat;
    public Light spotLight;

    public void GameOver()
    {
        Debug.LogError("Game Over!");
        turnSystem.isGameEnd = true;
        turnSystem.timeText.text = "Game Over!";
        Time.timeScale = 1;
        for (int i = 0; i < chessBoard.pieceList.Count; ++i)
        {
            chessBoard.pieceList[i].rb.AddExplosionForce(1000f, Vector3.zero, 1000f);
        }
    }

    public void GameClear()
    {
        Debug.LogError("Game Clear!");
        turnSystem.isGameEnd = true;
        turnSystem.timeText.text = "Game Clear!";
        Time.timeScale = 1;
        boss.rb.isKinematic = false;
        boss.rb.AddExplosionForce(10f, Vector3.zero, 100f);
        StartCoroutine(LightWiderCoroutine());
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
