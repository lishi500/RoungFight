using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleStrikeSkill : Skill
{
    // SkillType.Active
    Creature target;
    public override void OnSkillCast() {
        Debug.Log("DoubleStrikeSkill OnSkillCast");
        StartCoroutine(SkillProgress());
    }
    IEnumerator SkillProgress() {
        yield return new WaitForSeconds(0.1f);
        QuickDamage(target, 0);

        yield return new WaitForSeconds(0.3f);
        QuickDamage(target, 1);
    }

    public override void UpdateEffect() {}

    public override void OnSkillAd() { }

    public override void SkillSetup() { 
        target = targetObj.GetComponent<Creature>();
    }

}
