using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform laser;
    [SerializeField]
    private Transform bullet;
    public Transform target;
    [Space(10)]
    public float flightTime;
    private float timer = 0;

    private Vector3 targetPos;
    private Vector3 targetOffset = new Vector3(0, 0.1f, 0);

    private void Update()
    {
        targetPos = target.position + targetOffset;
        laser.LookAt(targetPos);
        laser.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
        laser.localScale = new Vector3(laser.localScale.x, laser.localScale.y, Vector3.Distance(transform.position, targetPos) * 10);
        bullet.position = Vector3.Lerp(transform.position, targetPos, timer / flightTime);
        bullet.LookAt(targetPos);
        timer += Time.unscaledDeltaTime;
    }
}
