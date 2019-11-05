using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for test
using Valve.VR;

public abstract class Piece : MonoBehaviour
{
    [Header("Piece Data")]
    public bool isAttacker = true;
    public int damage = 3;
    private int originalDamage;
    public int moveLimit = 2;
    public Vector2Int boardIdx;

    [Header("Piece Status")]
    public bool isAlive = true;
    public bool isActive = true;
    public bool canMove = true;
    public bool isMoving = false;
    public bool isProtected = false;

    [Header("for User Interface")]
    public GameObject laserPrefab;
    private GameObject laserInst;
    public GameObject landingPrefab;
    private GameObject landingInst;
    private float landingZOffset = 0.01f;
    public Vector3 attackPos = new Vector3();
    private Rigidbody rb;
    private Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        laserInst = Instantiate(laserPrefab, transform);
        laserInst.SetActive(false);
        landingInst = Instantiate(landingPrefab, transform);
        landingInst.transform.localPosition = new Vector3(0, 0, landingZOffset * landingZOffset);

        GameManager.inst.chessBoard.AddPiece(this);

        originalDamage = damage;
    }

    public abstract void PieceDestroy();

    public virtual void BeforeAttack() { }

    public void AutoAttack()
    {
        BeforeAttack(); // 이거를 또 델리게이트로 만들어서 옮겨야될듯
        if (isActive && isAttacker && damage != 0)
        {
            // auto attack
            var attackObj = Instantiate(GameManager.inst.attackObj, transform.position + attackPos, Quaternion.identity).GetComponent<AttackObj>();
            attackObj.damage = damage;
            attackObj.Init();
        }
    }

    public void Damaged(bool isStrong = false)
    {
        if (isStrong || !isProtected)
        {
            PieceDestroy();
        }
    }

    public void ResetAfterTurnEnd()
    {
        if (isActive)
        {
            switch(GameManager.inst.turnSystem.currentTurn)
            {
                case TurnType.AttackReady:
                case TurnType.Attack:
                    canMove = false;
                    landingInst.SetActive(false);
                    // 원래자리로 돌아가게 하는건데 쓸진 고민중
                    if (!isMoving) StartCoroutine(MovePieceCoroutine(GameManager.inst.chessBoard.IndexToLocalPos(boardIdx.x, boardIdx.y), 0.2f));
                    break;
                case TurnType.MovePiece:
                    damage = originalDamage;
                    landingInst.transform.localPosition = new Vector3(0, 0, landingZOffset * landingZOffset);
                    landingInst.transform.localRotation = Quaternion.identity;
                    landingInst.SetActive(true);
                    isProtected = false;
                    canMove = true;
                    break;
            }
        }
        else
        {
            canMove = true;
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
        Debug.Log(GameManager.inst.chessBoard.GetPiece(boardIdx));
        Vector2Int nextIdx = boardIdx;
        Vector3 nextPos;
        bool isDetected = false;
        col.enabled = false;
        while (isMoving)
        {
            var tempIdx = DetectFloor(out isDetected);
            if (isDetected)
            {
                laserInst.SetActive(true);
                landingInst.SetActive(true);
                if (Mathf.Abs(boardIdx.x - tempIdx.x) + Mathf.Abs(boardIdx.y - tempIdx.y) <= moveLimit)
                {
                    //Debug.Log(boardIdx + ", " + tempIdx);
                    if (GameManager.inst.chessBoard.CheckPiece(tempIdx)) nextIdx = boardIdx;
                    else nextIdx = tempIdx;
                }
                nextPos = GameManager.inst.chessBoard.IndexToGlobalPos(nextIdx.x, nextIdx.y, landingZOffset);
                laserInst.transform.position = Vector3.Lerp(transform.position, nextPos, 0.5f);
                landingInst.transform.position = nextPos;
                laserInst.transform.LookAt(transform.position);
                landingInst.transform.rotation = Quaternion.Euler(90, 0, 0);
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

        if (!isDetected)
        {
            isActive = false;
            col.enabled = true;
            isMoving = false;
            // PieceDestroy();
        }
        else
        {
            isActive = true;
            GameManager.inst.chessBoard.MovePiece(this, nextIdx);
            GameManager.inst.chessBoard.HideMoveArea(boardIdx);
        }
    }

    public IEnumerator MovePieceCoroutine(Vector3 nextLocalPos, float duration)
    {
        rb.velocity = Vector3.zero;
        col.enabled = false;
        isMoving = true;
        rb.isKinematic = true;

        float timer = 0;
        while (timer < duration * 0.8f)
        {
            timer += Time.unscaledDeltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, nextLocalPos, timer / duration);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, timer / duration);
            yield return null;
        }
        transform.localPosition = nextLocalPos;
        transform.localRotation = Quaternion.identity;

        isMoving = false;
        rb.isKinematic = false;
        col.enabled = true;
        //Debug.Log("End Moving");
    }
}
