using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackAction : Action
{
    public BaseAttackAction(GameObject self, GameObject target) : base(self, target) {}
    public BaseAttackAction(GameObject self, List<GameObject> targets) : base(self, targets) {}

    public override void StartAction() {
        Creature creature = self.GetComponent<Creature>();
        foreach (Creature to in toS) {
            BaseAttack(from, to);
        }
        ActionEnd();
    }

    public void BaseAttack(Creature from, Creature to) {
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(from.GetAttrVal(AttrType.Attack), from, to, DamageType.NORMAL);
        to.ReduceHealth(damageDef);
    }

}
