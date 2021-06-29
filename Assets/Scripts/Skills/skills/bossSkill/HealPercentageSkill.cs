using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPercentageSkill : Skill
{
    public override void OnSkillAd() {
    }

    public override void OnSkillCast() {
        StartCoroutine(SkillProgress());
    }

    public override void SkillSetup() {
    }

    public override void UpdateEffect() {

    }

    protected override IEnumerator SkillProgress() {
        yield return new WaitForSeconds(0.3f);

        float healPercentage = CalculateValue(0);
        float healAmount = healPercentage / 100f * target.health.maxValue;
        DamageDef healDef = DamageHelper.Instance.CalculateDamage(healAmount, owner.GetAttrVal(AttrType.Critical), DamageType.HEAL);

        target.Heal(healDef);

        skillController.OnSkillFinish();
    }


}
