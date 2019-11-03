using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public int buffRange = 2;
    public int buffAmount = 2;

    public override void PieceDestroy()
    {
        // when king destroyed, Game Over Action
        GameManager.inst.GameOver();
    }

    public override void BeforeAttack()
    {
        if (isActive)
        {
            // buff to near piece
            for (int i = -buffRange; i <= buffRange; ++i)
            {
                for (int j = -buffRange; j <= buffRange; ++j)
                {
                    if (GameManager.inst.chessBoard.IsInBoardIdx(boardIdx.x + i, boardIdx.y + j))
                    {
                        var buffed = GameManager.inst.chessBoard.GetPiece(boardIdx.x + i, boardIdx.y + j);
                        if (buffed != null) buffed.damage += buffAmount;
                        // buff effect
                    }
                }
            }
        }
    }
}
