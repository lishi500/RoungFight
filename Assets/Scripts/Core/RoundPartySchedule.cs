using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoundPartySchedule
{
    private int m_roundNumber = 0;
    public int RoundNumber {
        get { return m_roundNumber; }
    }
    public RoundPartySchedule() {
        m_partyQueue = new LinkedList<PartyStruct>();
    }
    private PartyStruct m_current;
    public PartyType Current {
        get { return m_current.party; }
    }
    private LinkedList<PartyStruct> m_partyQueue;
    private int sequenceId = 0;

    public PartyType GetNextParty() {
        if (m_partyQueue.Count > 0) {
            PartyStruct nextParty = m_partyQueue.First.Value;
            m_partyQueue.RemoveFirst();
            m_current = nextParty;

            m_partyQueue.AddLast(m_current);
            if (nextParty.id == 0) {
                m_roundNumber++;
            }
            return nextParty.party;
        } else {
            Debug.LogError("Cannot find Next party");
            return PartyType.None;
        }
    }

    public void InsertToNextParty(PartyType partyType, bool isRepeat = false, int id = -1) {
        PartyStruct party = new PartyStruct();
        party.party = partyType;
        party.isRepeat = isRepeat;
        party.id = id == -1 ? sequenceId++ : id;

        m_partyQueue.AddFirst(party);
    }

    public void KickToLast() {
        if (m_partyQueue.Count > 0) {
            PartyStruct nextParty = m_partyQueue.First.Value;
            m_partyQueue.RemoveFirst();
            m_partyQueue.AddLast(nextParty);
        }
    }

    public void SetRoundNumber(int number) {
        m_roundNumber = number;
    }

    private void RepeatToLast(PartyStruct party) {
        if (party != null && party.isRepeat) {
            m_partyQueue.AddLast(party);
        }
    }

    class PartyStruct {
        public PartyType party;
        public bool isRepeat;
        public int id;
    }
}
