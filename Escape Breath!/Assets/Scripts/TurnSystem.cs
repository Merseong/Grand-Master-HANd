using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnType
{
    AttackReady,
    Move,
    Attack
}

public class TurnSystem : MonoBehaviour
{
    public int turn = 0;
    public TurnType turnType = TurnType.Move;
}
