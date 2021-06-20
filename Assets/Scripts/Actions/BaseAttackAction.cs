using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackAction : Action
{
    CustomAnimationController animationController;

    public BaseAttackAction(GameObject self, GameObject target) : base(self, target) {}
    public BaseAttackAction(GameObject self, List<GameObject> targets) : base(self, targets) {}

    protected override void OnStartAction() {
        Debug.Log("Base Action start animation");
        AttackAnimation();
    }

    public void BaseAttack(Creature from, Creature to) {
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(from.GetAttrVal(AttrType.Attack), from, to, DamageType.NORMAL);
        Debug.Log("Base Action Attack with damage: " + damageDef.damage); ;
        to.ReduceHealth(damageDef);
    }

    private void AttackAnimation() {
        Creature creature = self.GetComponent<Creature>();
        animationController = creature.animationController;

        if (animationController != null) {
            SimpleEventHelper simpleEventHelper = animationController.eventHelper;
            animationController.SetBoolState(AnimationState.ATTACK, true, true);
            simpleEventHelper.notifyAnimationEventTrigger += OnAttackTrigger;
            simpleEventHelper.notifyAnimationEnd += OnAnimationEnd;
        }
    }

    private void OnAttackTrigger(string name) {
        Debug.Log("Base Action Receive attack trigger");

        if (name == "Attack") {
            animationController.eventHelper.notifyAnimationEventTrigger -= OnAttackTrigger;
            foreach (Creature to in toS) {
                BaseAttack(from, to);
            }
        }
    }

    private void OnAnimationEnd() {
        animationController.eventHelper.notifyAnimationEnd -= OnAnimationEnd;
        animationController.SetAllFalse();
        ActionEnd();
    }

    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>() { ActionType.BaseAttack };
    }

    protected override void OnPrepareAction() {
    }
}
