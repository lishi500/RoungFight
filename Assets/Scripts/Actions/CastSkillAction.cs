using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSkillAction : Action
{
    public Skill skill;
    public string skillName;
    public CastSkillAction(GameObject self) : base(self) { }
    public CastSkillAction(GameObject self, GameObject target) : base(self, target) { }
    public CastSkillAction(GameObject self, List<GameObject> targets) : base(self, targets) { }

    public GameObject skillObj;
    public Skill skillClone;

    protected override void OnPrepareAction() {
        if (skill == null && skillName == null) {
            ActionEnd();
            return;
        }

        GetOrInitSkillHolder();
        skillClone = skillObj.GetComponent<Skill>();

        skillClone.ownerObj = self;
        skillClone.sequenceId = GameManager.Instance.skillSeq;

        if (targets == null || targets.Count == 0) {
            if (skill.hasTargetController) {
                // TODO skill get target
            } else {
                targets = TargetHelper.Instance.SearchTargets(from.gameObject, skill.skillData.targetType);
            }
        }


        if (skillClone.skillData.IsMultiTarget) {
            skillClone.targetObjs = targets;
        } else {
            skillClone.targetObj = targets[0];
        }
    }

    protected override void OnStartAction() {
        SkillController skillController = skillObj.AddComponent<SkillController>();
        skillController.skill = skillClone;
        skillController.creator = self;
        skillController.primaryTarget = targets[0];
        skillController.targets = targets;
        skillController.notifySkillFinish += OnSkillEnd;
        skillController.action = this;
        Debug.Log("Listen " + skillClone.sequenceId);

        skillController.InitialSkill();
    }

    public void OnSkillEnd() {
        Debug.Log("On Skill End " + skillClone.sequenceId);
        SkillController skillController = skillObj.AddComponent<SkillController>();
        skillController.notifySkillFinish -= OnSkillEnd;
        ActionEnd();
    }

    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>() { ActionType.CastSkill };
    }

    public GameObject GetSkillPrefab() {
        if (skill != null) {
           return SkillHelper.Instance.GetSkillPrefab(skill);
        }
        return SkillHelper.Instance.GetSkillPrefab(skillName);
    }

    private void GetOrInitSkillHolder() {
        if (skill != null) {
            skillName = skill.skillName;
        }
        //skillObj = SkillHolderPool.Instance.DePool(skillName);
        if (skillObj == null) {
            GameObject skillPrefab = GetSkillPrefab();
            skillObj = GameObject.Instantiate(skillPrefab);
        } else {
            skillObj.SetActive(true);
        }
    }


}
