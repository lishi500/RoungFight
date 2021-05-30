using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    public List<Attribute> attributes;
    public bool isAlive = true;
    public Skill primarySkill;
    public Skill secondarySkill;
    public Creature target;

    public abstract void BaseAction();

    public Attribute health {
        get { return GetAttr(AttrType.Health); }
    }
    public Attribute shield {
        get { return GetAttr(AttrType.Shield); }
    }

    public abstract void OnDie();

    public Attribute GetAttr(AttrType type) {
        if (attributes != null) {
            return attributes.Where(attr => attr.type == type).First();
        }
        return null;
    }

    public float GetAttrVal(AttrType type) {
        Attribute attr = GetAttr(type);
        return GetAttrVal(attr);
    }

    public float GetAttrVal(Attribute attr) {
        if (attr != null) {
            return attr.GetCalculatedValue();
        }
        Debug.LogError("Attr " + attr.type + " Not exist in Object " + name);
        return 0;
    }

    public bool HasAttr(AttrType type) {
        return GetAttr(type) != null;
    }

    public void ReduceHealth(DamageDef damageDef) {
        float amount = damageDef.damage;
        bool isCritical = damageDef.isCritical;
        DamageType type = damageDef.type;

        if (isAlive && health != null) {
            if (GetAttrVal(shield) > 0) {
                float amountLeft = Mathf.Max(amount - GetAttrVal(shield), 0);
                shield.SubValue(amount);
                amount = amountLeft;
            }

            health.SubValue(amount);
            // TODO show hit damage
            DamageTextPool.Instance.PopDamage(this.gameObject, damageDef);
            // send hit event

            if (health.GetCalculatedValue() <= 0) {
                OnDie();
            }
            Debug.Log("Health Left:" + health.GetCalculatedValue());
        }
    }

}
