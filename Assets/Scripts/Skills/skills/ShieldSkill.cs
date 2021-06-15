using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : Skill
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
        yield return new WaitForSeconds(0.5f);

        float shieldAmount = CalculateValue();
        DamageDef shieldDef = new DamageDef(shieldAmount, skillData.CanDuplicate, DamageType.SHIELD);
        target.AddShield(shieldDef, 5, true, this.GetType());

        skillController.OnSkillFinish();
    }

}
