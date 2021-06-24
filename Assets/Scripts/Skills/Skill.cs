using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Skill : MonoBehaviour {
    [HideInInspector]
    public int sequenceId;
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
   
    public bool hasTargetController;
    [HideInInspector]
    public GameObject targetObj { 
        get { return targetObjs != null && targetObjs.Count > 0 ? targetObjs[0] : null; }
    }
    [HideInInspector]
    public List<GameObject> targetObjs;
    protected Creature target {
        get {
            if (targetObj != null) {
                return targetObj.GetComponent<Creature>();
            }
            return null;
        }
    }
    protected List<Creature> targets {
        get {
            if (targetObjs != null) {
                return targetObjs.Select(obj => obj.GetComponent<Creature>()).ToList();
            }
            return null;
        }
    }

    [HideInInspector]
    public int CDLeft = 0;
    [HideInInspector]
    public GameObject ownerObj;
    public Creature owner {
        get { return ownerObj.GetComponent<Creature>(); }
    }
    [HideInInspector]
    public SkillController skillController {
        get { return GetComponent<SkillController>(); }
    }
    [NonSerialized]
    public bool canCd = true;
    [NonSerialized]
    private bool isReady = true; // skill is ready to use
    public bool IsReady {
        get { return isReady; }
    }

    public bool hasEffectController;
    public List<EffectChain> effectChains;
    //public List<EffectCollider> colliderChains;
    public List<SkillAttachedBuff> postCastBuffDefs;
    public List<SkillAttachedBuff> preCastBuffDefs;
    public bool isBuffAttacedSkill;

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
    // TODO control targets by skill
    //public abstract List<GameObject> SearchSkillTarget();

    public virtual float CalculateValue(int phase = 0) {
        SkillFactor factor = factors[phase];
        float attack = owner.GetAttrVal(AttrType.Attack);
        return factor.baseValue + (attack * factor.factor);
    }
   
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

    protected DamageDef QuickDamage(Creature tar, int phase = 0) {
        float damagePhase = CalculateValue(phase);
        DamageDef damageDef = DamageHelper.Instance.CalculateDamage(damagePhase, owner, tar, factors[phase].damageType);
        tar.ReduceHealth(damageDef);

        return damageDef;
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