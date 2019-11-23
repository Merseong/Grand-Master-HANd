using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMoveSceneObject : MonoBehaviour
{
    public GameObject awakeUI;
    private SceneAwakeUI awakeUIinst;

    public void StartMoveScene()
    {
        awakeUIinst = Instantiate(awakeUI).GetComponent<SceneAwakeUI>();
        awakeUIinst.GetComponent<Canvas>().worldCamera = Camera.main;
        awakeUIinst.isFadeIn = false;
        StartCoroutine(awakeUIinst.BackgroundAlphaCoroutine());
        StartCoroutine(ChangeSceneAsync());
    }

    IEnumerator ChangeSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone && !awakeUIinst.isDone)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }
}
