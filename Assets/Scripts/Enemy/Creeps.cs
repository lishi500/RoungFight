using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creeps : EnemyBase
{
    public override void OnDie() {
        // no action, or send event enemy die
    }

    public override void BaseAction() {
        Action baseAttack = new BaseAttackAction(transform.gameObject, playerParty.player.gameObject);
        enemyParty.actionChain.AddAction(baseAttack);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
