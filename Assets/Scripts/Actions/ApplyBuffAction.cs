using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuffAction : Action
{
    public string buffName;
    public BaseBuff baseBuff;
    public override void StartAction() {
        GameObject buffPrefab = LoadBuffPrefab();
        if (buffPrefab != null) {
            if (targets == null || targets.Count == 0) { 
                
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
