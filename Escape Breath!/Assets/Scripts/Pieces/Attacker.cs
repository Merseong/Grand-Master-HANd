using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Piece
{
    public override void PieceDestroy()
    {
        // remove piece from board
        isAlive = false;
    }
}
