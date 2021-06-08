using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeBuff : BaseBuff
{
    [Header("AttributeBuff")]
    public AttrType attriType;
    public AttriValueType attriValueType;
    //private float originalValueSnap;
    // Attribute Buff will use static value, no calculation and factor required
    public override void OnBuffApply()
    {
        Debug.Log("OnBuffApply " + buffName);

        if (!holder.HasAttr(attriType)) {
            return;
        }
        Attribute attribute = holder.GetAttr(attriType);

        switch (attriValueType) {
            case AttriValueType.Value:
                attribute.AddValue(value);
                break;
            case AttriValueType.Adder:
                attribute.adder = attribute.adder + value;
                break;
            case AttriValueType.Modifier:
                attribute.modifier = attribute.modifier + value;
                break;
            case AttriValueType.MaxValue:
                attribute.maxValue = attribute.maxValue + value;
                break;
            //case AttriValueType.MinValue:
            //    attribute.minValue = attribute.minValue + value;
            //break;
            //case AttriValueType.Weight:
            //    break;
            //case AttriValueType.WeightAdder:
            //    break;
            //case AttriValueType.WeightModifier:
            //    break;
            default:
                break;
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("OnBuffRemove " + buffName);

        if (!holder.HasAttr(attriType)) {
            return;
        }
        Attribute attribute = holder.GetAttr(attriType);

        switch (attriValueType) {
            case AttriValueType.Value:
                attribute.SubValue(value);
                break;
            case AttriValueType.Adder:
                attribute.adder = attribute.adder - value;
                break;
            case AttriValueType.Modifier:
                attribute.modifier = attribute.modifier - value;
                break;
            case AttriValueType.MaxValue:
                attribute.maxValue = attribute.maxValue - value;
                break;
            default:
                break;
        }
    }

    public override bool CanApplyTo(Creature creature) {
        throw new System.NotImplementedException();
    }

    public override BuffEvaluatorResult OnBuffEvaluated(BuffEvaluatorResult evaluatorResult)
    {
        return evaluatorResult;
    }

    public override void OnBuffTrigger()
    {
    }

   
}
