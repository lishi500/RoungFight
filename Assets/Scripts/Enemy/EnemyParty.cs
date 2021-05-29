using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParty : Party
{
    public Boss boss;
    public Creeps[] creeps;

    private int m_creeps_cursor = 0;
    private Creeps m_current_creeps;

    public override void StartRound() {
        Debug.Log("enemy party start");

        OnRoundStart();
        // boss action
        BossAction();
    }

    protected void BossAction() {
        boss.notifyEnemyEnd += OnBossActionEnd;
        boss.Action();
    }

    protected void CreepsAction() {
        NextCreeps();
        if (m_current_creeps != null) {
            m_current_creeps.notifyEnemyEnd += OnCreepActionEnd;
            m_current_creeps.Action();
        } else {
            ResetAllActionFlag();
            OnRoundEnd();
        }
    }

    private void NextCreeps() {
        if (creeps != null && creeps.Length > m_creeps_cursor) {
            m_current_creeps = creeps[m_creeps_cursor++];
        }
        m_current_creeps = null;
    }

    void OnBossActionEnd() {
        CreepsAction();
        boss.notifyEnemyEnd -= OnBossActionEnd;
    }

    void OnCreepActionEnd() {
        m_current_creeps.notifyEnemyEnd -= OnCreepActionEnd;
        CreepsAction();
    }

    void ResetAllActionFlag() {
        boss.IsActionFinished = false;
        foreach (Creeps creep in creeps) {
            creep.IsActionFinished = false;
        }
    }

    // Start is called before the first frame update

    private void Awake() {
        boss = GetComponentInChildren<Boss>();
        creeps = GetComponentsInChildren<Creeps>();
    }
}
