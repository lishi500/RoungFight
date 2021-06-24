using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyParty : Party
{
    public Boss boss;
    public List<Creeps> creeps;

    private int m_creeps_cursor = 0;
    private Creeps m_current_creeps;

    public override void StartRound() {
        Debug.Log("enemy party start");
        actionChain.AddAction(new IdleAction(actionChain, 0), 0);
        OnRoundStart();
        // boss action
        BossAction();
    }

    public override void OnActionChainEnd() {
    }

    protected void BossAction() {
        actionChain.notifyActionChainEnd += OnBossActionEnd;

        if (boss.status.CanAction) {
            boss.BaseAction();
        } else {
            OnBossActionEnd();
        }
    }

    protected void CreepsAction() {
        NextCreeps();
        if (m_current_creeps != null) {
            actionChain.notifyActionChainEnd += OnCreepActionEnd;
            if (m_current_creeps.status.CanAction) {
                m_current_creeps.BaseAction();
            } else {
                OnCreepActionEnd();
            }
        } else {
            ResetAllActionFlag();
            //Debug.Log("Enermy Round end");
            OnRoundEnd();
        }
    }

    private void NextCreeps() {
        if (creeps != null && creeps.Count > m_creeps_cursor) {
            m_current_creeps = creeps[m_creeps_cursor++];
        }
        m_current_creeps = null;
    }

    void OnBossActionEnd() {
        Debug.Log("OnBossActionEnd");
        actionChain.notifyActionChainEnd -= OnBossActionEnd;
        if (creeps.Count > 0) {
            CreepsAction();
        } else {
            OnRoundEnd();
        }
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
        creeps = GetComponentsInChildren<Creeps>().ToList();
    }

    public override List<Creature> GetAllCreatures() {
        List<Creature> enemyCreatures = new List<Creature>();

        if (boss != null) { 
            enemyCreatures.Add(boss);
        }

        if (creeps != null && creeps.Count > 0) { 
            enemyCreatures.AddRange(creeps.Cast<Creature>().ToList());
        }
        return enemyCreatures;
    }
}