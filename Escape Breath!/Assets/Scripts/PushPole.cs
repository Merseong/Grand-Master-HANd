using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPole : MonoBehaviour
{
    public GameObject parent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piece"))
        {
            var piece = other.GetComponent<Piece>();
            if (!piece.isMoving && GameManager.inst.turnSystem.currentTurn == TurnType.MovePiece)
            {
                Vector2 dir = new Vector2(transform.forward.x, transform.forward.z);
                //Debug.Log(dir);
                GameManager.inst.chessBoard.MovePieceWithDir(piece.boardIdx, dir);
                Destroy(parent, 0.3f);
            }
        }
    }
}
