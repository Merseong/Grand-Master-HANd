using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicPiece : MonoBehaviour
{
    public Vector3 targetPos;
    public float acceleration = 0.8f;
    public Vector3 startPos;

    public void Awake()
    {
        transform.Translate(0, 0.11f, 0);
        startPos = transform.position;
        //Debug.Log("sonic ONLINE");
    }

    public void Fire(float time)
    {
        //Debug.Log("데굴 데굴");
        //gameObject.GetComponentInChildren<SonicPieceCore>().StartRotate(time);
        StartCoroutine(ThrowPiece(time));
    }

    IEnumerator ThrowPiece(float durationTime)
    {
        yield return new WaitForSeconds(1.5f);
        float distance = acceleration * durationTime * durationTime / 2;
        
        //startPos = transform.position;
        
        float Velocity = 0; //속도 계산 

        float elapseTime = 0;

        //Debug.Log("real start throw");
        Destroy(gameObject, durationTime - 0.2f);
        while (elapseTime < durationTime)
        {
            transform.Translate(0, 0,(Velocity - (acceleration * elapseTime)) * Time.deltaTime, 0);
            elapseTime += (Time.deltaTime);
            //Debug.Log("helo" + transform.position);

            yield return null;
        }
    }
}
