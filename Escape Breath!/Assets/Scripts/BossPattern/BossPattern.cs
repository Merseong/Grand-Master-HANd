using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPattern : MonoBehaviour
{
    protected Queue<Vector2Int> targets = new Queue<Vector2Int>();

    public abstract void StartPattern();

    protected abstract void SelectTarget();
}
