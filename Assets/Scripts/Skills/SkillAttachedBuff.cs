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
}
