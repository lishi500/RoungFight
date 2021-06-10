using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotBuff : BaseBuff
{
    public override bool CanApplyTo(Creature creature) {
        if (creature.GetType() != typeof(Cat)) { 
            return true;
        }
        return false;
    }

    public override void OnBuffApply() {
    }

    public override BuffEvaluatorResult OnBuffEvaluated(BuffEvaluatorResult evaluatorResult) {
        throw new System.NotImplementedException();
    }

    public override void OnBuffRemove() {
        throw new System.NotImplementedException();
    }

    public override void OnBuffTrigger() {
        throw new System.NotImplementedException();
    }
   
}
