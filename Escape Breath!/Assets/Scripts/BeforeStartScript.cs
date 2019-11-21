using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeStartScript : MonoBehaviour
{
    public Transform kingTr;
    public Transform bossTr;

    public EnvRotate[] envs;

    private bool isStarting = false;
    private Vector3 bossStartPos;
    private Vector3 bossFinalPos;

    private void Start()
    {
        bossFinalPos = bossTr.position;
        bossStartPos = bossFinalPos + new Vector3(0, 10, 0);
        bossTr.position = bossStartPos;
        bossTr.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == kingTr && !isStarting)
        {
            isStarting = true;
            for (int i = 0; i < envs.Length; ++i)
            {
                envs[i].isRotating = false;
            }
            StartCoroutine(LoadSceneAsyncCoroutine());
        }
    }

    IEnumerator LoadSceneAsyncCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;
        bossTr.gameObject.SetActive(true);

        float timer = 0;
        while (!asyncLoad.isDone && timer < 1f)
        {
            timer += Time.deltaTime;
            bossTr.position = Vector3.Lerp(bossStartPos, bossFinalPos, timer);
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }
}
