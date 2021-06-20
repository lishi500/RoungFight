using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : Action {
    public float idleTime;
    public ActionChain actionChain;

    public IdleAction(ActionChain actionChain, float idleTime = 0) {
        this.actionChain = actionChain;
        this.idleTime = idleTime;
    }

    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>();
    }

    protected override void OnPrepareAction() {
    }

    protected override void OnStartAction() {
        if (idleTime > 0 && actionChain != null) {
            PlaceHolderAction placeHolderAction = new PlaceHolderAction();
            actionChain.AddActionJumpQueue(placeHolderAction, idleTime);
        }
        ActionEnd();
    }

}
