using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerGrabObject : MonoBehaviour
{
    [Header("Controller Actions")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Vibration hapticAction;

    [Header("Else")]
    public GameObject contModel;
    private GameObject collidingObject;
    private GameObject objectInHand;
    public Piece pieceInHand;

    private void Update()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject && !objectInHand)
            {
                GrabObject();
            }
        }

        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        if (collidingObject.GetComponent<Outline>())
        {
            collidingObject.GetComponent<Outline>().enabled = false;
        }
        collidingObject = null;
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        if (col.GetComponent<Outline>())
        {
            col.GetComponent<Outline>().enabled = true;
        }
        collidingObject = col.gameObject;
    }

    private void GrabObject()
    {
        Piece piece = null;
        if (collidingObject.CompareTag("Piece"))
        {
            piece = collidingObject.GetComponent<Piece>();
            if (!piece.canMove) return;
            else if (GameManager.inst.isPlaying)
            {
                piece.isMoving = true;
                piece.canMove = false;
                GameManager.inst.chessBoard.RemovePieceFromBoard(piece);
                piece.isActive = false;
                GameManager.inst.chessBoard.ShowMoveArea(piece.boardIdx, piece.moveLimit, handType == SteamVR_Input_Sources.RightHand);
                StartCoroutine(piece.WhenGrabedCoroutine());
                pieceInHand = piece;
                StartCoroutine(DetectedHapticCoroutine());
            }
        }

        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    IEnumerator DetectedHapticCoroutine()
    {
        while (pieceInHand != null)
        {
            if (!pieceInHand.isFloorDetected)
            {
                hapticAction.Execute(0, 0.1f, 20, 10, handType);
            }
            yield return null;
        }
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

            if (objectInHand.CompareTag("Piece") && GameManager.inst.isPlaying)
            {
                var piece = objectInHand.GetComponent<Piece>();
                piece.isMoving = false;
                GameManager.inst.chessBoard.HideMoveArea(handType == SteamVR_Input_Sources.RightHand, piece);
            }
            if (objectInHand.GetComponent<Outline>())
            {
                objectInHand.GetComponent<Outline>().enabled = false;
            }
        }
        objectInHand = null;
        pieceInHand = null;
    }
}
