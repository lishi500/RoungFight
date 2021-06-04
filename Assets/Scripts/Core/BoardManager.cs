using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager> {
    private bool m_isPlayerActionEnabled;
    public bool IsPlayerActionEnabled {
        get { return m_isPlayerActionEnabled; }
    }

    public PlayerParty playerParty {
        get { return GetCurrentLevel().playerParty; }
    }
    public EnemyParty enemyParty { 
        get { return GetCurrentLevel().enemyParty; }
    }

    private LevelBase m_level;
   
    public void EnablePlayerAction() {
        m_isPlayerActionEnabled = true;
    }
    public void DisablePlayerAction() {
        m_isPlayerActionEnabled = false;
    }


    public LevelBase GetCurrentLevel() {
        if (m_level == null) {
            m_level = this.transform.gameObject.GetComponent<LevelBase>();
        }
        return m_level;
    }

    public bool IsPlayerRound() {
        return RoundManager.Instance.currentParty.partyType == PartyType.Player;
    }

    public bool IsPlayerAlreadyActioned() {
        return IsPlayerRound() && !IsPlayerActionEnabled;
    }

    // Start is called before the first frame update
    void Start()
    {
        RoundManager.Instance.MoveToNextRountParty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
