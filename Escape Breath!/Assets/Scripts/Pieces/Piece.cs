using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for test
using Valve.VR;

public abstract class Piece : MonoBehaviour
{
    public int damage = 3;
    public int moveLimit = 2;
    public Vector3 attackPos = new Vector3();
    public Vector2Int boardIdx;
    public GameObject laserPrefab;
    private GameObject laserInst;
    public bool isActive = true;
    public bool canMove = true;
    public bool isMoving = false;

    private Rigidbody rb;
    private Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        laserInst = Instantiate(laserPrefab, transform);
        laserInst.SetActive(false);
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

    public Vector2Int DetectFloor()
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("ChessBoard");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, layerMask))
        {
            //Debug.LogWarning("hit, " + hit.point);
            return GameManager.inst.chessBoard.PosToNearIndex(hit.point.x, hit.point.z);
        }
        else
        {
            return new Vector2Int(-1, -1);
        }
    }

    public IEnumerator WhenGrabedCoroutine()
    {
        Vector2Int nextIdx = boardIdx;
        Vector3 nextPos = transform.position;
        col.enabled = false;
        while (isMoving)
        {
            nextIdx = DetectFloor();
            if (!(nextIdx.x < 0 || nextIdx.y < 0))
            {
                laserInst.SetActive(true);
                if (GameManager.inst.chessBoard.GetPiece(nextIdx.x, nextIdx.y) != null) nextIdx = boardIdx;
                nextPos = GameManager.inst.chessBoard.IndexToGlobalPos(nextIdx.x, nextIdx.y);
                laserInst.transform.position = Vector3.Lerp(transform.position, nextPos, 0.5f);
                laserInst.transform.LookAt(transform.position);
                laserInst.transform.localScale = new Vector3(laserInst.transform.localScale.x, laserInst.transform.localScale.y, (transform.position - nextPos).magnitude / 2.5f);
            }
            else
            {
                laserInst.SetActive(false);
            }
            yield return null;
        }
        yield return null;
        Debug.LogWarning(nextIdx + " " + nextPos);
        if (nextIdx.x < 0 || nextIdx.y < 0)
        {
            isActive = false; // dead
            col.enabled = true;
            yield break;
        }
        else
        {
            laserInst.SetActive(false);
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
