using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Creature
{
    public Attribute Energy {
        get { return GetAttr(AttrType.Energy); }
    }
    private int m_pending_energy;

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
        OnCatActionEnd();
    }

    public void NormalAttack() {
        Boss boss = BoardManager.Instance.enemyParty.boss;
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(GetAttrVal(AttrType.Attack), this, boss, DamageType.NORMAL);
        boss.ReduceHealth(damageDef);
    }

    public override void OnDie() {
        // cat never die
    }

    public void CastCatPrimarySkill() {
        
    }

    public void ChargeEnergy(int energy) {
        if (energy > Energy.maxValue - Energy.value) {
            m_pending_energy += energy - (int)(Energy.maxValue - Energy.value);
        }
        Energy.AddValue(energy);
        // TODO boost by energy gain
    }

    protected virtual void OnEnergyChange() {
        if (Energy.value == Energy.maxValue) {
            CastCatPrimarySkill();
        }
    }

    private void OnCatActionEnd() {
        if (notifyCatActionEnd != null) {
            notifyCatActionEnd(this);
        }
    }
}
