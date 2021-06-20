using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Party : MonoBehaviour {
    public PartyType partyType;
    public delegate void PartRoundStartEvent();
    public event PartRoundStartEvent notifyPartyRoundStart;
    public delegate void PartRoundEndEvent();
    public event PartRoundEndEvent notifyPartyRoundEnd;
    public ActionChain actionChain;

    public GameObject roundState;

    protected BoardManager boardManager {
        get { return BoardManager.Instance; }
    }

    protected RoundManager roundManager {
        get { return RoundManager.Instance; }
    }

    //public abstract void CanStartRound();
    public abstract void StartRound();
    public abstract void OnActionChainEnd();
    public abstract List<Creature> GetAllCreatures();

    public void StartActionChain() {
        if (!actionChain.isStarted && !actionChain.isExecuting) {
            actionChain.StartActionChain();
        }
    }

    protected void OnRoundStart() {
        if (notifyPartyRoundStart != null) {
            notifyPartyRoundStart();
        }
        roundState.SetActive(true);
    }

    protected void OnRoundEnd() {
        if (notifyPartyRoundEnd != null) {
            notifyPartyRoundEnd();
        }
        roundState.SetActive(false);
        roundManager.MoveToNextRountParty();
    }

    protected void OnActionAdd(Action action) {
        Debug.Log("Add action " + action.GetType().ToString());
        StartActionChain();
    }


    protected virtual void Awake() {
        actionChain = GetComponent<ActionChain>();
        actionChain.notifyAddAction += OnActionAdd;
        actionChain.notifyActionChainEnd += OnActionChainEnd;
    }

}
