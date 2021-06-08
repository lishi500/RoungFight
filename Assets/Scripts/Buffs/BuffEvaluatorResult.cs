﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffEvaluatorResult
{
    List<BaseBuff> buffEvaluated;
    //GenericAttribute tempAttribute;
    bool continueEvalute = true; // keep evalute next buff
    bool denialAction = false; // if denial action, caller action will be cancelled

    List<BaseBuff> attachBuffToSelf;
    List<BaseBuff> attachBuffToCollisonTarget;

    public void Init()
    {
        buffEvaluated = new List<BaseBuff>();
        //tempAttribute = new GenericAttribute();
        attachBuffToSelf = new List<BaseBuff>();
        attachBuffToCollisonTarget = new List<BaseBuff>();
    }
}