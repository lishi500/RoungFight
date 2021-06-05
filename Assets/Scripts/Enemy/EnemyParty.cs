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

    public override void OnActionChainEnd() {
        
    }

    protected void BossAction() {
        actionChain.notifyActionChainEnd += OnBossActionEnd;
        boss.BaseAction();
    }

    protected void CreepsAction() {
        NextCreeps();
        if (m_current_creeps != null) {
            actionChain.notifyActionChainEnd += OnCreepActionEnd;
            m_current_creeps.BaseAction();
        } else {
            ResetAllActionFlag();
            Debug.Log("Enermy Round end");
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
        actionChain.notifyActionChainEnd -= OnBossActionEnd;
        CreepsAction();
    }

    void OnCreepActionEnd() {
        actionChain.notifyActionChainEnd -= OnCreepActionEnd;
        CreepsAction();
    }

    void ResetAllActionFlag() {
        boss.IsActionFinished = false;
        foreach (Creeps creep in creeps) {
            creep.IsActionFinished = false;
        }
    }

    // Start is called before the first frame update

    protected override void Awake() {
        base.Awake();
        boss = GetComponentInChildren<Boss>();
        creeps = GetComponentsInChildren<Creeps>();
    }

   
}