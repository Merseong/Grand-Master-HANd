using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    private int maxBoardIndex = 7;
    private Vector2 zeroOffset = new Vector2(-0.21f, -0.21f);
    private float indexOffset = 0.06f;
    private Piece[,] piecesGrid = new Piece[8, 8]
        {
            {null, null,null,null,null,null,null,null},
            {null, null,null,null,null,null,null,null},
            {null, null,null,null,null,null,null,null},
            {null, null,null,null,null,null,null,null},
            {null, null,null,null,null,null,null,null},
            {null, null,null,null,null,null,null,null},
            {null, null,null,null,null,null,null,null},
            {null, null,null,null,null,null,null,null}
        }; // [right, up] from left-down
    public List<Piece> pieceList = new List<Piece>();

    public Vector2 IndexToLocalPos(int right, int up)
    {
        if (0 > right || right > maxBoardIndex || 0 > up || up > maxBoardIndex)
        {
            right = Mathf.Clamp(right, 0, maxBoardIndex);
            up = Mathf.Clamp(up, 0, maxBoardIndex);
            Debug.LogWarning("[WARN-IndexToPos] bound exceeded, clamped");
        }
        return new Vector2(zeroOffset.y + up * indexOffset, zeroOffset.x + right * indexOffset);
    }

    public Vector3 IndexToGlobalPos(int right, int up)
    {
        var lPos = 3 * IndexToLocalPos(right, up);
        return new Vector3(lPos.y, 0, lPos.x) + transform.position;
    }

    // offset: 0.06f -> 0.18f, area -0.63 ~ 0.63
    public Vector2Int PosToNearIndex(float x, float y)
    {
        int i = Mathf.Clamp(Mathf.RoundToInt((x - zeroOffset.x * 3) / (indexOffset * 3)), 0, maxBoardIndex);
        int j = Mathf.Clamp(Mathf.RoundToInt((y - zeroOffset.y * 3) / (indexOffset * 3)), 0, maxBoardIndex);
        return new Vector2Int(i, j);
    }

    public bool AddPiece(Piece p)
    {
        var idx = PosToNearIndex(p.transform.position.x, p.transform.position.z);
        if (GetPiece(idx.x, idx.y) != null)
        {
            Debug.LogError("[ERR-AddPiece] already piece on " + idx);
            return false;
        }
        else
        {
            piecesGrid[idx.x, idx.y] = p;
            p.boardIdx = idx;
            pieceList.Add(p);
        }
        return true;
    }

    public Piece GetPiece(int right, int up)
    {
        if (0 > right || right > maxBoardIndex || 0 > up || up > maxBoardIndex)
        {
            Debug.LogError("[ERR-GetPiece] bound exceeded: " + right + ", " + up);
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
        if (0 > toRight || toRight > maxBoardIndex || 0 > toUp || toUp > maxBoardIndex)
        {
            Debug.LogError("[ERR-MovePiece] to bound exceeded");
            return;
        }
        piecesGrid[toRight, toUp] = piecesGrid[fromRight, fromUp];
        piecesGrid[fromRight, fromUp] = null;
        piecesGrid[toRight, toUp].boardIdx = new Vector2Int(toRight, toUp);
        return;
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        MovePiece(from.x, from.y, to.x, to.y);
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
