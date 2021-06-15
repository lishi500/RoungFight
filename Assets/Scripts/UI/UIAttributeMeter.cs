using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIAttributeMeter : MonoBehaviour
{
    public Creature creature;
    public AttrType type;
    protected Attribute monitorAttribute;
    protected Text text;

    public void MonitorNewAttibute(Attribute attr) {
        UnMonitorAttibute();
        monitorAttribute = attr;
        MonitorAttibute();
    }

    public void UnMonitorAttibute() {
        monitorAttribute.notifyValueChange -= OnAttributeChange;
    }

    protected virtual void RefreshText() {
        float val = monitorAttribute.GetCalculatedValue();
        float max = monitorAttribute.maxValue;
        text.text = val + "/" + max;
    }

    private void MonitorAttibute() { 
        monitorAttribute.notifyValueChange += OnAttributeChange;
    }

    private void OnAttributeChange(Attribute attr) {
        RefreshText();
    }

    protected virtual void Start() {
        text = GetComponent<Text>();
        if (creature != null) {
            monitorAttribute = creature.GetAttr(type);
        }
        if (monitorAttribute != null) {
            MonitorAttibute();
            OnAttributeChange(monitorAttribute);
        }
    }
}
