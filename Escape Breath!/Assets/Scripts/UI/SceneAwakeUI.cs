using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneAwakeUI : MonoBehaviour
{
    public Image background;
    public Image icon;
    public bool startOnAwake = false;
    public bool isFadeIn = true;
    public bool isDone = false;

    private void Start()
    {
        if (startOnAwake) StartCoroutine(BackgroundAlphaCoroutine());
    }

    public IEnumerator BackgroundAlphaCoroutine()
    {
        float time;
        if (isFadeIn)
        {
            time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                icon.rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -360, time));
                yield return null;
            }
        }
        time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            if (isFadeIn)
            {
                background.color = Color.Lerp(Color.black, Color.clear, time);
                icon.color = Color.Lerp(Color.white, Color.clear, time);
            }
            else
            {
                background.color = Color.Lerp(Color.clear, Color.black, time);
                icon.color = Color.Lerp(Color.clear, Color.white, time);
            }
            yield return null;
        }
        if (!isFadeIn)
        {
            time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                icon.rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -360, time));
                yield return null;
            }
        }
        isDone = true;
        Destroy(gameObject, 3f); 
    }
}
