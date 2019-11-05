using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerResurrection : MonoBehaviour
{
    public ControllerGrabObject contGrab;

    [Header("Controller Actions")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Vector2 touchPositionAction;

    [Header("Else")]
    public ParticleSystem particle;

    private float rechargeEnd = 50f;
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
                nextPoint += Vector2.Distance(beforeTouchPosition, touchPositionAction.axis);
                nextPoint += Vector3.Distance(beforeControllerPosition, controllerPose.transform.position);
                if (nextPoint > 0)
                {
                    p.rechargePoint += nextPoint;
                    p.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.Lerp(Color.black, Color.white, p.rechargePoint / rechargeEnd));
                }

                if (!particle.isPlaying) particle.Play();
                if (p.rechargePoint > rechargeEnd)
                {
                    p.Resurrection();
                }
            }
            else
            {
                particle.Stop();
            }
            beforeTouchPosition = touchPositionAction.axis;
            beforeControllerPosition = controllerPose.transform.position;
        }
        else
        {
            particle.Stop();
        }
    }
}
