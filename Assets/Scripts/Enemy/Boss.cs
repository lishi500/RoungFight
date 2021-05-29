using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    public override void OnDie() {
        // level complete
    }

    public void Action() {
        Creature target = BoardManager.Instance.playerParty.player;
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(GetAttrVal(AttrType.Attack), this, target, DamageType.NORMAL);
        target.ReduceHealth(damageDef);
        ActionFinished();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
