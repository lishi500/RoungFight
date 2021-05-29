using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Creature
{
    public delegate void CatActionEndEvent(Cat cat);
    public event CatActionEndEvent notifyCatActionEnd;
    private void OnMouseUpAsButton() {
        Debug.Log("Cat clicked: " + name);
        if (BoardManager.Instance.IsPlayerActionEnabled) {
            BoardManager.Instance.DisablePlayerAction();
            Action();
        }
    }

    public void Action() {
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

    private void OnCatActionEnd() {
        if (notifyCatActionEnd != null) {
            notifyCatActionEnd(this);
        }
    }
}
