using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Creature : MonoBehaviour {
    public List<Attribute> attributes;
    public bool isAlive = true;
    public Skill primarySkill;
    public Skill secondarySkill;
    public List<Skill> skillList;
    public List<BaseBuff> buffs;
    public Creature target;
    public CustomAnimationController animationController;
    public Shield shield;

    [HideInInspector]
    public CreatureStatus status;
    public Party party {
        get { return GetComponentInParent<Party>(); }
    }
    public PartyType partyType {
        get { return party != null ? party.partyType : PartyType.None; }
    }
    public PlayerParty playerParty {
        get { return BoardManager.Instance.playerParty;  }
    }
    public EnemyParty enemyParty { 
        get { return BoardManager.Instance.enemyParty;  }
    }

    public abstract void BaseAction();

    public Attribute health {
        get { return GetAttr(AttrType.Health); }
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
        if (isAlive && health != null) {
            if (shield.amount > 0) {
                shield.ReduceShield(damageDef);
            }

            health.SubValue(damageDef.damage);
            if (damageDef.damage > 0) {
                ShowGetHitAnimation();
            }
            DamageTextPool.Instance.PopDamage(this.gameObject, damageDef);
            // send hit event

            if (health.GetCalculatedValue() <= 0) {
                OnDie();
            }
        }
    }

    public void Heal(DamageDef damageDef) {
        if (isAlive && damageDef.type == DamageType.HEAL) {
            health.AddValue(damageDef.damage);
            // event on heal
            DamageTextPool.Instance.PopDamage(this.gameObject, damageDef);
        }
    }

    public void AddShield(DamageDef shieldDef, int roundToLive, bool canStack, Type classType) {
        if (isAlive && shieldDef.type == DamageType.SHIELD) {
            shield.AddShield(shieldDef.damage, roundToLive, canStack, classType);
            // event on heal
            DamageTextPool.Instance.PopDamage(this.gameObject, shieldDef);
        }
    }

    public void AddBuff(BaseBuff buff) {
        buffs.Add(buff);
    }

    public void RemoveBuff(BaseBuff buff) {
        if (buffs.Contains(buff)) {
            buffs.Remove(buff);
        }
    }
    protected virtual void ShowGetHitAnimation() {
        animationController.SetBoolState(AnimationState.GET_HIT);
        animationController.eventHelper.notifyAnimationEnd += OnHitAnimationEnd;
    }

    private void OnHitAnimationEnd() {
        animationController.SetAllFalse();
        animationController.eventHelper.notifyAnimationEnd -= OnHitAnimationEnd;
    }

    private void LoadSkill() {
        Transform primarySkillObj = transform.Find("PrimarySkill");
        Transform secondarySkillObj = transform.Find("SecondarySkill");
        Transform skillListObj = transform.Find("SkillList");

        if (primarySkillObj != null) {
            primarySkill = primarySkillObj.GetComponentInChildren<Skill>();
        }
        if (secondarySkillObj != null) {
            secondarySkill = secondarySkillObj.GetComponentInChildren<Skill>();
        }
        if (skillListObj != null) {
            skillList = skillListObj.GetComponentsInChildren<Skill>().ToList();
        }

        if (primarySkill != null) {
            primarySkill.ownerObj = gameObject;
        }
        if (secondarySkill != null) {
            secondarySkill.ownerObj = gameObject;
        }
        if (skillList != null) {
            skillList.ForEach(skill => skill.ownerObj = gameObject);
        }
    }

    protected virtual void Awake() {
        status = new CreatureStatus();
        animationController = GetComponentInChildren<CustomAnimationController>();
        shield = new Shield(this);
        buffs = new List<BaseBuff>();
        buffs.AddRange(GetComponentsInChildren<BaseBuff>());

        LoadSkill();
    }

}
