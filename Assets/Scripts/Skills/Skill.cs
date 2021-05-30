using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Skill : MonoBehaviour {
    public SkillData skillData;
    public string skillName {
        get { return skillData.Name; }
    }
    public int skillId {
        get { return skillData.ID; }
    }
    public int CD {
        get { return skillData.Cooldown; }
    }
    public SkillType type {
        get { return skillData.type; }
    }

    public float duration;
    public float baseValue;
    public float factor;
    public float castRange = float.MaxValue; // for AI, range that this skill should be cast
    public DamageType damageType;

    public bool loopCast;
    public bool isChannelSkill;
    //public ShootPointPosition shootPointPosition = ShootPointPosition.MID;

    public bool hasEffectController;
    public bool hasCollisionController;
    public bool needTarget = true;
    public bool isTargetAllies = false;

    [HideInInspector]
    public int CDLeft = 0;
    [HideInInspector]
    public GameObject ownerObj;
    public Creature owner {
        get { return ownerObj.GetComponent<Creature>(); }
    }
    [HideInInspector]
    public GameObject skillControllerObj;
    [NonSerialized]
    public bool canCd = true;
    [NonSerialized]
    private bool isReady = true; // skill is ready to use
    public bool IsReady {
        get { return isReady; }
    }


    //public List<EffectChain> effectChains;
    //public List<EffectCollider> colliderChains;
    //public List<SkillAttachedBuff> triggeredBuffDefs;
    //public List<SkillAttachedBuff> onApplyBuffDefs;

    //public BaseEffect OnCastEffect; // position caster
    //public BaseEffect OnTriggerEffect;

    public delegate void StartCDDelegate();
    public event StartCDDelegate notifyStartCd;

    public delegate void SkillReadyDelegate(Skill skill);
    public event SkillReadyDelegate notifySkillReady;

    public delegate void SkillCDChangeDelegate(Skill skill);
    public event SkillCDChangeDelegate notifySkillCDChange;

    //public SkillAnimationDef skillAnimations;
    public abstract void SkillSetup();
    public abstract void OnSkillAd();
    public abstract void OnSkillCast();
    public abstract void UpdateCollider();
    public abstract void UpdateEffect();

    //public virtual bool OnColliderTrigger(Transform collideObj, int colliderIndex = 0) {
    //    // return ture, continue process in SkillController
    //    // return false, handle in skill, skillController will skip
    //    return true;
    //}

    public virtual float CalculateValue() {
        float attack = owner.GetAttrVal(AttrType.Attack);
        return baseValue + (attack * factor);
    }

    public virtual GameObject GetCustomTarget() { return null; }

    //public virtual void ApplyBuffsToRole(List<SkillAttachedBuff> buffDefs, Role role) {
    //    foreach (SkillAttachedBuff buffDef in buffDefs) {
    //        GameObject buffObj = Instantiate(buffDef.buffObj);
    //        if (buffDef.overrideExisting) {
    //            BaseBuff baseBuff = buffObj.GetComponent<BaseBuff>();
    //            baseBuff.duration = buffDef.duration;
    //            baseBuff.frequency = buffDef.frequency;
    //            baseBuff.value = buffDef.value;
    //            baseBuff.factor = buffDef.factor;
    //        }
    //        role.AddBuff(buffObj, owner.GetComponent<Role>());
    //    }
    //}

    public void ReduceCD(int round = 1) {
        if (CDLeft > 0) {
            CDLeft -= round;
        }
    }
    public void UpdateCD() {
        if (canCd && !isReady && CD > 0) {
            if (CDLeft > 0) {
                ReduceCD();
            }

            if (CDLeft <= 0) {
                ResetCD();
            }
        }
    }

    public void StartCastSkill() {
        StartCD();
        OnSkillCast();
        //StartSkillAnimation();
    }

    public void StartCD() {
        //Debug.Log("Start CD " + skillName);
        if (CDLeft <= 0 && CD > 0) {
            CDLeft = CD;
            isReady = false;

            if (notifyStartCd != null) {
                notifyStartCd();
            }
        }
    }

    //public void StartSkillAnimation() {
    //    if (skillAnimations != null && skillAnimations.hasCustomSkillAnimation
    //        && ownerRole != null && ownerRole.animationController != null) {

    //        CustomAnimationController animationController = ownerRole.animationController;
    //        if (skillAnimations.useSkillId) {
    //            animationController.SetInt(AnimationState.SKILL_ANIMATION, skillId);
    //            if (skillAnimations.isExclusiveAnimation) {
    //                animationController.SetAllFalse();
    //                animationController.animationState = AnimationState.NONE;
    //            }
    //        }
    //    }
    //}

    private void OnSkillReady() {
        if (notifySkillReady != null) {
            notifySkillReady(this);
        }
    }
    private void OnSkillCDChange() {
        if (notifySkillCDChange != null) {
            notifySkillCDChange(this);
        }
    }

    public void ResetCD() {
        CDLeft = 0;
        isReady = true;
        OnSkillReady();
    }

    protected virtual void Start() {
        CDLeft = 0;
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");

    }
    protected virtual void Update() {
        if (hasEffectController) {
            UpdateEffect();
        }
        if (hasCollisionController) {
            UpdateCollider();
        }
    }
}