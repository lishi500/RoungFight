using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIAttributeMeter : MonoBehaviour
{
    public Creature creature;
    public AttrType type;
    private Attribute monitorAttribute;
    private Text text;

    public void MonitorNewAttibute(Attribute attr) {
        UnMonitorAttibute();
        monitorAttribute = attr;
        MonitorAttibute();
    }

    public void UnMonitorAttibute() {
        monitorAttribute.notifyValueChange -= OnAttributeChange;
    }

    private void MonitorAttibute() { 
        monitorAttribute.notifyValueChange += OnAttributeChange;
    }

    private void OnAttributeChange(Attribute attr) {
        float val = attr.GetCalculatedValue();
        float max = attr.maxValue;
        text.text = val + "/" + max;
    }

    // Start is called before the first frame update
    void Start()
    {
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
