using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [Header("Board Data")]
    private int maxBoardIndex = 7;
    private Vector2 zeroOffset = new Vector2(-0.21f, -0.21f);
    private float indexOffset = 0.06f;

    [Header("Piece Set")]
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

    public delegate void PieceAction();
    public PieceAction allAttack;
    public PieceAction allReset;
    public PieceAction allBeforeAttack;

    [Header("Show On Board")]
    public GameObject DangerAreaPrefab;
    public GameObject moveableAreaPrefab;
    private List<GameObject> DangerAreaList = new List<GameObject>();
    private List<GameObject> moveableAreaList = new List<GameObject>();

    public bool IsInBoardIdx(int right, int up)
    {
        if (0 > right || right > maxBoardIndex || 0 > up || up > maxBoardIndex) return false;
        else return true;
    }

    public Vector3 IndexToLocalPos(int right, int up, float zOffset = 0)
    {
        if (!IsInBoardIdx(right, up))
        {
            right = Mathf.Clamp(right, 0, maxBoardIndex);
            up = Mathf.Clamp(up, 0, maxBoardIndex);
            Debug.LogWarning("[WARN-IndexToPos] bound exceeded, clamped");
        }
        return new Vector3(zeroOffset.y + up * indexOffset, zeroOffset.x + right * indexOffset, zOffset);
    }

    public Vector3 IndexToGlobalPos(int right, int up, float zOffset = 0)
    {
        var lPos = 3 * IndexToLocalPos(right, up);
        return new Vector3(lPos.y, zOffset, lPos.x) + transform.position;
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
            allAttack += p.AutoAttack;
            allReset += p.ResetAfterTurnEnd;
            allBeforeAttack += p.BeforeAttack;
        }
        return true;
    }

    public void TemporalyRemovePiece(Piece p)
    {
        piecesGrid[p.boardIdx.x, p.boardIdx.y] = null;
    }

    public void TemporalyReturnPiece(Piece p)
    {
        piecesGrid[p.boardIdx.x, p.boardIdx.y] = p;
    }

    public Piece GetPiece(int right, int up)
    {
        if (!IsInBoardIdx(right, up))
        {
            Debug.LogError("[ERR-GetPiece] bound exceeded: " + right + ", " + up);
            return null;
        }
        else return piecesGrid[right, up];
    }

    public Piece GetPiece(Vector2Int idx)
    {
        return GetPiece(idx.x, idx.y);
    }

    public void MovePiece(int fromRight, int fromUp, int toRight, int toUp)
    {
        if (!IsInBoardIdx(fromRight, fromUp))
        {
            Debug.LogError("[ERR-MovePiece] from bound exceeded");
            return;
        }
        if (!IsInBoardIdx(toRight, toUp))
        {
            Debug.LogError("[ERR-MovePiece] to bound exceeded");
            return;
        }
        piecesGrid[toRight, toUp] = piecesGrid[fromRight, fromUp];
        Debug.Log(piecesGrid[toRight, toUp]);
        if (!(fromRight == toRight && fromUp == toUp))
        {
            piecesGrid[fromRight, fromUp] = null;
            piecesGrid[toRight, toUp].boardIdx = new Vector2Int(toRight, toUp);
            Debug.Log("Move from " + fromRight + ", " + fromUp + " to " + toRight + ", " + toUp);
        }
        StartCoroutine(piecesGrid[toRight, toUp].MovePieceCoroutine(IndexToLocalPos(toRight, toUp), 1f));
        return;
    }

    public void MovePiece(Piece p, int toRight, int toUp)
    {
        if (!IsInBoardIdx(toRight, toUp))
        {
            Debug.LogError("[ERR-MovePiece] to bound exceeded");
            return;
        }
        piecesGrid[toRight, toUp] = p;
        p.boardIdx = new Vector2Int(toRight, toUp);
        StartCoroutine(p.MovePieceCoroutine(IndexToLocalPos(toRight, toUp), 1f));
        return;
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        MovePiece(from.x, from.y, to.x, to.y);
    }

    public bool MovePieceWithDir(Vector2Int pieceIdx, Vector2 dir)
    {
        Vector2Int nextIdx = pieceIdx + (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? new Vector2Int(Mathf.RoundToInt(dir.x / Mathf.Abs(dir.x)), 0) : new Vector2Int(0, Mathf.RoundToInt(dir.y / Mathf.Abs(dir.y))));

        if (IsInBoardIdx(nextIdx.x, nextIdx.y))
        {
            if (GetPiece(nextIdx))
            {
                if (MovePieceWithDir(nextIdx, dir))
                {
                    MovePiece(pieceIdx, nextIdx);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MovePiece(pieceIdx, nextIdx);
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public void ShowAttackArea(int right, int up, float duration, bool isStrong = false)
    {
        // show attack mark on [right, up] for duration
    }

    public void ShowMoveArea(int right, int up, int limit)
    {
        for (int i = -limit; i <= limit; ++i)
        {
            for (int j = -limit; j <= limit; ++j)
            {
                if (IsInBoardIdx(right + i, up + j))
                {
                    var isPiece = GetPiece(right + i, up + j);
                    Debug.Log((right + i) + ", " + (up + j) + ", " + isPiece);
                    if (Mathf.Abs(i) + Mathf.Abs(j) <= limit && isPiece == null)
                    {
                        var newMoveableArea = Instantiate(moveableAreaPrefab, transform);
                        newMoveableArea.transform.localPosition = IndexToLocalPos(right + i, up + j, 0.002f);
                        moveableAreaList.Add(newMoveableArea);
                    }
                }
            }
        }
    }

    public void ShowMoveArea(Vector2Int boardIdx, int limit)
    {
        ShowMoveArea(boardIdx.x, boardIdx.y, limit);
    }

    public void HideMoveArea()
    {
        while (moveableAreaList.Count > 0)
        {
            Destroy(moveableAreaList[0]);
            moveableAreaList.RemoveAt(0);
        }
    }

    private void Awake()
    {
        // init all pieces
        allBeforeAttack = new PieceAction(() =>
        {
            Debug.Log("All beforeAttack");
        });
        allAttack = new PieceAction(() =>
        {
            Debug.Log("All attack");
        });
        allReset = new PieceAction(() =>
        {
            Debug.Log("All reset");
        });
    }
}
