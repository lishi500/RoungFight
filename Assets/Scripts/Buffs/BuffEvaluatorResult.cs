using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffEvaluatorResult
{
    public List<BaseBuff> buffEvaluated;
    public Action action;
    public ActionType actionType;
    //GenericAttribute tempAttribute;
    public bool continueEvalute = true; // keep evalute next buff
    public bool denialAction = false; // if denial action, caller action will be cancelled

    //List<BaseBuff> attachBuffToSelf;
    //List<BaseBuff> attachBuffToCollisonTarget;

    public void Init()
    {
        buffEvaluated = new List<BaseBuff>();
        //tempAttribute = new GenericAttribute();
        //attachBuffToSelf = new List<BaseBuff>();
        //attachBuffToCollisonTarget = new List<BaseBuff>();
    }
}