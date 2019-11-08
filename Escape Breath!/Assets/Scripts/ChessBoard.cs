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
    private Dictionary<Vector2Int, Piece> piecesGrid = new Dictionary<Vector2Int, Piece>();
    public List<Piece> pieceList = new List<Piece>();

    public delegate void PieceAction();
    public PieceAction allAttack;
    public PieceAction allReset;
    public PieceAction allBeforeAttack;

    [Space(10)]
    public Transform pieceSetTransform;
    [Tooltip("0: queen, 1: knight, 2: bishop, 3: rook")]
    public GameObject[] pieceGameObjects;

    [Header("Show On Board")]
    public GameObject DangerAreaPrefab;
    public GameObject WeakDangerAreaPrefab;
    public GameObject moveableAreaPrefab;
    private List<GameObject> DangerAreaList = new List<GameObject>();
    private Dictionary<Vector2Int, GameObject> moveableAreaListR = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> moveableAreaListL = new Dictionary<Vector2Int, GameObject>();

    public bool IsInBoardIdx(int right, int up)
    {
        if (0 > right || right > maxBoardIndex || 0 > up || up > maxBoardIndex) return false;
        else return true;
    }
    public bool IsInBoardIdx(Vector2Int idx)
    {
        return IsInBoardIdx(idx.x, idx.y);
    }

    public bool CheckPiece(Vector2Int idx)
    {
        return CheckPiece(idx.x, idx.y);
    }
    public bool CheckPiece(int right, int up)
    {
        //string newStr = "\n";
        //for (int i = 0; i < maxBoardIndex; i++)
        //{
        //    for (int j = 0; j < maxBoardIndex; j++)
        //    {
        //        newStr += piecesGrid.ContainsKey(new Vector2Int(i, j)) ? "1" : "0";
        //    }
        //    newStr += "\n";
        //}
        //Debug.LogError(newStr);
        return piecesGrid.ContainsKey(new Vector2Int(right, up));
    }

    public Piece GetPiece(Vector2Int idx)
    {
        piecesGrid.TryGetValue(idx, out Piece output);
        return output;
    }

    public Piece GetPiece(int right, int up)
    {
        return GetPiece(new Vector2Int(right, up));
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

    public Vector3 IndexToGlobalPos(int right, int up, float zOffset = 0) //vector2int 로 하면 안되냐 안됨말고
    {
        var lPos = 3 * IndexToLocalPos(right, up);
        return new Vector3(lPos.y, zOffset, lPos.x) + transform.position;
    }

    // offset: 0.06f -> 0.18f, area -0.63 ~ 0.63
    public Vector2Int PosToNearIndex(float x, float z)
    {
        int i = Mathf.Clamp(Mathf.RoundToInt((x - zeroOffset.x * 3) / (indexOffset * 3)), 0, maxBoardIndex);
        int j = Mathf.Clamp(Mathf.RoundToInt((z - zeroOffset.y * 3) / (indexOffset * 3)), 0, maxBoardIndex);
        return new Vector2Int(i, j);
    }

    public void AddPiece(Piece p)
    {
        var idx = PosToNearIndex(p.transform.position.x, p.transform.position.z);
        if (CheckPiece(idx))
        {
            Debug.LogError("[ERR-AddPiece] already piece on " + idx);
        }
        else
        {
            piecesGrid.Add(idx, p);
            p.boardIdx = idx;
            pieceList.Add(p);

            allAttack += p.AutoAttack;
            allReset += p.ResetAfterTurnEnd;
            allBeforeAttack += p.BeforeAttack;
        }
    }

    public void PromotePawns(bool isRight, int targetNum)
    {
        int targetX = isRight ? 6 : 0;
        if (CheckPiece(targetX, 7))
        {
            Piece p = GetPiece(targetX, 7);
            Instantiate(pieceGameObjects[targetNum], IndexToGlobalPos(p.boardIdx.x, p.boardIdx.y), Quaternion.Euler(-90, 0, 0), pieceSetTransform);
            PermanantRemovePiece(p);
        }
        if (CheckPiece(targetX + 1, 7))
        {
            Piece p = GetPiece(targetX + 1, 7);
            Instantiate(pieceGameObjects[targetNum], IndexToGlobalPos(p.boardIdx.x, p.boardIdx.y), Quaternion.Euler(-90, 0, 0), pieceSetTransform);
            PermanantRemovePiece(p);
        }
    }

    public void PermanantRemovePiece(Piece p)
    {
        if (piecesGrid.ContainsKey(p.boardIdx)) piecesGrid.Remove(p.boardIdx);
        allAttack -= p.AutoAttack;
        allReset -= p.ResetAfterTurnEnd;
        allBeforeAttack -= p.BeforeAttack;
        p.PieceDestroy();
        Destroy(p.gameObject);
    }

    public void RemovePieceFromBoard(Piece p)
    {
        if (p.isActive)
            piecesGrid.Remove(p.boardIdx);
    }

    public void MovePiece(Piece p, Vector2Int nextPos)
    {
        if (!IsInBoardIdx(nextPos))
        {
            Debug.LogError("[ERR-MovePiece] nextPos exeed range");
            return;
        }
        else if (CheckPiece(nextPos))
        {
            Debug.LogError("[ERR-MovePiece] already piece on nextPos");
            return;
        }
        
        if (piecesGrid.TryGetValue(p.boardIdx, out Piece _p) && p == _p)
        {
            RemovePieceFromBoard(_p);
        }
        piecesGrid.Add(nextPos, p);
        p.boardIdx = nextPos;
        StartCoroutine(p.MovePieceCoroutine(IndexToLocalPos(nextPos.x, nextPos.y), 1f));
        //Debug.Log("Move " + p + " to " + nextPos);
    }

    public void MovePiece(Vector2Int fromIdx, Vector2Int toIdx)
    {
        if (CheckPiece(fromIdx) && !CheckPiece(toIdx))
        {
            MovePiece(GetPiece(fromIdx), toIdx);
        }
    }

    public bool MovePieceWithDir(Vector2Int pieceIdx, Vector2 dir)
    {
        Vector2Int nextIdx = pieceIdx + (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? new Vector2Int(Mathf.RoundToInt(dir.x / Mathf.Abs(dir.x)), 0) : new Vector2Int(0, Mathf.RoundToInt(dir.y / Mathf.Abs(dir.y))));

        if (IsInBoardIdx(nextIdx.x, nextIdx.y))
        {
            if (CheckPiece(nextIdx))
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

    public void ShowMoveArea(int right, int up, int limit, bool isRightHand)
    {
        for (int i = -limit; i <= limit; ++i)
        {
            for (int j = -limit; j <= limit; ++j)
            {
                if (IsInBoardIdx(right + i, up + j) && !CheckPiece(right + i, up + j))
                {
                    //Debug.Log((right + i) + ", " + (up + j));
                    if (Mathf.Abs(i) + Mathf.Abs(j) <= limit)
                    {
                        Vector2Int newIdx = new Vector2Int(right + i, up + j);
                        var newMoveableArea = Instantiate(moveableAreaPrefab, transform);
                        newMoveableArea.transform.localPosition = IndexToLocalPos(right + i, up + j, 0.002f);
                        if (isRightHand) moveableAreaListR.Add(newIdx, newMoveableArea);
                        else moveableAreaListL.Add(newIdx, newMoveableArea);
                    }
                }
            }
        }
    }

    public void ShowMoveArea(Vector2Int boardIdx, int limit, bool isRightHand)
    {
        ShowMoveArea(boardIdx.x, boardIdx.y, limit, isRightHand);
    }

    public void HideMoveArea(bool isRightHand, Piece droped)
    {
        if (isRightHand)
        {
            List<GameObject> values = new List<GameObject>(moveableAreaListR.Values);
            for (int i = 0; i < values.Count; ++i)
            {
                Destroy(values[i]);
            }
            moveableAreaListR.Clear();
        }
        else
        {
            List<GameObject> values = new List<GameObject>(moveableAreaListL.Values);
            for (int i = 0; i < values.Count; ++i)
            {
                Destroy(values[i]);
            }
            moveableAreaListL.Clear();
        }
    }

    public void HideMoveArea(Vector2Int pos)
    {
        if (moveableAreaListL.TryGetValue(pos, out GameObject vala))
        {
            Destroy(vala);
            moveableAreaListL.Remove(pos);
        }
        if (moveableAreaListR.TryGetValue(pos, out GameObject valb))
        {
            Destroy(valb);
            moveableAreaListR.Remove(pos);
        }
    }

    public Vector2Int GetNearestIndex(Vector2Int boardidx)
    {
        int dist = 0;
        Vector2Int output = boardidx;
        while (true)
        {
            for (int i = -dist; i <= dist; ++i)
            {
                int j = -(dist - Mathf.Abs(i));
                output.x = boardidx.x + i;
                output.y = boardidx.y + j;
                if (IsInBoardIdx(output) && !CheckPiece(output))
                {
                    return output;
                }

                j = (dist - Mathf.Abs(i));
                output.x = boardidx.x + i;
                output.y = boardidx.y + j;
                if (IsInBoardIdx(output) && !CheckPiece(output))
                {
                    return output;
                }
            }
            dist++;
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
