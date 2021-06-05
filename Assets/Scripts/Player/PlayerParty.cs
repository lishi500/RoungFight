using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty : Party
{
    public List<Cat> cats;
    public Player player;

    public override void StartRound() {
        Debug.Log("player party start");
        OnRoundStart();
        EnablePlayerAction();
    }

    public override void OnActionChainEnd() {
        if (boardManager.IsPlayerRound() && boardManager.IsPlayerAlreadyActioned()) {
            Debug.Log("Player Round end");
            OnRoundEnd();
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
