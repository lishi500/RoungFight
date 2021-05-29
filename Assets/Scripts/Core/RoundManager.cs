using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : Singleton<RoundManager> {
    public RoundPartySchedule PartySchedule {
        get { return m_roundParty; }
    }
    public Party currentParty;
    public int RoundNumber;

    private RoundPartySchedule m_roundParty;

    public void MoveToNextRountParty() {
        PartyType partyType = PartySchedule.GetNextParty();
        Debug.Log("current party type:"+partyType);

        switch (partyType) {
            case PartyType.Player:
                currentParty = BoardManager.Instance.playerParty;
                break;
            case PartyType.Enemy:
                currentParty = BoardManager.Instance.enemyParty;
                break;
        }
        currentParty.StartRound();
    }

    protected override void Awake() {
        base.Awake();
        m_roundParty = new RoundPartySchedule();
    }
}
