using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour
{
    private float maxHealth;
    public int health;
    public int phase;
    public List<GameObject> phasePatterns = new List<GameObject>();

    [Space(10)]
    public GameObject outsideAttackerObj;

    [Space(10)]
    public int attackType;
    public Transform bossTarget;
    [SerializeField]
    private Slider redHealth = null;
    private float redValue = 1;
    [SerializeField]
    private Slider yellowHealth = null;
    private float yellowValue = 1;
    public Object attackArea;
    
    public Transform attackPoint;
    [HideInInspector]
    public Rigidbody rb;

    bool isClose = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxHealth = health;
    }

    //test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //meteor test
        {
            Debug.Log("test Space");
            var A = Instantiate(phasePatterns[1]).GetComponent<BossPattern>();
            A.StartPattern();
        }
    }
    //test
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            var obj = other.GetComponent<AttackObj>();
            Damaged(obj.damage);

            obj.rb.isKinematic = true;
            obj.rb.velocity = Vector3.zero;
            obj.transform.position = transform.position;
        }
        else if (other.CompareTag("Piece"))
        {
            var obj = other.GetComponent<Piece>();
            Damaged(obj.damage);

            GameManager.inst.chessBoard.PermanantRemovePiece(obj);
        }
    }

    private void Damaged(int damage)
    {
        if (!GameManager.inst.isPlaying) return;
        health = Mathf.Max(0, health - damage * 3);
        redValue = health / maxHealth;
        redHealth.value = redValue;
        if (health == 0) GameManager.inst.GameClear();
    }

    public void ResetYellowHealth()
    {
        yellowValue = redValue;
        yellowHealth.value = yellowValue;
    }

    private BossPattern SelectPattern()
    {
        // randomly make outside attacker
        if (Random.Range(0f, 1f) < 0.5f)
        {
            SpawnOutsideAttacker();
        }

        // select random pattern or biased pattern
        if (isClose)
            return Instantiate(phasePatterns[0]).GetComponent<BossPattern>(); //근접 공격 phasepatterns 임시 숫자임
        else
        {
            int idx = Random.Range(0, phasePatterns.Count);
            return Instantiate(phasePatterns[idx]).GetComponent<BossPattern>();
        }
    }

    public void StartPattern()
    {
        var pattern = SelectPattern();
        pattern.StartPattern();
    }

    void SpawnOutsideAttacker()
    {
        Vector3 randomPos = new Vector3(Random.Range(-3f, 3f), Random.Range(1f, 3f), Random.Range(-3f, 3f));
        var outside = Instantiate(outsideAttackerObj, randomPos, Quaternion.identity).GetComponent<OutsideEnemy>();
        outside.target = GameManager.inst.chessBoard.GetRandomPiece().transform;
    }
    
    public void CheckClose()
    {
        for (int b = 5; b < 8; b++)
        {
            for (int a = 0; a < 8; a++)
            {
                if (GameManager.inst.chessBoard.GetPiece(a, b) != null)
                    isClose = true;
            }
        }
    }

    public void AttackOnBoard(Vector2Int pos, float duration, bool isStrong = false)
    {
        StartCoroutine(AttackPiece(pos, duration, isStrong));
        GameManager.inst.chessBoard.ShowAttackArea(pos, duration - 0.05f, isStrong);
    }

    IEnumerator AttackPiece(Vector2Int pos, float time, bool isStrong)
    {
        yield return new WaitForSeconds(time);

        if (GameManager.inst.chessBoard.GetPiece(pos.x, pos.y) != null)
        {
            GameManager.inst.chessBoard.GetPiece(pos.x, pos.y).Damaged();
        }
        yield return null;
    }
}
