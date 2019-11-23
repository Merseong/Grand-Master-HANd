using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class OutsideEnemy : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticAction;
    [SerializeField]
    private Transform laser;
    [SerializeField]
    private Transform bullet;
    public Transform target;
    [Space(10)]
    public float flightTime;
    private float timer = 0;

    private Vector3 targetPos;
    private Vector3 targetOffset = new Vector3(0, 0.1f, 0);

    private float lDist;
    private float rDist;

    private void Start()
    {
        while (target == null)
        {
            PieceType randomTarget;
            float rand = Random.Range(0f, 1f);
            if (rand < 0.4f) randomTarget = PieceType.King;
            else if (rand < 0.6f) randomTarget = PieceType.Queen;
            else if (rand < 0.8f) randomTarget = PieceType.Rook;
            else if (rand < 0.9f) randomTarget = PieceType.Bishop;
            else randomTarget = PieceType.Knight;
            target = GameManager.inst.chessBoard.GetRandomPiece(randomTarget).transform;
        }
        StartCoroutine(HapticCoroutine());
        Destroy(gameObject, flightTime + 0.1f);
    }

    private void Update()
    {
        targetPos = target.position + targetOffset;
        laser.LookAt(targetPos);
        transform.LookAt(target);
        laser.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
        laser.localScale = new Vector3(laser.localScale.x, laser.localScale.y, Vector3.Distance(transform.position, targetPos) * 10);
        bullet.position = Vector3.Lerp(transform.position, targetPos, timer / flightTime);
        bullet.LookAt(targetPos);
        timer += Time.unscaledDeltaTime;
    }

    IEnumerator HapticCoroutine()
    {
        float time = 0;
        while (time < flightTime - 0.2f)
        {
            time += Time.unscaledDeltaTime;
            lDist = Vector3.Distance(transform.position, GameManager.inst.leftController.transform.position);
            rDist = Vector3.Distance(transform.position, GameManager.inst.rightController.transform.position);
            SteamVR_Input_Sources handtype = lDist < rDist ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
            hapticAction.Execute(0, 1f, 128, 50 * time / flightTime, handtype);
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
