using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPattern : MonoBehaviour
{
    protected List<Vector2Int> targets;

    public abstract void StartPattern();

    protected abstract void SelectTarget();
}
