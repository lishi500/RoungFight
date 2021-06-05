using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackAction : Action
{
    CustomAnimationController animationController;

    public BaseAttackAction(GameObject self, GameObject target) : base(self, target) {}
    public BaseAttackAction(GameObject self, List<GameObject> targets) : base(self, targets) {}

    public override void StartAction() {
        AttackAnimation();
    }

    public void BaseAttack(Creature from, Creature to) {
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(from.GetAttrVal(AttrType.Attack), from, to, DamageType.NORMAL);
        to.ReduceHealth(damageDef);
    }

    private void AttackAnimation() {
        Creature creature = self.GetComponent<Creature>();
        animationController = creature.animationController;

        if (animationController != null) {
            SimpleEventHelper simpleEventHelper = animationController.eventHelper;
            animationController.SetBoolState(AnimationState.ATTACK);
            simpleEventHelper.notifyAnimationEventTrigger += OnAttackTrigger;
            simpleEventHelper.notifyAnimationEnd += OnAnimationEnd;
        }
    }

    private void OnAttackTrigger(string name) {
        if (name == "Attack") {
            foreach (Creature to in toS) {
                BaseAttack(from, to);
            }
        }
    }

    private void OnAnimationEnd() {
        animationController.SetAllFalse();
        ActionEnd();
    }

}
