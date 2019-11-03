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
            if (GameManager.inst.chessBoard.IsInBoardIdx(boardIdx.x, boardIdx.y - 1))
            {
                Piece backside = GameManager.inst.chessBoard.GetPiece(boardIdx.x, boardIdx.y - 1);
                if (backside != null) backside.isProtected = true;
                // make protecter long
            }
        }
    }
}
