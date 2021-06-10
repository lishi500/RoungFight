using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuffAction : Action
{
    public string buffName;
    public BaseBuff baseBuff;
    List<Creature> targetCreatures;
    public override void StartAction() {
        GameObject buffPrefabObj = LoadBuffPrefab();
        SelectBuffTarget(buffPrefabObj);

        foreach (Creature creature in targetCreatures) {
            GameObject buffObj = GameObject.Instantiate(buffPrefabObj, creature.transform);
            BaseBuff buff = buffObj.GetComponent<BaseBuff>();
            buff.caster = from;
            buff.holder = creature;
        }

        ActionEnd();
    }

    private void SelectBuffTarget(GameObject buffPrefab) {
        targetCreatures = toS;

        if (buffPrefab != null) {
            if (targetCreatures == null || targetCreatures.Count == 0) {
                BaseBuff buffPre = buffPrefab.GetComponent<BaseBuff>();
                targetCreatures = buffPre.SelectTargets();
            }

            if (targetCreatures.Count == 0) {
                Creature tar = GuessBuffTarget();
                targetCreatures.Add(tar);
            }
        }

    }

    private GameObject LoadBuffPrefab() {
        if (buffName != null) {
            GameObject buffPrefab = CommonUtil.Instance.FindPrafab(buffName, "Buffs");
            return buffPrefab;
        }

        return null;
    }

    private Creature GuessBuffTarget() {
        PartyType partyType = PartyType.None;
        TargetType targetType = baseBuff.targetType;
        if (from != null) {
            partyType = from.partyType;
        }

        switch (targetType) {
            case TargetType.SELF:
                return from;
            case TargetType.SELF_AND_PLAYER:
                if (partyType == PartyType.Player) {
                    return from.playerParty.player;
                } else {
                    return from;
                }
            case TargetType.ALLIES:
                if (partyType == PartyType.Player) {
                    return RandomCreatureExcludeSelf(from, from.playerParty.cats.ToList<Creature>());
                } else {
                    if (from == from.enemyParty.boss) {
                        if (from.enemyParty.creeps != null && from.enemyParty.creeps.Length > 0) {
                            return from.enemyParty.creeps.ElementAt(Random.Range(0, from.enemyParty.creeps.Length));
                        }
                    } else { // creeps
                        return RandomCreatureExcludeSelf(from, from.enemyParty.creeps.ToList<Creature>());
                    }
                }
                break;
            case TargetType.ENEMY:
                if (partyType == PartyType.Player) {
                    List<Creature> playerCreatures = new List<Creature>();
                    playerCreatures.Add(from.playerParty.player);
                    playerCreatures.Concat(from.playerParty.cats);
                    return RandomCreatureExcludeSelf(null, playerCreatures);
                } else {
                    List<Creature> enemyCreatures = new List<Creature>();
                    enemyCreatures.Add(from.enemyParty.boss);
                    enemyCreatures.Concat(from.enemyParty.creeps);
                    return RandomCreatureExcludeSelf(null, enemyCreatures);
                }
            case TargetType.CAT:
                return from.playerParty.cats.ElementAt(Random.Range(0, from.playerParty.cats.Count));
            case TargetType.BOSS:
                return from.enemyParty.boss;
            default:
                break;
        }
        return null;
    }

    private Creature RandomCreatureExcludeSelf(Creature self, List<Creature> creatures) {
        if (creatures != null && creatures.Count > 0) {
            List<Creature> otherCreature = creatures.Where(creature => creature != self).ToList();
            if (otherCreature.Count > 0) {
                System.Random rnd = new System.Random();
                int index = rnd.Next(otherCreature.Count);
                return otherCreature[index];
            }
        }
        return null;
    }
}
