using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Party : MonoBehaviour {
    public PartyType partyType;
    public delegate void PartRoundStartEvent(Party party);
    public event PartRoundStartEvent notifyPartyRoundStart;
    public delegate void PartRoundEndEvent(Party party);
    public event PartRoundEndEvent notifyPartyRoundEnd;
    public ActionChain actionChain;

    protected BoardManager boardManager {
        get { return BoardManager.Instance; }
    }

    protected RoundManager roundManager {
        get { return RoundManager.Instance; }
    }

    //public abstract void CanStartRound();
    public abstract void StartRound();
    public abstract void OnActionChainEnd();

    public void StartActionChain() {
        if (!actionChain.isStarted && !actionChain.isExecuting) {
            actionChain.Start();
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
        roundManager.MoveToNextRountParty();
    }

    protected void OnActionAdd(Action action) {
        StartActionChain();
    }


    protected virtual void Awake() {
        actionChain = GetComponent<ActionChain>();
        actionChain.notifyAddAction += OnActionAdd;
        actionChain.notifyActionChainEnd += OnActionChainEnd;
    }

}
