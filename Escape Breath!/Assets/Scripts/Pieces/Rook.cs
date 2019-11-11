using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    [Header("Rook Special")]
    [SerializeField]
    private GameObject protectEffect = null;

    public Vector3 originScale = new Vector3(0.06f, 0.03f, 0.07f);
    public Vector3 originPos = new Vector3(0, 0, 0.03f);
    public Vector3 longScale = new Vector3(0.06f, 0.06f, 0.07f);
    public Vector3 longPos = new Vector3(-0.03f, 0, 0.03f);

    protected override void SpecialReset()
    {
        protectEffect.SetActive(false);
        protectEffect.transform.localPosition = originPos;
        protectEffect.transform.localScale = originScale;
    }

    public override void BeforeAttack()
    {
        if (isActive && isAlive)
        {
            // guard this and backside
            isProtected = true;
            protectEffect.transform.localPosition = originPos;
            protectEffect.transform.localScale = originScale;
            protectEffect.SetActive(true);
            if (GameManager.inst.chessBoard.IsInBoardIdx(boardIdx.x, boardIdx.y - 1) && GameManager.inst.chessBoard.CheckPiece(boardIdx.x, boardIdx.y - 1))
            {
                Piece backside = GameManager.inst.chessBoard.GetPiece(new Vector2Int(boardIdx.x, boardIdx.y - 1));
                backside.isProtected = true;
                // make protecter long
                protectEffect.transform.localPosition = longPos;
                protectEffect.transform.localScale = longScale;
            }
        }
    }
}
