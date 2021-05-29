using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : Singleton<DamageTextPool>
{
    List<DamageUp> damageUpPool;

    public void PopDamage(GameObject obj, DamageDef damageDef) {
        DamageUp damageUp = GetAvailableDamageUpFromPool();
        if (damageUp == null) {
            damageUp = AddOneToPool();
        }

        damageUp.SetDamageUp(obj, damageDef);
    }

    private DamageUp GetAvailableDamageUpFromPool() {
        foreach (DamageUp damageUp in damageUpPool) {
            if (damageUp.isAvailable) {
                return damageUp;
            }
        }

        return null;
    }

    private DamageUp AddOneToPool() {
        GameObject damageUpPrefabs = CommonUtil.Instance.GetPrefabByName("DamageUp");
        GameObject newDamageUp = Instantiate(damageUpPrefabs, this.transform);
        damageUpPool.Add(newDamageUp.GetComponent<DamageUp>());

        return newDamageUp.GetComponent<DamageUp>();
    }

    protected override void Awake() {
        base.Awake();
        damageUpPool = new List<DamageUp>();
        DamageUp[] existing = GetComponentsInChildren<DamageUp>();
        damageUpPool.AddRange(existing);
    }

}
