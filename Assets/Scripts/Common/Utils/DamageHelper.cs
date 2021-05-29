using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHelper : Singleton<DamageHelper>
{
    public float CalculateArmorDefence(float armor) {
        if (armor >= 0) {
            return 100 / (100 + armor);
        } else {
            return 2 - (100 / (100 - armor));
        }
    }

    public DamageDef CalculateDamage(float baseDamage, Creature from, Creature to, DamageType damageType) {
        float criticalChange = from.GetAttrVal(AttrType.Critical);
        float targetArmor = to.GetAttrVal(AttrType.Defence);
        float targetArmorDefence = CalculateArmorDefence(targetArmor);
        Debug.Log(to.name + " Armor:" + targetArmor + " Reduce:" + targetArmorDefence + " base damage: " + baseDamage);
        // TODO extra damage reduction

        return CalculateDamage(baseDamage, criticalChange, damageType, targetArmorDefence);
    }

    public DamageDef CalculateDamage(float baseDamage, float criticalChance, DamageType damageType, float armorDefence) {
        float damage = baseDamage * armorDefence;
        DamageDef damageDef = new DamageDef(damage, false, damageType);

        return CriticalProcess(damageDef, criticalChance);
    }

    private DamageDef CriticalProcess(DamageDef damageDef, float criticalChance) {
        if (isCritical(criticalChance)) {
            damageDef.isCritical = true;
            damageDef.damage = damageDef.damage * 2;
        }
        damageDef.damage = Mathf.Round(damageDef.damage);
        Debug.Log("Deal damage: " + damageDef.damage + " isCritical:" + damageDef.isCritical);
        return damageDef;
    }

    private bool isCritical(float chance) {
        return chance/100f >= Random.Range(0, 1f);
    }
}
