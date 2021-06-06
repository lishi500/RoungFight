using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectChain: BaseEffect
{
    public PositionType positionType;
    public float xOffSet;
    public float yOffSet;
    public bool useRoleFacing = true;
    public bool followRole;
}
