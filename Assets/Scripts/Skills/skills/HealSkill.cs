using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : Skill
{
    public override void OnSkillAd() {
        throw new System.NotImplementedException();
    }

    public override void OnSkillCast() {
        StartCoroutine(SkillProgress());
    }

    public override void SkillSetup() {
    }

    public override void UpdateEffect() {
    }

    protected override IEnumerator SkillProgress() {
        yield return new WaitForSeconds(0.2f);
        float amount = CalculateValue();
        DamageDef healDef = DamageHelper.Instance.CalculateDamage(amount, owner.GetAttrVal(AttrType.Critical), DamageType.HEAL);
        target.Heal(healDef);

        skillController.OnSkillFinish();
    }
}
