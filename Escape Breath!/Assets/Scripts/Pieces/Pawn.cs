using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override void PieceDestroy()
    {
        Destroy(this);
    }
}
