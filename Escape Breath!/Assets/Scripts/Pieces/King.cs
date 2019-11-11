using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    [Header("King Special")]
    public int buffRange = 2;
    public int buffAmount = 2;

    [SerializeField]
    private GameObject buffCube;

    public Vector3 originScale;
    public Vector3 largedScale;

    public override void PieceDestroy()
    {
        // when king destroyed, Game Over Action
        base.PieceDestroy();
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
                    if (GameManager.inst.chessBoard.IsInBoardIdx(boardIdx.x + i, boardIdx.y + j) && GameManager.inst.chessBoard.CheckPiece(boardIdx.x + i, boardIdx.y + j))
                    {
                        var buffed = GameManager.inst.chessBoard.GetPiece(new Vector2Int(boardIdx.x + i, boardIdx.y + j));
                        if (buffed != null) buffed.damage += buffAmount;
                        // buff effect
                        StartCoroutine(KingBuffCoroutine());
                    }
                }
            }
        }
    }

    IEnumerator KingBuffCoroutine()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            buffCube.transform.localScale = Vector3.Lerp(originScale, largedScale, timer * 2);
            yield return null;
        }

        buffCube.transform.localScale = originScale;
    }
}
