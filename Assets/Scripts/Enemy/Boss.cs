using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    public override void OnDie() {
        // level complete
    }

    public override void BaseAction() {
        Action baseAttack = new BaseAttackAction(transform.gameObject, playerParty.player.gameObject);
        enemyParty.actionChain.AddAction(baseAttack);
    }
}
