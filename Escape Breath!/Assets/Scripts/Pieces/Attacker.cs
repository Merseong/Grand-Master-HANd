﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Piece
{
    public override void PieceDestroy()
    {
        GetComponent<MeshRenderer>().material = GameManager.inst.blackMat;
        isAlive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) PieceDestroy();
    }
}