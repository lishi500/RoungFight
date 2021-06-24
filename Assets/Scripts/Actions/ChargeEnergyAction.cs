using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnergyAction : Action
{
    public ChargeEnergyAction(GameObject self, GameObject target, Attribute actionAttr = null) : base(self, target, actionAttr) { }

    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>() { ActionType.Charge };
    }

    protected override void OnPrepareAction() {
    }

    protected override void OnStartAction() {
        Cat cat = targets[0].GetComponent<Cat>();
        if (cat != null && actionAttr != null) {
            float chargeAmount = actionAttr.GetCalculatedValue();
            //Debug.Log("Charge energy Action: " + cat.name + " : " + actionAttr.GetCalculatedValue());
            if (cat.status.CanAction) {
                cat.ChargeEnergy(chargeAmount);
            } else {
                // If cat cannot action, cannot charge to full amount
                float gap = (cat.Energy.maxValue - cat.Energy.value);
                float minCharge = Mathf.Min(chargeAmount, gap - 1);
                cat.ChargeEnergy(minCharge);
            }
        }
        ActionEnd();
    }
}
