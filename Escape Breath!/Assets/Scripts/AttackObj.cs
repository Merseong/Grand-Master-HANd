using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObj : MonoBehaviour
{
    public int damage = 0;
    public TrailRenderer trail;
    public Rigidbody rb;

    private float height = 2;
    private float time = 7;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public void Init()
    {
        //Material material = new Material(trail.material);
        Color color = Color.Lerp(Color.white, Color.yellow, damage / 10f);
        trail.material.SetColor("_BaseColor", color);
        trail.material.SetColor("_EmissionColor", color);
        //trail.material = material;
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);

        Vector3 direction = GameManager.inst.boss.bossTarget.position - transform.position;
        float distance = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * distance / time * 10;
        direction.y = 2 * height / time + 2.5f;
        rb.AddForce(direction, ForceMode.VelocityChange);

        Destroy(gameObject, 3);
    }
}
