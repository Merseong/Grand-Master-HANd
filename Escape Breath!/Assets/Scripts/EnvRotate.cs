using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvRotate : MonoBehaviour
{
    private Quaternion original;
    private float speed;
    private Vector3 rotAngle;
    public bool isRotating = true;

    private void Start()
    {
        original = transform.rotation;
        speed = Random.Range(1f, 10f);
        rotAngle = new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
        StartCoroutine(ObjRotate());
    }

    IEnumerator ObjRotate()
    {
        while (isRotating)
        {
            transform.Rotate(rotAngle * speed * Time.deltaTime);
            yield return null;
        }
        for (float time = 0; time < 2f; time += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, original, time * 0.5f);
            yield return null;
        }
    }
}
