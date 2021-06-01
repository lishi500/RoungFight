using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackAction : Action {
    public override void StartAction() {
        Creature from = self.GetComponent<Creature>();
        foreach (GameObject obj in targets) { 
            Creature to = obj.GetComponent<Creature>();
            BaseAttack(from, to);
        }
    }

    public void BaseAttack(Creature from, Creature to) {
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(from.GetAttrVal(AttrType.Attack), from, to, DamageType.NORMAL);
        to.ReduceHealth(damageDef);
    }

}
