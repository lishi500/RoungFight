using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public GameObject creator;
    public GameObject primaryTarget;
    public List<GameObject> targets;

    public Skill skill;

    //protected PrafabHolder prafabHolder;
    [HideInInspector]
    public Action action;

    [HideInInspector]
    public int effectChainIndex;
   
    [HideInInspector]
    public float pastTime;
    [HideInInspector]
    public Transform position1;
    [HideInInspector]
    public Transform position2;
    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public bool isStarted = false;



    public virtual void SkillOnTrail() { }
    public virtual void OnSkillCast() {
        ApplyAttachedBuff();
    }
    public virtual void SkillUpdate() { }
    public virtual void OnSkillStop() { }

    public virtual void InitialSkill() {
        pastTime = 0;
        effectChainIndex = 0;
        skill.SkillSetup();
        Debug.Log(skill.sequenceId + " start skill " + skill.name);
        ReactionController.Instance.EvaluateBuffs(creator.GetComponent<Creature>(), ActionType.CastSkill, action);

        skill.StartCastSkill();
        OnSkillCast();
        isStarted = true;

        StartCoroutine(ShowEffectWithDelay(skill.OnCastEffect, skill.owner.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted) {
            pastTime += Time.deltaTime;

            SkillUpdate();
            UpdateEffect();
            if (skill.hasEffectController) {
                skill.UpdateEffect();
            } else {
                UpdateEffect();
            }
        }

    }

    // -------------------- Buff ---------------------------------------
    public void ApplyBuffToCreature(Creature creature, BaseBuff buff) {
        if (creature != null) { 
            ApplyBuffAction applyBuffAction = new ApplyBuffAction(creator, creature.gameObject);
            applyBuffAction.buffName = buff.buffName;
            creature.party.actionChain.AddActionJumpQueue(applyBuffAction, 0);
        }
    }

    void ApplyAttachedBuff() { 
        if (skill.onApplyBuffDefs != null && skill.onApplyBuffDefs.Count > 0) {
            foreach (SkillAttachedBuff attachedBuff in skill.onApplyBuffDefs) {
                attachedBuff.skill = skill;
                ApplyBuffAction applyBuffAction = new ApplyBuffAction(creator);
                applyBuffAction.SetSkillAttackedBuff(attachedBuff);
                Creature creature = creator.GetComponent<Creature>();
                if (creature != null) {
                    creature.party.actionChain.AddAction(applyBuffAction, 0);
                } else {
                    // TODO handle no creator skill/buff
                    Debug.LogError("handle no creator skill/buff");
                }
            }
        }
    }


     // ------------------- Effect Part Start --------------------------------
    void UpdateEffect() {
        if (skill.effectChains != null && effectChainIndex < skill.effectChains.Count) {
            EffectChain effectChain = skill.effectChains[effectChainIndex];
            if (effectChain.delay <= pastTime) {
                Debug.Log(skill.name + ":" + skill.sequenceId + " EffectChain: " + effectChainIndex + " : " + Time.time);
                ShowEffectChain(effectChain, GeneratePositionByType(effectChain.positionType).position);
                effectChainIndex++;
            }
        }
    }

    void ShowEffectChain(EffectChain effectChain, Vector3 position) {
        GameObject effect = null;
        if (effectChain.followRole) {
            effect = GameObject.Instantiate(effectChain.effect, position, Quaternion.identity, GeneratePositionByType(effectChain.positionType));
        } else {
            effect = GameObject.Instantiate(effectChain.effect, position, Quaternion.identity);
        }

        if (effectChain.useRoleFacing) {
            Vector3 targetPos = GetPositionWithDistanceAndAngle(creator.transform, 10, 0);
            effect.transform.LookAt(targetPos);
        }

        float destoryTime = effectChain.duration == 0 ? 3 : effectChain.duration;
        effect.AddComponent<AutoDestroy>().timeToLive = destoryTime;

        ParticleUtils.Instance.PlayParticleSystem(effect);
    }
    void ShowBaseEffect(BaseEffect baseEffect, Vector3 position) {
        if (baseEffect.effect != null) {
            GameObject effectObj = GameObject.Instantiate(baseEffect.effect, position, Quaternion.identity);

            if (baseEffect.duration > 0) {
                effectObj.AddComponent<AutoDestroy>().timeToLive = baseEffect.duration;
            }

            ParticleUtils.Instance.PlayParticleSystem(effectObj);
        }
    }
    // TODO more position type
    private Transform GeneratePositionByType(PositionType type) {
        switch (type) {
            case PositionType.CENTER:
                return GameManager.Instance.centerFightPoint;
            case PositionType.TARGET: // refine, primary target/multi target/ first target
                return targets[0].transform;
            default:
                return GameManager.Instance.centerFightPoint;
        }
    }
    private IEnumerator ShowEffectWithDelay(BaseEffect baseEffect, Vector3 position) {
        if (baseEffect.effect != null) {
            if (baseEffect.delay == 0) {
                ShowBaseEffect(baseEffect, position);
            } else {
                yield return new WaitForSeconds(baseEffect.delay);
                ShowBaseEffect(baseEffect, position);
            }
        }

        yield return null;
    }

    private Vector3 GetPositionWithDistanceAndAngle(Transform from, float distance, float angle) {
        Vector3 newPos = from.position + Quaternion.AngleAxis(angle, Vector3.up) * from.forward * distance;
        return newPos;
    }
    // ------------------------ Effect part end ---------------------------------


    public void OnSkillFinish() {
        if (notifySkillFinish != null) {
            notifySkillFinish();
        }
        gameObject.SetActive(false);
    }

    public delegate void SkillFinishDelegate();
    public event SkillFinishDelegate notifySkillFinish;
}

//[HideInInspector]
//public int colliderChainIndex;

//void UpdateCollider()
//{
//    if (colliderChainIndex < skill.colliderChains.Count)
//    {
//        EffectCollider effectCollider = skill.colliderChains[colliderChainIndex];
//       //  Debug.Log("UpdateCollider " + effectCollider.delay + " <= " + pastTime + " - " + (effectCollider.delay <= pastTime));

//        if (effectCollider.delay <= pastTime) {
//            ApplyCollider(effectCollider, CommonUtils.Instance.IsPositionZero(effectCollider.center) ? 
//                GeneratePositionByType(effectCollider.position).position : effectCollider.center);
//            colliderChainIndex++;
//        }
//    }
//}



//private EffectCollider gizmosCollider;
//private Vector3 gizmosColliderPosition;
//void ApplyCollider(EffectCollider effectCollider, Vector3 position) {
//    //Debug.Log("ApplyCollider");
//    gizmosCollider = effectCollider;
//    gizmosColliderPosition = position;
//    // ---------------
//    string[] targetTags = SkillUtils.Instance.GetSkillTargetsTags(skill);
//    List<Transform> collideObjs = null;
//    if (effectCollider.type == ColliderType.Sphere) {
//        collideObjs = AttackUtils.Instance.CircleCollision(position, effectCollider.radius, targetTags);
//    }

//    if (collideObjs != null) {
//        foreach (Transform collide in collideObjs) {
//            bool continueProcess = skill.OnColliderTrigger(collide, colliderChainIndex);
//            if (continueProcess) {
//                OnDefaultColliderTrigger(collide);
//            }
//        }
//    }
//}

//public virtual void OnDefaultColliderTrigger(Transform transform) {
//    ShowBaseEffect(skill.OnTriggerEffect, transform.position);
//    Role hitRole = transform.GetComponent<Role>();
//    if (hitRole != null) {
//        skill.ApplyBuffsToRole(skill.triggeredBuffDefs, hitRole);
//        hitRole.ReduceHealth(skill.CalculateValue());
//    }
//}
