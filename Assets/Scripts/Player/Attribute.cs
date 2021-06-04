using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Attribute {
    public delegate void AttributeValueChangeEvent(Attribute attr);
    public event AttributeValueChangeEvent notifyValueChange;

    public AttrType type;
    [SerializeField]
    protected float m_value;
    public float value {
        get { return m_value; }
        set { SetValue(value); }
    }

    [SerializeField]
    protected float m_maxValue = 100;
    public float maxValue {
        get { return m_maxValue; }
        set { m_maxValue = value; NotifyChange(); }
    }

    [HideInInspector]
    public float minValue = 0;

    protected float m_adder = 0;
    [HideInInspector]
    public float adder {
        get { return m_adder; }
        set { m_adder = value; NotifyChange(); }
    }

    protected float m_modifier = 1;
    [HideInInspector]
    public float modifier {
        get { return m_modifier; }
        set { modifier = value; NotifyChange(); }
    }

    [HideInInspector]
    protected float m_weight = 1;
    public float weight {
        get { return (m_weight + weightAdder) * weightModifier; }
        set { m_weight = value; }
    }
    [HideInInspector]
    public float weightAdder = 0;
    [HideInInspector]
    public float weightModifier = 1;

    public bool isInverseCalc = false;

    public void SetValue(float v) {
        m_value = Mathf.Clamp(v, minValue, maxValue);
        NotifyChange();
    }

    public float GetCalculatedValue() {
        return (value + adder) * modifier;
    }

    public void AddValue(float add) {
        float newValue = m_value + add;

        if (type == AttrType.Energy) {
            Debug.Log("Add energy from " + value + " to " + newValue);
        }
        SetValue(newValue);
    }

    public void SubValue(float sub) {
        float newValue = m_value - sub;
        SetValue(newValue);
    }


    public float GetWeightedProcessedValue() {
        return weight * GetCalculatedValue();
    }

    public static Attribute Of(float val, AttrType attrType = AttrType.None) {
        Attribute attr = new Attribute();
        attr.value = val;
        attr.type = attrType;
        return attr;
    }

    private void NotifyChange() {
        if (notifyValueChange != null) {
            notifyValueChange(this);
        }
    }

    public override string ToString() {
        return type + ":" + value + " MaxValue:" + maxValue + " Modifier:" + modifier + " Adder:" + adder + " weight:" + weight;
    }
}
