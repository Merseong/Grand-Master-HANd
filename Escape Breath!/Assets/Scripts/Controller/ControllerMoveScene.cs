using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMoveScene : MonoBehaviour
{
    [SerializeField]
    private bool isChanging = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isChanging && other.GetComponent<ControllerMoveSceneObject>())
        {
            Debug.Log("Start Scene Move");
            isChanging = true;
            other.GetComponent<ControllerMoveSceneObject>().StartMoveScene();
        }
    }
}
