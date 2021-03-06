using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuffAction : Action
{
    public string buffName;
    public GameObject buffPrefabObj;
    public BaseBuff baseBuff;
    List<Creature> targetCreatures;
    SkillAttachedBuff skillAttachedBuff;

    public ApplyBuffAction(GameObject self) : base(self) { }
    public ApplyBuffAction(GameObject self, GameObject target) : base(self, target) { }
    public ApplyBuffAction(GameObject self, List<GameObject> targets) : base(self, targets) { }

    public void SetSkillAttackedBuff(SkillAttachedBuff skillAttachedBuff) {
        this.skillAttachedBuff = skillAttachedBuff;
        buffPrefabObj = skillAttachedBuff.buffObj;
    }
    protected override void OnPrepareAction() {
        if (buffPrefabObj == null) {
            buffPrefabObj = LoadBuffPrefab();
        }
    }

    protected override void OnStartAction() {
        SelectBuffTarget(buffPrefabObj);

        foreach (Creature creature in targetCreatures) {
            GameObject buffObj = GameObject.Instantiate(buffPrefabObj, creature.transform);
            BaseBuff buff = buffObj.GetComponent<BaseBuff>();
            buff.caster = from;
            buff.holder = creature;
            creature.AddBuff(buff);
        }

        ActionEnd();
    }

    private void SelectBuffTarget(GameObject buffPrefab) {
        targetCreatures = toS;

        if (buffPrefab != null) {
            if (skillAttachedBuff != null) {
                List<GameObject> tars = new List<GameObject>();
                if (skillAttachedBuff.targetType == TargetType.INHERITED) {
                    //Debug.Log("INHERITED Target " + skillAttachedBuff.skill.targetObjs.Count);
                    tars = skillAttachedBuff.skill.targetObjs;
                } else {
                    tars = TargetHelper.Instance.SearchTargets(from.gameObject, skillAttachedBuff.targetType);
                    tars.Select(tar => tar.GetComponent<Creature>());
                }
                targetCreatures.AddRange(tars.Select(tar => tar.GetComponent<Creature>()));
            }
        }
    }

    private GameObject LoadBuffPrefab() {
        if (buffName != null) {
            GameObject buffPrefab = CommonUtil.Instance.FindPrafab(buffName, "Buffs");
            return buffPrefab;
        } else {
            Debug.LogError("No buff name, or buff prefab provided");
        }

        return null;
    }

    public override List<ActionType> DefaultActionType() {
        return new List<ActionType>() { ActionType.ApplyBuff };
    }

   
}
