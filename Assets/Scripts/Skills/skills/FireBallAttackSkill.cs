using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAttackSkill : Skill
{
    public GameObject flameEffectPrefab;
    public GameObject explodeEffectPrefab;

    public GameObject flameEffect;
    private GameObject explodeEffect;

    private bool isStarted = false;


    private Vector3 flameEndPos;

    public override void OnSkillAd() {
    }

    public override void OnSkillCast() {
    }

    public override void SkillSetup() {
    }

    public override void UpdateEffect() {
        // TODO refactor SkillPool, get issue to clean update Skill.
        if (!isStarted) {
            isStarted = true;
            StartFlameEffect();
        }
    }

    private void OnFlameArrive() {
        flameEndPos = flameEffect.transform.position;
        Destroy(flameEffect);
        StartCoroutine(SkillProgress());
        StartExplodeEffect();
    }

    private void StartFlameEffect() {
        Debug.Log("StartFlameEffect " + sequenceId);
        if (flameEffect == null) {
            flameEffect = Instantiate(flameEffectPrefab);
            flameEffect.transform.position = owner.transform.position;
        }

        MoveTo moveTo = flameEffect.AddComponent<MoveTo>();
        moveTo.SetUp(target.gameObject, 1f, MoveFunc.EaseIn, true);
        moveTo.notifyDestinationArrived += OnFlameArrive;

        ParticleUtils.Instance.PlayParticleSystem(flameEffect);
        moveTo.StartMove();
    }

    private void StartExplodeEffect() {
        if (explodeEffectPrefab != null) {
            explodeEffect = Instantiate(explodeEffectPrefab);
            explodeEffect.transform.position = flameEndPos;
            ParticleUtils.Instance.PlayParticleSystem(explodeEffect);
        }
    }

    protected override IEnumerator SkillProgress() {
        yield return new WaitForSeconds(0.1f);
        DamageDef damageDef1 = QuickDamage(target, 0);
        //Debug.Log(name + ":" + sequenceId + " Phase: 1" + "Damage: " + damageDef1.damage + " : " + Time.time);

        skillController.OnSkillFinish();
    }

   
}
