using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicPieceCore : MonoBehaviour
{
    float time = 0;
    private void Awake()
    {
        //transform.rotation = Quaternion.identity;

    }

    public void Update()
    {
        time += Time.deltaTime;
        transform.Rotate(0, 0, -10);
    }
}
