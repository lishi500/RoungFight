using System;
using System.Linq;
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
    public List<SkillFactor> factors {
        get { return skillData.factors; }
    }
   

    public bool hasEffectController;
    //public bool hasCollisionController;
    public bool isTargetAllies = false;
    public GameObject targetObj;
    public List<GameObject> targetObjs;
    protected Creature target;
    protected List<Creature> targets;


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

    public List<EffectChain> effectChains;
    //public List<EffectCollider> colliderChains;
    //public List<SkillAttachedBuff> triggeredBuffDefs;
    //public List<SkillAttachedBuff> onApplyBuffDefs;

    public BaseEffect OnCastEffect; // position caster
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
    //public abstract void UpdateCollider();
    protected abstract IEnumerator SkillProgress();
    public abstract void UpdateEffect();

    public virtual float CalculateValue(int phase = 0) {
        SkillFactor factor = factors[phase];
        float attack = owner.GetAttrVal(AttrType.Attack);
        return factor.baseValue + (attack * factor.factor);
    }

    protected virtual void LoadTargetCreature() {
        if (targetObj != null) {
            target = targetObj.GetComponent<Creature>();        
        }
        if (targetObjs != null) {
            targets = targetObjs.Select(obj => obj.GetComponent<Creature>()).ToList();
        }
        
    }
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

    protected void QuickDamage(Creature tar, int phase = 0) {
        float damagePhase = CalculateValue(phase);
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(damagePhase, owner, tar, factors[phase].damageType);
        tar.ReduceHealth(damageDef);
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
        //GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
    }
    protected virtual void Update() {
        //if (hasEffectController) {
        //    UpdateEffect();
        //}
        //if (hasCollisionController) {
        //    UpdateCollider();
        //}
    }
}