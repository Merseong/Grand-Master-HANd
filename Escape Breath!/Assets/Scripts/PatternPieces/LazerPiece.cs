using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerPiece : MonoBehaviour
{
    public Vector3 targetPos;
    public float gravity = 2.9f;
    public AudioSource dropSound;

    public Vector3 startPos;

    public void Throw(float time)
    {
        //Debug.Log("던져 던저");
        StartCoroutine(ThrowPiece(time));
        StartCoroutine(StartBGM(time));
    }
    IEnumerator ThrowPiece(float durationTime)
    {
        float distance = gravity * durationTime * durationTime / 2;

        transform.Translate(0, 0, - distance);
        startPos = transform.position;
        //Debug.Log(startPos);
        float Velocity = 0; //속도 계산 

        float elapseTime = 0;

        //Debug.Log("real start throw");
        Destroy(gameObject, durationTime + 1f);
        while (elapseTime < durationTime)
        {
            transform.Translate(0, 0, - (Velocity -(gravity * elapseTime)) * Time.deltaTime);
            elapseTime += (Time.deltaTime );
            //Debug.Log("helo" + transform.position);

            yield return null;
        }
    }
    IEnumerator StartBGM(float durationTime)
    {
        yield return new WaitForSeconds(durationTime- 0.35f);
        dropSound.Play();
    }
}
