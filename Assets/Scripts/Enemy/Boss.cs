using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    public override void OnDie() {
        // level complete
    }

    public virtual void StartRoundAction() {
        List<Skill> readySkills = GetReadySkills();
        if (readySkills.Count == 0 || !status.CanCastSkill) {
            Debug.Log("Boss base Action");
            BaseAction();
            return;
        }

        Debug.Log("Boss base Action ready " + readySkills.Count);
        foreach (Skill skill in readySkills) {
            Debug.Log("Boss Cast skill " + skill.skillName);
            CastSkillAction castSkillAction = new CastSkillAction(gameObject);
            castSkillAction.skill = skill;
            enemyParty.actionChain.AddAction(castSkillAction);
        }
    }

    private List<Skill> GetReadySkills() {
        List<Skill> readySkills = new List<Skill>();
        if (primarySkill != null && primarySkill.IsReady) {
            readySkills.Add(primarySkill);
        }
        // TODO consider two skill both ready
        if (secondarySkill != null && secondarySkill.IsReady) {
            readySkills.Add(secondarySkill);
        }

        if (skillList != null) {
            foreach (Skill skill in skillList) {
                if (skill != null && skill.IsReady) {
                    readySkills.Add(skill);
                }
            }
        }
        return readySkills;
    }

    public override void BaseAction() {
        Action baseAttack = new BaseAttackAction(transform.gameObject, playerParty.player.gameObject);
        enemyParty.actionChain.AddAction(baseAttack);
    }
}
