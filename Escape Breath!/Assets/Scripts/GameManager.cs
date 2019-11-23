using System.Collections;
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

    public GameObject moveSceneObj;
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
        chessBoard.allReset();
        chessBoard.HideMoveArea(true);
        chessBoard.HideMoveArea(false);
        chessBoard.HideDangerArea();

        hmdUI.turnText.text = "Game Over!";
        StartCoroutine(PieceFlyingCoroutine());
        StartCoroutine(ShowMoveSceneObject(Color.red));
    }

    public void GameClear()
    {
        Debug.LogError("Game Clear!");
        turnSystem.isGameEnd = true;
        isPlaying = false;
        chessBoard.allReset();
        chessBoard.HideMoveArea(true);
        chessBoard.HideMoveArea(false);
        chessBoard.HideDangerArea();

        hmdUI.turnText.text = "Game Clear!";
        Time.timeScale = 1;
        boss.rb.isKinematic = false;
        boss.rb.AddExplosionForce(5f, Vector3.zero, 500f, 1, ForceMode.Impulse);
        StartCoroutine(LightWiderCoroutine());
        StartCoroutine(ShowMoveSceneObject(Color.green));
    }

    IEnumerator PieceFlyingCoroutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 0.2f;
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
    
    IEnumerator ShowMoveSceneObject(Color color)
    {
        yield return new WaitForSecondsRealtime(3f);
        var obj = Instantiate(moveSceneObj).GetComponent<MeshRenderer>();
        obj.material.color = color;
    }
}
