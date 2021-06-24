using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunDebuff : BaseBuff
{
    public override bool CanApplyTo(Creature creature) {
        return true;
    }

    public override void OnBuffApply() {
        holder.status.AddStatus(seqId, StatusType.Stun);
    }

    public override BuffEvaluatorResult OnBuffEvaluated(BuffEvaluatorResult evaluatorResult) {
        return evaluatorResult;
    }

    public override void OnBuffRemove() {
        holder.status.RemoveStatusById(seqId);
    }

    public override void OnBuffTrigger() {
    }

    public override void OnReactionTrigger(Action action) {
    }
}
