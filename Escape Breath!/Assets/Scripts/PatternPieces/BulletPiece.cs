using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPiece : MonoBehaviour
{
    public GameObject shooter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piece"))
        {
            var p = other.GetComponent<Piece>();
            GameManager.inst.chessBoard.RemovePieceFromBoard(p);
            p.isActive = false;
            p.Damaged(true);
            p.rb.AddForce(transform.forward.normalized * 1300f);
            Destroy(shooter);
        }
    }
}
