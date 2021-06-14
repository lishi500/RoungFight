using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SkillAttachedBuff
{
    public GameObject buffObj;
    public TargetType targetType;

    [HideInInspector]
    public Skill skill;
    //public bool overrideExisting; // use this buff defination to override buff prafab value
    //public float duration;
    //public float frequency;

    //public float value;
    //public float factor;

}
