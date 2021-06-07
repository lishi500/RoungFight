using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSkillAction : Action
{
    public Skill skill;

    public CastSkillAction(GameObject self, GameObject target) : base(self, target) { }
    public CastSkillAction(GameObject self, List<GameObject> targets) : base(self, targets) { }

    public override void StartAction() {
        if (skill == null) {
            ActionEnd();
            return;
        }
        // TODO make a skill holder pool

        GameObject skillPrefab = SkillHelper.Instance.GetSkillPrefab(skill);
        GameObject skillObjClone = GameObject.Instantiate(skillPrefab);
        Skill skillClone = skillObjClone.GetComponent<Skill>();

        skillClone.ownerObj = self;

        if (skillClone.skillData.IsMultiTarget) {
            skillClone.targetObjs = targets;
        } else {
            skillClone.targetObj = targets[0];
        }

        SkillController skillController = skillObjClone.AddComponent<SkillController>();
        skillController.skill = skillClone;
        skillController.creator = self;
        skillController.primaryTarget = targets[0];
        skillController.targets = targets;
        skillController.notifySkillFinish += OnSkillEnd;

        skillController.InitialSkill();
    }

    public void OnSkillEnd() {
        ActionEnd();
    }
}
