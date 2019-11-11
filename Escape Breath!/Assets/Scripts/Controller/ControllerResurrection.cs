using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerResurrection : MonoBehaviour
{
    public ControllerGrabObject contGrab;

    [Header("Controller Actions")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Vector2 touchPositionAction;
    public SteamVR_Action_Vibration hapticAction;

    [Header("Else")]
    public ParticleSystem particle;

    private float rechargeEnd = 30f;
    private Vector2 beforeTouchPosition = new Vector2();
    private Vector3 beforeControllerPosition;

    private void Start()
    {
        beforeControllerPosition = transform.position;
    }

    private void Update()
    {
        if (contGrab.pieceInHand != null)
        {
            if (!contGrab.pieceInHand.isAlive)
            {
                Piece p = contGrab.pieceInHand;
                float nextPoint = 0;
                nextPoint += Vector2.Distance(beforeTouchPosition, touchPositionAction.GetAxis(handType));
                //Debug.Log(nextPoint);
                //nextPoint += Vector3.Distance(beforeControllerPosition, controllerPose.transform.position);
                //Debug.LogError(nextPoint);
                if (nextPoint > 0)
                {
                    p.rechargePoint += nextPoint;
                    hapticAction.Execute(0, 0.1f, nextPoint, nextPoint, handType);
                    p.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.Lerp(Color.black, Color.white, p.rechargePoint / rechargeEnd));
                }

                if (!particle.isPlaying) particle.Play();
                //Debug.Log(p.rechargePoint);
                if (p.rechargePoint > rechargeEnd)
                {
                    p.Resurrection();
                }
            }
            else
            {
                particle.Stop();
            }
            beforeTouchPosition = touchPositionAction.GetAxis(handType);
            beforeControllerPosition = controllerPose.transform.position;
        }
        else
        {
            particle.Stop();
        }
    }
}
