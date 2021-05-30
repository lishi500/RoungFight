using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackAction : Action
{
    public override void StartAction() {
        Creature from = self.GetComponent<Creature>();
        foreach (GameObject obj in targets) { 
            Creature to = obj.GetComponent<Creature>();
            BaseAttack(from, to);
        }
    }

    public void BaseAttack(Creature from, Creature to) {
        Boss boss = BoardManager.Instance.enemyParty.boss;
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(from.GetAttrVal(AttrType.Attack), from, boss, DamageType.NORMAL);
        boss.ReduceHealth(damageDef);
    }

}
