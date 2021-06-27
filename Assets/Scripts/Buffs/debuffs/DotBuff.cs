using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotBuff : BaseBuff
{
    public float damageSnapshot;
    public override bool CanApplyTo(Creature creature) {
        if (creature.GetType() != typeof(Cat)) { 
            return true;
        }
        return false;
    }

    public override void OnBuffApply() {
        damageSnapshot = CalculatValue();
    }

    public override BuffEvaluatorResult OnBuffEvaluated(BuffEvaluatorResult evaluatorResult) {
        return evaluatorResult ;
    }

    public override void OnBuffRemove() {
    }

    public override void OnBuffTrigger() {
        StartCoroutine(TriggerWithDeplay());
    }

    public override void OnReactionTrigger(Action action) {
        throw new System.NotImplementedException();
    }

    private IEnumerator TriggerWithDeplay() {
        yield return new WaitForSeconds(0.2f);
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(damageSnapshot, caster, holder, damageType);
        holder.ReduceHealth(damageDef);
        TriggerEnd();
    }
}
