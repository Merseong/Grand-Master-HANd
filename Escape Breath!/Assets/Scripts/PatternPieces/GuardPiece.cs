using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPiece : MonoBehaviour
{
    public Vector3 targetPos;
    public float gravity = 3.9f;

    public Vector3 startPos;

    public void Throw(float time)
    {
        StartCoroutine(ThrowPiece(time));
    }
    IEnumerator ThrowPiece(float durationTime)
    {
        durationTime -= 0.1f;

        float distance = gravity * durationTime * durationTime / 2;

        transform.Translate(0, distance+0.15f, 0);
        startPos = transform.position;
        Debug.Log(startPos);
        float Velocity = 0; //속도 계산 

        float elapseTime = 0;

        //Debug.Log("real start throw");
        Destroy(gameObject, durationTime + 0.8f);
        while (elapseTime < durationTime)
        {
            transform.Translate(0, (Velocity - (gravity * elapseTime)) * Time.deltaTime, 0);
            elapseTime += (Time.deltaTime);
            //Debug.Log("helo" + transform.position);

            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
    }
}

