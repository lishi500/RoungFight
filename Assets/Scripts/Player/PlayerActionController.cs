using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    public Player player {
        get { return GetComponent<Player>();  }
    }
    public PlayerParty playerParty {
        get { return BoardManager.Instance.playerParty; }
    }

    public void ChargeEnergy(Cat cat, int energy) {
        BoardManager.Instance.DisablePlayerAction();
        if (energy + cat.Energy.value < cat.Energy.maxValue) {
            cat.BaseAction();
        }

        ChargeEnergyAction chargeEnergy = new ChargeEnergyAction(null, cat.gameObject, Attribute.Of(energy));
        playerParty.actionChain.AddAction(chargeEnergy);

        // listen action chain end
    }
}
