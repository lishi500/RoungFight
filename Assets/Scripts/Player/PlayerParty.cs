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

    public void AddCat(Cat cat) {
        cats.Add(cat);
        cat.notifyCatActionEnd += ListenCatActionEnd;
    }

    // Temp
    public void OnPlayerActionEnd() {
        OnRoundEnd();
    }

    public void ListenCatActionEnd(Cat cat) {
        OnPlayerActionEnd();
    }

    private void EnablePlayerAction() {
        BoardManager.Instance.EnablePlayerAction();
    }

    protected override void Awake() {
        base.Awake();
        player = GetComponentInChildren<Player>();
        cats = GetComponentsInChildren<Cat>().ToList();
        foreach (Cat cat in cats) {
            cat.notifyCatActionEnd += ListenCatActionEnd;
        }
    }
}
