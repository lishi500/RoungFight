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

        skill.ownerObj = self;

        if (skill.skillData.IsMultiTarget) {
            skill.targetObjs = targets;
        } else {
            skill.targetObj = targets[0];
        }


        SkillController skillController = self.AddComponent<SkillController>();
        skillController.skill = skill;
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
