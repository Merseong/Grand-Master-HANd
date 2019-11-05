using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPiece : MonoBehaviour
{
    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public GameObject metoerP;
    public Transform Projectile;
    private Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }
    public void Throw(Vector3 targetPosition, float time)
    {
        Debug.Log("throw!");
        Target.position = targetPosition;
        StartCoroutine(ThrowMeteor(time));
    }


    IEnumerator ThrowMeteor(float time) //주어진건 출발 위치, 도착 위치 = 거리, 턴 타이머 = 체공 시간 => 속력에 변화를 줘야함. 
    {
        yield return new WaitForSeconds(2.5f); //확인 해야대 없애 말아

        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        float target_Distance = Vector3.Distance(Projectile.position, Target.position); //거리 계산
        Debug.Log("distance calculating");

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity); //속도 계산 
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        Debug.Log("velocity calculating");

        float flightDuration = target_Distance / Vx; //체공 시간

        float timeScale = flightDuration / time;
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position); //빙글빙글

        float elapse_time = 0;

        Debug.Log("real start throw");
        while (elapse_time < time)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * timeScale * Time.deltaTime, Vx * timeScale * Time.deltaTime);
            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
