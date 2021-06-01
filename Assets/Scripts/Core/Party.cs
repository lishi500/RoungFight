using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Party : MonoBehaviour
{
    public PartyType partyType;
    public delegate void PartRoundStartEvent(Party party);
    public event PartRoundStartEvent notifyPartyRoundStart;
    public delegate void PartRoundEndEvent(Party party);
    public event PartRoundEndEvent notifyPartyRoundEnd;
    public ActionChain actionChain;

    //public abstract void CanStartRound();
    public abstract void StartRound();

    public void AddAction(Action action) {
        actionChain.AddAction(action);
    }

    public void StartActionChain() {
        if (!actionChain.isStarted && !actionChain.isExecuting) {
            actionChain.StartActionChain();
        }
    }

    protected void OnRoundStart() {
        if (notifyPartyRoundStart != null) {
            notifyPartyRoundStart(this);
        }
    }

    protected void OnRoundEnd() {
        if (notifyPartyRoundEnd != null) {
            notifyPartyRoundEnd(this);
        }
        RoundManager.Instance.MoveToNextRountParty();
    }

    protected virtual void Awake() {
        actionChain = GetComponent<ActionChain>();
    }

}
