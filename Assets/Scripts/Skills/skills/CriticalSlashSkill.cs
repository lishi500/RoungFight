using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalSlashSkill : Skill
{
    public override void OnSkillAd() {
        throw new System.NotImplementedException();
    }

    public override void OnSkillCast() {
        StartCoroutine(SkillProgress());
    }

    protected override IEnumerator SkillProgress() {
        yield return new WaitForSeconds(0.1f);
        float baseDamage = CalculateValue(0);
        float targetArmor = target.GetAttrVal(AttrType.Defence);
        float targetArmorDefence = DamageHelper.Instance.CalculateArmorDefence(targetArmor);
        
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(baseDamage, 100f, factors[0].damageType, targetArmorDefence);
        target.ReduceHealth(damageDef);
    }

    public override void SkillSetup() {
        LoadTargetCreature();
    }

    public override void UpdateEffect() {
    }
}
