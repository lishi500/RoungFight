using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActoin : Action
{
    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>() { ActionType.Damage };

    }

    protected override void OnPrepareAction() {
    }

    protected override void OnStartAction() {
    }

   
}
