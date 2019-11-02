using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override void PieceDestroy()
    {
        // remove piece from board
        GameManager.inst.chessBoard.allAttack -= AutoAttack;
        GameManager.inst.chessBoard.allReset -= ResetAfterTurnEnd;
        Destroy(this);
    }
}
