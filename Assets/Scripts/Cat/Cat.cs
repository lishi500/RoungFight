using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Creature
{
    public Attribute Energy {
        get { return GetAttr(AttrType.Energy); }
    }
    private float m_pending_energy;

    public delegate void CatActionEndEvent(Cat cat);
    public event CatActionEndEvent notifyCatActionEnd;
    private void OnMouseUpAsButton() {
        //Debug.Log("Cat clicked: " + name);
        //if (BoardManager.Instance.IsPlayerActionEnabled) {
        //    BoardManager.Instance.DisablePlayerAction();
        //    Action();
        //}
    }

    public override void BaseAction() {
        NormalAttack();
        //OnCatActionEnd();
    }

    public void NormalAttack() {
        Action baseAttack = new BaseAttackAction(transform.gameObject, enemyParty.boss.gameObject);
        playerParty.actionChain.AddAction(baseAttack);
    }

    public override void OnDie() {
        // cat never die
    }

    public void CastCatPrimarySkill() {
        ClearEnergy();
        NormalAttack();
    }

    public void ChargeEnergy(float energy) {
        if (energy > Energy.maxValue - Energy.value) {
            m_pending_energy += energy - (Energy.maxValue - Energy.value);
        }
        Energy.AddValue(energy);
    }

    protected virtual void OnEnergyChange(Attribute attr) {
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

    private void OnCatActionEnd() {
        if (notifyCatActionEnd != null) {
            notifyCatActionEnd(this);
        }
    }

    private void Awake() {
        if (Energy != null) {
            Energy.notifyValueChange += OnEnergyChange;
        }
    }
}
