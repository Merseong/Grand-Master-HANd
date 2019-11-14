using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    NULL,
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public abstract class Piece : MonoBehaviour
{
    [Header("Piece Data")]
    public PieceType pieceType;
    public bool isAttacker = true;
    public int damage = 3;
    private int originalDamage;
    public int moveLimit = 2;
    public Vector2Int boardIdx;
    public float rechargePoint = 0;

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
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public bool isFloorDetected = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        laserInst = Instantiate(laserPrefab, transform);
        laserInst.SetActive(false);
        landingInst = Instantiate(landingPrefab, transform);
        landingInst.transform.localPosition = new Vector3(0, 0, landingZOffset * 0.2f);
        GetComponent<Outline>().enabled = false;

        GameManager.inst.chessBoard.AddPiece(this);

        originalDamage = damage;
    }

    public virtual void PieceDestroy()
    {
        GetComponent<MeshRenderer>().material = GameManager.inst.blackMat;
        landingInst.SetActive(false);
        rechargePoint = 0;
        isAlive = false;
    }

    public void Resurrection()
    {
        if (!isAlive)
        {
            isAlive = true;
            rechargePoint = 0;
            GetComponent<MeshRenderer>().material = GameManager.inst.whiteMat;
        }
    }

    public virtual void BeforeAttack() { }

    public void AutoAttack()
    {
        if (isAlive && isActive && isAttacker && damage != 0)
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
        if (!GameManager.inst.isPlaying)
        {
            canMove = true;
            landingInst.SetActive(false);
            isActive = false;
        }
        else if (isActive)
        {
            switch(GameManager.inst.turnSystem.currentTurn)
            {
                case TurnType.AttackReady:
                case TurnType.Attack:
                    canMove = false;
                    landingInst.SetActive(false);
                    GetComponent<Outline>().OutlineColor = Color.red;
                    // 원래자리로 돌아가게 하는건데 쓸진 고민중
                    if (!isMoving) StartCoroutine(MovePieceCoroutine(GameManager.inst.chessBoard.IndexToLocalPos(boardIdx.x, boardIdx.y), 0.2f));
                    break;
                case TurnType.MovePiece:
                    SpecialReset();
                    damage = originalDamage;
                    landingInst.transform.localPosition = new Vector3(0, 0, landingZOffset * 0.2f);
                    landingInst.transform.localRotation = Quaternion.identity;
                    landingInst.SetActive(true);
                    isProtected = false;
                    canMove = true;
                    GetComponent<Outline>().OutlineColor = Color.green;
                    break;
            }
        }
        else
        {
            canMove = true;
        }
    }

    protected virtual void SpecialReset() { } 

    public Vector2Int DetectFloor(out bool isFloorDetected)
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("ChessBoard");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, layerMask))
        {
            //Debug.LogWarning("hit, " + hit.point);
            isFloorDetected = true;
            return GameManager.inst.chessBoard.PosToNearIndex(hit.point.x, hit.point.z);
        }
        else
        {
            isFloorDetected = false;
            return new Vector2Int(-1, -1);
        }
    }

    public IEnumerator WhenGrabedCoroutine()
    {
        //Debug.Log(GameManager.inst.chessBoard.GetPiece(boardIdx));
        Vector2Int nextIdx = boardIdx;
        Vector3 nextPos;
        gameObject.layer = LayerMask.NameToLayer("MovingPiece");
        isFloorDetected = false;
        while (isMoving)
        {
            var tempIdx = DetectFloor(out isFloorDetected);
            if (isFloorDetected)
            {
                laserInst.SetActive(true);
                landingInst.SetActive(true);
                if (Mathf.Abs(boardIdx.x - tempIdx.x) + Mathf.Abs(boardIdx.y - tempIdx.y) <= moveLimit)
                {
                    //Debug.Log(boardIdx + ", " + tempIdx);
                    if (GameManager.inst.chessBoard.CheckPiece(tempIdx)) nextIdx = GameManager.inst.chessBoard.GetNearestIndex(boardIdx);
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

        if (!isFloorDetected)
        {
            isActive = false;
            gameObject.layer = LayerMask.NameToLayer("Piece");
            isMoving = false;
            GetComponent<Outline>().OutlineColor = Color.red;
            // PieceDestroy();
            //Debug.Log(this + " go outside");
        }
        else
        {
            isActive = true;
            GetComponent<Outline>().OutlineColor = Color.red;
            GameManager.inst.chessBoard.MovePiece(this, nextIdx);
            GameManager.inst.chessBoard.HideMoveArea(boardIdx);
        }
    }

    public IEnumerator MovePieceCoroutine(Vector3 nextLocalPos, float duration)
    {
        rb.velocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("MovingPiece");
        isMoving = true;
        rb.isKinematic = true;

        float timer = 0;
        while (GameManager.inst.isPlaying && timer < duration * 0.8f)
        {
            timer += Time.unscaledDeltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, nextLocalPos, timer / duration);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, timer / duration);
            yield return null;
        }
        if (GameManager.inst.isPlaying)
        {
            transform.localPosition = nextLocalPos;
            transform.localRotation = Quaternion.identity;
        }
        

        isMoving = false;
        rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Piece");
        //Debug.Log("End Moving");
    }
}
