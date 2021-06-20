using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Not suppose to be used in any other class except combine with IdleAction to achieve idle time
 */
public class PlaceHolderAction : Action
{
    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>();
    }

    protected override void OnPrepareAction() {
    }

    protected override void OnStartAction() {
        ActionEnd();
    }
}
