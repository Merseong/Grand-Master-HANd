using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;
    public Transform bossTarget;
    public TextMesh testTextMesh;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            var obj = other.GetComponent<AttackObj>();
            health = Mathf.Max(0, health - obj.damage);
            if (health > 0) testTextMesh.text = health.ToString();
            else GameEnd(true);

            obj.rb.isKinematic = true;
            obj.rb.velocity = Vector3.zero;
            obj.transform.position = transform.position;
        }
    }

    private void GameEnd(bool isClear)
    {
        if (isClear)
        {
            testTextMesh.text = "클리어!";
        }
        else
        {

        }
    }
}
