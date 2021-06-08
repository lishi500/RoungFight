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
    public Creature caster;
    [HideInInspector]
    public Creature holder;
    [HideInInspector]
    public PartyType partyType;

    public float value;
    public float factor;

    public int maxTriggerTimes;
    public bool isDeBuff;
    public bool isForever;
    public bool canDuplicate;
    public string attachedSkillName;
    public int prioirty = 10; // 10 is lowest, 1 is highest

    public BaseEffect OnApplyEffect;
    public BaseEffect OnTriggerEffect;
    public BaseEffect LivingEffect;
    public BaseEffect[] OtherEffects;


    private float roundPasted;
    [HideInInspector]
    public int tirggeredCount;
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
    protected virtual void OnRoundStart() {
        OnBuffTrigger();
        roundPasted += 1;
    }
    protected virtual void OnRoundEnd() {
        if (roundPasted >= duration) {
            Destroy(gameObject);
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
    // Possible benefit by other attribute
    public virtual float CalculatValue() {
        return value + caster.GetAttrVal(AttrType.Attack) * factor;
    }
 
    public void RenewBuff() {
        roundPasted = 0;
        tirggeredCount = 0;
    }
    // Start is called before the first frame update
    void RegisterEventListener() {
        if (eventHelper != null)
        {
            foreach (ReactEventType reactEventType in reactTypes)
            {
                switch (reactEventType)
                {
                    case ReactEventType.SELF_ATTACK:
                        //eventHelper.notifyRangeAttack += OnAttack;
                        //eventHelper.notifyMeleeAttack += OnAttack;
                        break;
                    case ReactEventType.SELF_CAST_SKILL:
                        //eventHelper.notifyCastSpell += OnCastSkill;
                        break;

                    case ReactEventType.SELF_GET_HIT:
                        //eventHelper.notifyGetHit += OnGetHit;
                        break;

                    //case ReactEventType.SKILL_READY:
                    //    eventHelper.notifySkillReady += OnSkillReady;
                    //    break;

                    case ReactEventType.DIE:
                        //    eventHelper.notifyDied += OnDie;
                        break;
                    default:
                        break;
                }
            }
        } 
    }

    public void ShowEffect(BaseEffect baseEffect, bool positionOnly = false)
    {
        GameObject effectObj = null;

        if (baseEffect.effect != null) {
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
       
        RegisterEventListener();

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

    public void OnDestroy()
    {
        if (notifyBuffRemoved != null) {
            notifyBuffRemoved(gameObject);
        }
        OnBuffRemove();
    }
}
