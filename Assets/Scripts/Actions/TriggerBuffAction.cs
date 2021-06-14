using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBuffAction : Action
{
    public BaseBuff baseBuff;

    public override void StartAction() {
        baseBuff.TriggerBuff();
        ActionEnd();
    }
}
