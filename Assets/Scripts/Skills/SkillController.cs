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


        PlayParticleSystem(effect);

    }


    public void ShowBaseEffect(BaseEffect baseEffect, Vector3 position) {
        if (baseEffect.effect != null) {
            GameObject effectObj = GameObject.Instantiate(baseEffect.effect, position, Quaternion.identity);

            if (baseEffect.duration > 0) {
                effectObj.AddComponent<AutoDestroy>().timeToLive = baseEffect.duration;
            }

            PlayParticleSystem(effectObj);
        }
    }

    private void PlayParticleSystem(GameObject effect)
    {
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
    }
    public Transform GeneratePositionByType(PositionType type) {
        switch (type) {
            case PositionType.CENTER:
                return GameManager.Instance.centerFightPoint;
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


    void UpdateEffect()
    {
        if (skill.effectChains != null && effectChainIndex < skill.effectChains.Count)
        {
            EffectChain effectChain = skill.effectChains[effectChainIndex];
            if (effectChain.delay <= pastTime)
            {
                ShowEffectChain(effectChain, GeneratePositionByType(effectChain.positionType).position);
                effectChainIndex++;
                //if (effectChainIndex >= skill.effectChains.Count) {
                //    AddDelayDestory(skill.effectChains[effectChainIndex - 1].duration);
                //}
            }
        }

    }

    public Vector3 GetPositionWithDistanceAndAngle(Transform from, float distance, float angle) {
        Vector3 newPos = from.position + Quaternion.AngleAxis(angle, Vector3.up) * from.forward * distance;
        return newPos;
    }

    public virtual void SkillOnTrail() { }
    public virtual void OnSkillCast() { }
    public virtual void SkillUpdate() { }
    public virtual void OnSkillStop() { }

    public virtual void InitialSkill() {
        pastTime = 0;
        effectChainIndex = 0;
        skill.SkillSetup();

        skill.StartCastSkill();
        OnSkillCast();
        isStarted = true;

        StartCoroutine(ShowEffectWithDelay(skill.OnCastEffect, skill.owner.transform.position));
        Destroy(gameObject, skill.skillData.liveDuration + 0.05f);
    }

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted) {
            pastTime += Time.deltaTime;

            SkillUpdate();
            UpdateEffect();
            //if (!skill.hasEffectController)
            //{
            //    UpdateEffect();
            //}
            //else {
            //    skill.UpdateEffect();
            //}
        }

    }

    private void OnSkillFinish(Skill skill) {
        Destroy(this);
    }

    void OnDestroy()
    {
        if (notifySkillFinish != null) {
            notifySkillFinish();
        }
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
