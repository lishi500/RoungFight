using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    [HideInInspector]
    public Attribute Health {
        get { return GetAttr(AttrType.Health); }
    }

    public override void OnDie() {
        // game failed
    }

    void Start()
    {
        foreach (Attribute attr in attributes) {
            Debug.Log(attr.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
