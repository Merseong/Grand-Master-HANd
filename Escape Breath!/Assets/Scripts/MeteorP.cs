using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorP : MonoBehaviour
{
    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    private Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }

    public void Throw(Vector3 targetPosition)
    {
        Target.position = targetPosition;
        StartCoroutine(ThrowMeteor());
    }


    IEnumerator ThrowMeteor()
    {
        yield return new WaitForSeconds(1.5f);

        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        float target_Distance = Vector3.Distance(Projectile.position, Target.position); //거리 계산

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity); //속도 계산 
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = target_Distance / Vx; //체공 시간
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position); //빙글빙글

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
