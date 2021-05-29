using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creeps : EnemyBase
{
    public override void OnDie() {
        // no action, or send event enemy die
    }

    public void Action() {
        Creature target = BoardManager.Instance.playerParty.player;
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(GetAttrVal(AttrType.Attack), this, target, DamageType.NORMAL);
        target.ReduceHealth(damageDef);
        ActionFinished();
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
