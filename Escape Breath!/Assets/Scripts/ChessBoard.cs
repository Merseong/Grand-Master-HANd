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

    [Header("Show On Board")]
    public GameObject DangerAreaPrefab;
    public GameObject WeakDangerAreaPrefab;
    public GameObject moveableAreaPrefab;
    private List<GameObject> DangerAreaList = new List<GameObject>();
    private List<GameObject> moveableAreaList = new List<GameObject>();

    public bool IsInBoardIdx(int right, int up)
    {
        if (0 > right || right > maxBoardIndex || 0 > up || up > maxBoardIndex) return false;
        else return true;
    }

    public Vector2 IndexToLocalPos(int right, int up, float zOffset = 0)
    {
        if (!IsInBoardIdx(right, up))
        {
            right = Mathf.Clamp(right, 0, maxBoardIndex);
            up = Mathf.Clamp(up, 0, maxBoardIndex);
            Debug.LogWarning("[WARN-IndexToPos] bound exceeded, clamped");
        }
        return new Vector3(zeroOffset.y + up * indexOffset, zeroOffset.x + right * indexOffset, zOffset);
    }

    public Vector3 IndexToGlobalPos(int right, int up, float zOffset = 0) //vector2int 로 하면 안되냐 안됨말고
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
        }
        return true;
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
        piecesGrid[fromRight, fromUp] = null;
        piecesGrid[toRight, toUp].boardIdx = new Vector2Int(toRight, toUp);
        return;
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        MovePiece(from.x, from.y, to.x, to.y);
    }

    public void ShowAttackArea(Vector2Int pos, float duration, bool isStrong = false) //ㅎ right, up => Vector2Int
    {
        if (isStrong)
        {
            var newDangerArea = Instantiate(DangerAreaPrefab, transform);
            newDangerArea.transform.localPosition = IndexToLocalPos(pos.x, pos.y, 0.001f);
            Destroy(newDangerArea, duration);
        }
        else
        {
            var newWeakDangerArea = Instantiate(WeakDangerAreaPrefab, transform);
            newWeakDangerArea.transform.localPosition = IndexToLocalPos(pos.x, pos.y, 0.001f);
            Destroy(newWeakDangerArea, duration);
        }
        Debug.Log("danger area one appear");
    }

    public void ShowMoveArea(int right, int up, int limit)
    {
        for (int i = -limit; i <= limit; ++i)
        {
            for (int j = -limit; j <= limit; ++j)
            {
                if (IsInBoardIdx(right + i, up + j))
                {
                    //Debug.Log(i + ", " + j);
                    var isPiece = GetPiece(right + i, up + j);
                    if ((i == 0 && j == 0) || (Mathf.Abs(i) + Mathf.Abs(j) <= limit && isPiece == null))
                    {
                        var newMoveableArea = Instantiate(moveableAreaPrefab, transform);
                        newMoveableArea.transform.localPosition = IndexToLocalPos(right + i, up + j, 0.001f);
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
    public void HideDangerArea()
    {
        while(DangerAreaList.Count > 0)
        {
            Destroy(DangerAreaList[0]);
            DangerAreaList.RemoveAt(0);
        }
    }

    private void Awake()
    {
        // init all pieces
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
