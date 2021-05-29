using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelBase : MonoBehaviour
{
    public int LevelNumber;
    public PlayerParty playerParty;
    public EnemyParty enemyParty;

    public virtual void InitRoundParty() {
        // TODO Hard code for now, dynamic later
        //playerParty = transform.gameObject.AddComponent<PlayerParty>();
        //enemyParty = transform.gameObject.AddComponent<EnemyParty>();

        RoundPartySchedule partySchedule = RoundManager.Instance.PartySchedule;
        partySchedule.InsertToNextParty(PartyType.Enemy, true);
        partySchedule.InsertToNextParty(PartyType.Player, true);
    }

    public void Awake() {
        InitRoundParty();
    }
}
