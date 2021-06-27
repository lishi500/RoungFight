using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBuffAction : Action
{
    public BaseBuff baseBuff;

    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>() { ActionType.TriggerBuff };
    }

    protected override void OnPrepareAction() {
        //if (baseBuff != null) {
        //}
    }

    protected override void OnStartAction() {
        baseBuff.TriggerBuff();
        ActionEnd();
    }
}
