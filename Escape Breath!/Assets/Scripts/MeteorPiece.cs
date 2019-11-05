using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPiece : MonoBehaviour
{
    public Vector3 Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    
    private Transform myTransform;
    public Vector3 startPos;

    void Awake()
    {
        myTransform = transform;
        startPos = transform.position;
    }
    public void Throw(Vector3 targetPosition, float time)
    {
        Debug.Log("throw!");
        Target = targetPosition;
        StartCoroutine(ThrowMeteor(time));
    }


    IEnumerator ThrowMeteor(float time) //주어진건 출발 위치, 도착 위치 = 거리, 턴 타이머 = 체공 시간 => 속력에 변화를 줘야함. 
    {
        transform.position = startPos + new Vector3(0, 0.0f, 0);

        float target_Distance = Vector3.Distance(transform.position, Target); //거리 계산
        Debug.Log("distance calculating");

        float transform_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity); //속도 계산 
        float Vx = Mathf.Sqrt(transform_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(transform_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        Debug.Log("velocity calculating");

        float flightDuration = target_Distance / Vx; //체공 시간

        float timeScale = flightDuration / time;
        transform.rotation = Quaternion.LookRotation(Target - transform.position); //빙글빙글

        float elapse_time = 0;

        Debug.Log("real start throw");
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time))  * Time.deltaTime, Vx  * Time.deltaTime);
            elapse_time += Time.deltaTime;
            Debug.Log("helo" + transform.position);

            yield return null;
        }
    }
}
