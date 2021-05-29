using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Creature
{
    public bool IsActionFinished = false;
    //public delegate void EnemyActionStartEvent();
    //public event EnemyActionStartEvent notifyPlayerStart;
    public delegate void EnemyActionEndEvent();
    public event EnemyActionEndEvent notifyEnemyEnd;

    protected void ActionFinished() {
        IsActionFinished = true;
        if (notifyEnemyEnd != null) {
            notifyEnemyEnd();
        }
    }
}
