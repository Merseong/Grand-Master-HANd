using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override void PieceDestroy()
    {
        throw new System.NotImplementedException();
    }

    public override void BeforeAttack()
    {
        if (isActive)
        {
            // guard this and backside
            isProtected = true;
            // instantiate protecter gameObj
            Piece backside; 
            if (backside = GameManager.inst.chessBoard.GetPiece(boardIdx.x, boardIdx.y - 1))
            {
                backside.isProtected = true;
                // make protecter long
            }
        }
    }
}
