using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    public void ChargeEnergyAction(Cat cat, int energy) {
        BoardManager.Instance.DisablePlayerAction();
        if (energy + cat.Energy.value > cat.Energy.maxValue) {
            cat.ChargeEnergy(energy);
            // use skill
        } else {
            // stanard attack
            cat.BaseAction();
        }

        // listen action chain end
    }

    public void OnActionChainEnd() {
        RoundManager.Instance.MoveToNextRountParty();
    }
}
