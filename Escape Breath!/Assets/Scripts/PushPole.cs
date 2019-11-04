using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPole : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Piece"))
        {
            var piece = other.GetComponent<Piece>();
            if (!piece.isMoving)
            {
                GameManager.inst.chessBoard.MovePieceWithDir(piece.boardIdx, new Vector2(0, 1));
            }
        }
    }
}
