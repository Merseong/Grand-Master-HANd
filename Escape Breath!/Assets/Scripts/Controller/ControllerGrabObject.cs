using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrabObject : MonoBehaviour
{
    [Header("Controller Actions")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

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
        collidingObject = null;
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
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
            else
            {
                piece.isMoving = true;
                //piece.canMove = false;
                GameManager.inst.chessBoard.RemovePieceFromBoard(piece);
                //piece.isActive = false;
                //GameManager.inst.chessBoard.ShowMoveArea(piece.boardIdx, piece.moveLimit, handType == SteamVR_Input_Sources.RightHand);
                //StartCoroutine(piece.WhenGrabedCoroutine());
                pieceInHand = piece;
            }
        }

        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
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

            if (objectInHand.CompareTag("Piece"))
            {
                var piece = objectInHand.GetComponent<Piece>();
                piece.isMoving = false;
                //GameManager.inst.chessBoard.HideMoveArea(handType == SteamVR_Input_Sources.RightHand, piece);
            }
        }
        objectInHand = null;
        pieceInHand = null;
    }
}
