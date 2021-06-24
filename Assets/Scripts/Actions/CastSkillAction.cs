using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSkillAction : Action
{
    public Skill skill;
    public string skillTypeName;
    public CastSkillAction(GameObject self) : base(self) { }
    public CastSkillAction(GameObject self, GameObject target) : base(self, target) { }
    public CastSkillAction(GameObject self, List<GameObject> targets) : base(self, targets) { }

    public GameObject skillObj;
    public Skill skillClone;
    SkillController skillController;

    protected override void OnPrepareAction() {
        if (skill == null && skillTypeName == null) {
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
            skillClone.targetObjs = new List<GameObject>();
            skillClone.targetObjs.Add(targets[0]);
        }
    }

    protected override void OnStartAction() {

        skillController.skill = skillClone;
        skillController.creator = self;
        skillController.primaryTarget = targets[0];
        skillController.targets = targets;
        skillController.notifySkillFinish += OnSkillEnd;
        skillController.action = this;

        skillController.InitialSkill();
    }

    public void OnSkillEnd() {
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
        return SkillHelper.Instance.GetSkillPrefab(skillTypeName);
    }

    private void GetOrInitSkillHolder() {
        if (skill != null) {
            skillTypeName = skill.GetType().ToString();
        }

        skillObj = SkillHolderPool.Instance.DePool(skillTypeName);

        if (skillObj == null) {
            GameObject skillPrefab = GetSkillPrefab();
            skillObj = GameObject.Instantiate(skillPrefab);
            skillController = skillObj.AddComponent<SkillController>();
        } else {
            skillObj.SetActive(true);
            skillController = skillObj.GetComponent<SkillController>();
        }
    }


}
