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

    private void Awake() {
        playerActionController = transform.GetComponent<PlayerActionController>();
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
