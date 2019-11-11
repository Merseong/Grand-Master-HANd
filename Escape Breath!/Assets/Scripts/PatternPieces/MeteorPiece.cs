using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPiece : MonoBehaviour
{
    public Vector3 TargetPos;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    
    public Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }
    public void Throw(Vector3 targetPosition, float time)
    {
        //Debug.Log("throw!");
        TargetPos = targetPosition;
        StartCoroutine(ThrowMeteor(time));
    }


    IEnumerator ThrowMeteor(float durationTime)
    {
        transform.position = startPos;

        float distance2Target = Vector3.Distance(transform.position, TargetPos); //거리 계산
         //Debug.Log("distance calculating");

        float Velocity = distance2Target / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity); //속도 계산 
        float Vx = Mathf.Sqrt(Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        //Debug.Log("velocity calculating");

        float flightDuration = distance2Target / Vx; //체공 시간
        float timeScale = flightDuration / durationTime;

        transform.rotation = Quaternion.LookRotation(TargetPos - transform.position); //폰 바라보게

        float elapseTime = 0;

        //Debug.Log("real start throw");
        Destroy(gameObject, durationTime - 0.2f);
        while (elapseTime < durationTime)
        {
            transform.Translate(0, (Vy - (gravity * elapseTime)) * Time.deltaTime * timeScale, Vx * Time.deltaTime * timeScale);
            elapseTime += (Time.deltaTime * timeScale);
            //Debug.Log("helo" + transform.position);

            yield return null;
        }
    }
}
