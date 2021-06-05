using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnergyAction : Action
{
    public ChargeEnergyAction(GameObject self, GameObject target, Attribute actionAttr = null) : base(self, target, actionAttr) { }

    public override void StartAction() {
        Cat cat = targets[0].GetComponent<Cat>();
        if (cat != null && actionAttr != null) {
            //Debug.Log("Charge energy Action: " + cat.name + " : " + actionAttr.GetCalculatedValue());
            cat.ChargeEnergy(actionAttr.GetCalculatedValue());
        }
        ActionEnd();
    }
}
