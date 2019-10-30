using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for test
using Valve.VR;

public abstract class Piece : MonoBehaviour
{
    [Header("Piece Data")]
    public int damage = 3;
    public int moveLimit = 2;
    public Vector3 attackPos = new Vector3();
    public Vector2Int boardIdx;

    [Header("Piece Status")]
    public bool isActive = true;
    public bool canMove = true;
    public bool isMoving = false;

    [Header("for User Interface")]
    public GameObject laserPrefab;
    private GameObject laserInst;
    public GameObject landingPrefab;
    private GameObject landingInst;
    private float landingZOffset = 0.0007f;

    private Rigidbody rb;
    private Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        laserInst = Instantiate(laserPrefab, transform);
        laserInst.SetActive(false);
        landingInst = Instantiate(landingInst, transform);
        landingInst.SetActive(false);

        GameManager.inst.chessBoard.AddPiece(this);
    }

    public abstract void PieceDestroy();

    public void AutoAttack()
    {
        if (damage != 0)
        {
            // auto attack
            var attackObj = Instantiate(GameManager.inst.attackObj, transform.position + attackPos, Quaternion.identity).GetComponent<AttackObj>();
            attackObj.damage = damage;
            attackObj.Init();
        }
    }

    public Vector2Int DetectFloor(out bool isDetected)
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("ChessBoard");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, layerMask))
        {
            //Debug.LogWarning("hit, " + hit.point);
            isDetected = true;
            return GameManager.inst.chessBoard.PosToNearIndex(hit.point.x, hit.point.z);
        }
        else
        {
            isDetected = false;
            return new Vector2Int(-1, -1);
        }
    }

    public IEnumerator WhenGrabedCoroutine()
    {
        Vector2Int nextIdx = boardIdx;
        Vector3 nextPos = transform.position;
        bool isDetected = false;
        col.enabled = false;
        while (isMoving)
        {
            var tempIdx = DetectFloor(out isDetected);
            if (isDetected)
            {
                laserInst.SetActive(true);
                landingInst.SetActive(true);
                if (Mathf.Abs(boardIdx.x - tempIdx.x) < moveLimit && Mathf.Abs(boardIdx.y - tempIdx.y) < moveLimit)
                {
                    if (GameManager.inst.chessBoard.GetPiece(nextIdx.x, nextIdx.y) != null) nextIdx = boardIdx;
                    else nextIdx = tempIdx;
                }
                laserInst.transform.position = Vector3.Lerp(transform.position, nextPos, 0.5f);
                landingInst.transform.localPosition = GameManager.inst.chessBoard.IndexToLocalPos(nextIdx.x, nextIdx.y, landingZOffset);
                laserInst.transform.LookAt(transform.position);
                laserInst.transform.localScale = new Vector3(laserInst.transform.localScale.x, laserInst.transform.localScale.y, (transform.position - nextPos).magnitude / 2.5f);
            }
            else
            {
                landingInst.SetActive(false);
                laserInst.SetActive(false);
            }
            yield return null;
        }
        yield return null;

        //Debug.LogWarning(nextIdx + " " + nextPos);
        laserInst.SetActive(false);
        landingInst.SetActive(false);

        yield return null;
        if (!isDetected)
        {
            isActive = false; // dead
            col.enabled = true;
            // PieceDestroy();
            yield break;
        }
        else
        {
            nextPos = GameManager.inst.chessBoard.IndexToLocalPos(nextIdx.x, nextIdx.y);
            if (boardIdx != nextIdx) GameManager.inst.chessBoard.MovePiece(boardIdx, nextIdx);
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            
            float time = 1;
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(transform.localPosition, nextPos, timer / time);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, timer / time);
                yield return null;
            }

            rb.isKinematic = false;
            col.enabled = true;
            yield break;
        }
    }
    // 들고있을때 이것저것 표시 (어디에 옮겨질지나, 그런거)
}
