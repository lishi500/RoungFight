using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Creature
{
    public Attribute Energy {
        get { return GetAttr(AttrType.Energy); }
    }
    private float m_pending_energy;

    public override void BaseAction() {
        Action baseAttack = new BaseAttackAction(transform.gameObject, enemyParty.boss.gameObject);
        playerParty.actionChain.AddAction(baseAttack);
    }


    public void CastCatPrimarySkill() {
        //Debug.Log("CastCatPrimarySkill");
        //BaseAction();
        ClearEnergy();
        CastSkillAction castSkillAction = new CastSkillAction(gameObject);
        castSkillAction.skill = primarySkill;
        playerParty.actionChain.AddAction(castSkillAction);
    }

    public void ChargeEnergy(float energy) {
        if (energy > Energy.maxValue - Energy.value) {
            m_pending_energy += energy - (Energy.maxValue - Energy.value);
        }
        Energy.AddValue(energy);
    }

    protected virtual void OnEnergyChange(Attribute attr) {
        if (!status.CanAction) {
            return;
        }

        if (Energy.value == Energy.maxValue) {
            CastCatPrimarySkill();
        }

        if (m_pending_energy != 0) {
            float tempPending = m_pending_energy;
            m_pending_energy = 0;
            ChargeEnergy(tempPending);
        }
    }

    private void ClearEnergy() {
        Energy.value = 0;
    }

    private void OnMouseUpAsButton() {}
    public override void OnDie() { } // cat never die

    protected override void Awake() {
        base.Awake();
        if (Energy != null) {
            Energy.notifyValueChange += OnEnergyChange;
        }
    }
}
