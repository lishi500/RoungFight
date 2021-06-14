using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty : Party
{
    public List<Cat> cats;
    public Player player;
    private bool hasPreStateChain;
    
    public override void StartRound() {
        //Debug.Log("player party start");
        OnRoundStart();
        StartCoroutine(SlowStart());
    }
    public IEnumerator SlowStart() {
        yield return new WaitForSeconds(0.1f);
        if (actionChain.IsAllTaskCompleted()) {
            EnablePlayerAction();
        } else {
            hasPreStateChain = true;
        }
    }
    public override void OnActionChainEnd() {
        if (hasPreStateChain) {
            hasPreStateChain = false;
            EnablePlayerAction();
        } else {
            if (boardManager.IsPlayerRound() && boardManager.IsPlayerAlreadyActioned()) {
                //Debug.Log("Player Round end");
                OnRoundEnd();
            }
        }
    }
   
    private void EnablePlayerAction() {
        boardManager.EnablePlayerAction();
    }

    protected override void Awake() {
        base.Awake();
        player = GetComponentInChildren<Player>();
        cats = GetComponentsInChildren<Cat>().ToList();
    }
}
