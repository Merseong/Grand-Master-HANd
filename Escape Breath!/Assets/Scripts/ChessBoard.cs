using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    private int maxBoardIndex = 7;
    private Vector2 zeroOffset = new Vector2(-0.21f, -0.21f);
    private float indexOffset = 0.6f;
    private Piece[,] piecesGrid = new Piece[8, 8]; // [right, up] from left-down

    public Vector2 IndexToPos(int right, int up)
    {
        if (0 > right || right > maxBoardIndex || 0 > up || up > maxBoardIndex)
        {
            right = Mathf.Clamp(right, 0, maxBoardIndex);
            up = Mathf.Clamp(up, 0, maxBoardIndex);
            Debug.LogWarning("[WARN-IndexToPos] bound exceeded, clamped");
        }
        return new Vector2(zeroOffset.x + right * indexOffset, zeroOffset.y + up * indexOffset);
    }

    public Vector2Int PosToNearIndex(float x, float y)
    {
        int i = Mathf.Clamp(Mathf.RoundToInt(x / indexOffset - zeroOffset.x), 0, maxBoardIndex);
        int j = Mathf.Clamp(Mathf.RoundToInt(y / indexOffset - zeroOffset.y), 0, maxBoardIndex);
        return new Vector2Int(i, j);
    }

    public Piece GetPiece(int right, int up)
    {
        if (0 > right || right > maxBoardIndex || 0 > up || up > maxBoardIndex)
        {
            Debug.LogError("[ERR-GetPiece] bound exceeded");
            return null;
        }
        else return piecesGrid[right, up];
    }

    public void MovePiece(int fromRight, int fromUp, int toRight, int toUp)
    {
        if (0 > fromRight || fromRight > maxBoardIndex || 0 > fromUp || fromUp > maxBoardIndex)
        {
            Debug.LogError("[ERR-MovePiece] from bound exceeded");
            return;
        }
        else if (0 > toRight || toRight > maxBoardIndex || 0 > toUp || toUp > maxBoardIndex)
        {
            Debug.LogError("[ERR-MovePiece] to bound exceeded");
            return;
        }
        else
        {
            piecesGrid[toRight, toUp] = piecesGrid[fromRight, fromUp];
            piecesGrid[fromRight, fromUp] = null;
        }
    }

    public void ShowAttackArea(int right, int up, float duration, bool isStrong = false)
    {
        // show attack mark on [right, up] for duration
    }

    private void Start()
    {
        // init all pieces
    }
}
