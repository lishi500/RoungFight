using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarWithShield : UIAttributeMeter
{
    Shield shield;
    // Start is called before the first frame update
    protected override void RefreshText() {
        float val = monitorAttribute.GetCalculatedValue();
        float max = monitorAttribute.maxValue;
        string healthTxt = val + "/" + max;
        string shieldTxt = shield != null && shield.amount > 0 ? " (" + Mathf.CeilToInt(shield.amount) + ")": "";

        text.text = healthTxt + shieldTxt;
    }
    private void OnShieldChange() {
        RefreshText();
    }

    protected override void Start()
    {
        base.Start();

        if (creature != null) {
            shield = creature.shield;
            shield.notifyShieldAdd += OnShieldChange;
            shield.notifyShieldReduce += OnShieldChange;
        }

    }
}
