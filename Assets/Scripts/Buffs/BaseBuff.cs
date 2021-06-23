using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff : MonoBehaviour
{
    public string buffName;
    public int duration; // round
    //public float frequency; // gap
    public BuffType type;
    public List<ReactEventType> reactTypes;
    [HideInInspector]
    public TargetType targetType;

    //[HideInInspector]
    public Creature caster;
    [HideInInspector]
    public Creature holder;

    public float value;
    public float factor;
    public DamageType damageType;

    public int maxTriggerTimes;
    public int maxReactTimes;
    public bool isDeBuff;
    public bool isForever;
    public bool canDuplicate;
    public bool canBeCleaned = true;
    public string attachedSkillName;
    public int prioirty = 10; // 10 is lowest, 1 is highest


    public BaseEffect OnApplyEffect;
    public BaseEffect OnTriggerEffect;
    public BaseEffect LivingEffect;
    public BaseEffect[] OtherEffects;

    [HideInInspector]
    public int seqId;
    // TODO make it private
    public float roundPasted;
    [HideInInspector]
    private int tirggeredCount;
    [HideInInspector]
    private int reactCount;
    [HideInInspector]
    SimpleEventHelper eventHelper;
    [HideInInspector]
    public delegate void OnBuffRemoveDelegate(GameObject self);
    [HideInInspector]
    public event OnBuffRemoveDelegate notifyBuffRemoved;

    public abstract BuffEvaluatorResult OnBuffEvaluated(BuffEvaluatorResult evaluatorResult);
    public abstract void OnBuffTrigger();
    public abstract void OnBuffApply();
    public abstract void OnBuffRemove();
    public abstract bool CanApplyTo(Creature creature); 
    public abstract void OnReactionTrigger(Action action);
    //public virtual List<Creature> SelectTargets() {
    //    return new List<Creature>();
    //}
    public void TriggerBuff() {
        if (tirggeredCount < maxTriggerTimes) {
            ShowEffect(OnTriggerEffect);
            OnBuffTrigger();
            tirggeredCount++;
        }
    }

    public void TriggerReact(Action action) {
        if (reactCount < maxReactTimes) {
            reactCount++;
            OnReactionTrigger(action);
        }
    }

    protected virtual void OnRoundStart() {
        if (tirggeredCount < maxTriggerTimes) {
            TriggerBuffAction triggerBuffAction = new TriggerBuffAction();
            triggerBuffAction.baseBuff = this;
            holder.party.actionChain.AddAction(triggerBuffAction, 0.4f);
        }
        roundPasted += 1;
    }
    protected virtual void OnRoundEnd() {
        if (roundPasted >= duration) {
            DestroyBuff();
        }
    }
    
    public virtual void OnAttack(string state) {
        Debug.Log("OnAttack BaseBuff");
    }
    public virtual void OnCastSkill(string state)
    {
    }

    public virtual void OnSkillReady(Skill skill)
    {

    }
    public virtual void OnGetHit(DamageDef damageDef)
    {

    }
    public virtual void OnEnemyEnterOuterZone(string state)
    {

    }
    public virtual void OnEnemyCollision(string state)
    {

    }
    public virtual void OnDie(string state)
    {
    }
    public virtual void OnBuffClean() {
        if (canBeCleaned) {
            DestroyBuff();
        }
    }

    // Possible benefit by other attribute
    public virtual float CalculatValue() {
        return value + caster.GetAttrVal(AttrType.Attack) * factor;
    }
 
    public void RenewBuff() {
        roundPasted = 0;
        tirggeredCount = 0;
    }
   
    public void ShowEffect(BaseEffect baseEffect, bool positionOnly = false)
    {
        GameObject effectObj = null;

        if (baseEffect != null && baseEffect.effect != null) {
            if (positionOnly)
            {
                effectObj = GameObject.Instantiate(baseEffect.effect, transform.position, Quaternion.identity);
            }
            else {
                effectObj = Instantiate(baseEffect.effect);
                effectObj.transform.SetParent(gameObject.transform);
                effectObj.transform.localPosition = Vector3.zero;
            }
           

            if (baseEffect.duration > 0)
            {
                effectObj.AddComponent<AutoDestroy>().timeToLive = baseEffect.duration;
            }

            ParticleSystem ps = effectObj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }
    }


    public virtual void Start()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        holder = GetComponentInParent<Creature>();
        //PlayerController playerController = GetComponentInParent<PlayerController>();
        //BaseAI baseAI = GetComponentInParent<BaseAI>();

        if (holder != null && holder.animationController != null) { 
            eventHelper = holder.animationController.eventHelper;
        }
        if (holder != null) {
            holder.party.notifyPartyRoundStart += OnRoundStart;
            holder.party.notifyPartyRoundEnd += OnRoundEnd;
        }
        seqId = GameManager.Instance.RegisterBuff(this);
        
        OnBuffApply();
        ShowEffect(OnApplyEffect);
        ShowEffect(LivingEffect);
    }

    public CustomAnimationController GetAnimationController() {
        return holder.animationController;
    }

    public bool IsPlayer() {
        return holder.GetType() == typeof(Player);
    }

    public void DestroyBuff() {
        OnBuffRemove();
        holder.status.RemoveStatusById(seqId);

        if (notifyBuffRemoved != null) {
            notifyBuffRemoved(gameObject);
        }
        GameManager.Instance.UnRegisterBuff(seqId);
        holder.party.notifyPartyRoundStart -= OnRoundStart;
        holder.party.notifyPartyRoundEnd -= OnRoundEnd;

        Destroy(gameObject);
    }
}
