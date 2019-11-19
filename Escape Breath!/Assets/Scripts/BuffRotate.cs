using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRotate : MonoBehaviour
{
    public int rotateSpeed = 1;
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }
}
