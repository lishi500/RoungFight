using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    [HideInInspector]
    public PlayerActionController playerActionController;

    [HideInInspector]
    public Attribute Health {
        get { return GetAttr(AttrType.Health); }
    }

    public override void BaseAction() {
    }

    public override void OnDie() {
        // game failed
    }

    protected override void Awake() {
        base.Awake();
        playerActionController = transform.GetComponent<PlayerActionController>();
    }

}
