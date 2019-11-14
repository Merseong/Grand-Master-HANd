using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicPieceCore : MonoBehaviour
{
    private void Awake()
    {
        //transform.rotation = Quaternion.identity;

    }

    public void StartRotate(float time)
    {
        StartCoroutine(SonicRotate(time));
    }

    IEnumerator SonicRotate(float time)
    {
        Debug.Log("공굴러가유");
        float elapseTime = 0;
        while (elapseTime < time)
        {
            //transform.rotation = Quaternion.Euler(0,0,- 50 * elapseTime);
            elapseTime += Time.deltaTime;
            transform.Rotate(Vector3.forward, 50 * elapseTime);
        }
        yield return null;
    }

}
