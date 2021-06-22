using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAttackBuff : BaseBuff
{
    public GameObject flameBallPrefab;
    private GameObject flameBall;
    public override bool CanApplyTo(Creature creature) {
        return true;
    }

    public override void OnBuffApply() {
        flameBall = Instantiate(flameBallPrefab);
        MoveCycle moveCycle = flameBall.AddComponent<MoveCycle>();
        moveCycle.SetUp(holder.gameObject, 1f, true);
        moveCycle.isForever = true;
        moveCycle.distance = 1f;

        moveCycle.StartCycling();

        
    }

    public override BuffEvaluatorResult OnBuffEvaluated(BuffEvaluatorResult evaluatorResult) {
        return evaluatorResult;
    }

    public override void OnBuffRemove() {
    }

    public override void OnBuffTrigger() {
    }

    public override void OnReactionTrigger(Action action) {
        CastSkillAction castSkillAction = new CastSkillAction(holder.gameObject, action.targets);
        castSkillAction.skillTypeName = attachedSkillName;
        castSkillAction.PrepareAction();
        FireBallAttackSkill fireSkill = castSkillAction.skillObj.GetComponent<Skill>() as FireBallAttackSkill;
        fireSkill.flameEffect = flameBall;

        DelayStartHandler.Instance.DelayAction(castSkillAction, 0.3f);
    }

}
