using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleStrikeSkill : Skill
{
    // SkillType.Active
    public override void OnSkillCast() {
        //Debug.Log("DoubleStrikeSkill OnSkillCast");
        StartCoroutine(SkillProgress());
    }
    protected override IEnumerator SkillProgress() {
        yield return new WaitForSeconds(0.1f);
        DamageDef damageDef0 = QuickDamage(target, 0);
        Debug.Log(name + ":" + sequenceId + "Phase 0" +  "Damage: " + damageDef0.damage + " : " + Time.time);


        yield return new WaitForSeconds(0.2f);
        DamageDef damageDef1 = QuickDamage(target, 1);
        Debug.Log(name + ":" + sequenceId + " Phase: 1" + "Damage: " + damageDef1.damage + " : " + Time.time);

        skillController.OnSkillFinish();
    }

    public override void UpdateEffect() {}

    public override void OnSkillAd() { }

    public override void SkillSetup() {
    }

}
