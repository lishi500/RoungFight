using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDef {
    public DamageDef(float damage, bool isCritical, DamageType type) {
        this.damage = damage;
        this.isCritical = isCritical;
        this.type = type;
    }
    public float damage;
    public bool isCritical;
    public DamageType type;
}
