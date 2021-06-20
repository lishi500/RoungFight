using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReactionController : Singleton<ReactionController> {
    List<ReactEventType> baseAttackReaction = new List<ReactEventType> { ReactEventType.SELF_ATTACK, ReactEventType.ALLIES_ATTACK, ReactEventType.ENEMY_ATTACK };
    List<ReactEventType> castSkillReaction = new List<ReactEventType> { ReactEventType.SELF_CAST_SKILL, ReactEventType.ALLIES_CAST_SKILL, ReactEventType.ENEMY_CAST_SKILL };

    List<ReactEventType> selfGroup = new List<ReactEventType> {
        ReactEventType.SELF_ATTACK,
        ReactEventType.SELF_CAST_SKILL,
        ReactEventType.SELF_ENERGY_RECEIVE,
        ReactEventType.SELF_GET_HIT
    };
    List<ReactEventType> alliesGroup = new List<ReactEventType> {
        ReactEventType.ALLIES_ATTACK,
        ReactEventType.ALLIES_CAST_SKILL,
        ReactEventType.ALLIES_ENERGY_RECEIVE
    };
    List<ReactEventType> enemyGroup = new List<ReactEventType> {
        ReactEventType.ENEMY_GET_HIT,
        ReactEventType.ENEMY_ATTACK,
        ReactEventType.ENEMY_CAST_SKILL,
    };
    List<ReactEventType> playerGroup = new List<ReactEventType> {
        ReactEventType.PLAYER_HEAL,
        ReactEventType.PLAYER_SHIELD,
    };

    public BuffEvaluatorResult EvaluateBuffs(Creature from, ActionType actionType, Action action) {
        BuffEvaluatorResult result = new BuffEvaluatorResult();
        result.action = action;
        result.actionType = actionType;
        List<ReactEventType> reactTypes = GetReactTypes(actionType);

        foreach (ReactEventType reactEvent in reactTypes) {
            List<Creature> creatures = GetCreatureByReactType(from, reactEvent);
            foreach (Creature creature in creatures) {
                CheckCreatureBuffs(creature, reactEvent, result);
            }
        }

        return result;
    }

    private List<Creature> GetCreatureByReactType(Creature from, ReactEventType reactEventType) {
        TargetType targetType = GetReactEventTargetType(reactEventType);
        List<Creature> creatures = TargetHelper.Instance.SearchTargets(from, targetType);

        return creatures;
    }

    private void CheckCreatureBuffs(Creature creature, ReactEventType reactEventType, BuffEvaluatorResult evaluatorResult) {
        if (creature.buffs.Count > 0) {
            foreach (BaseBuff buff in creature.buffs) {
                if (IsContainsReactEventTyle(buff, reactEventType)) {
                    evaluatorResult = buff.OnBuffEvaluated(evaluatorResult);
                    buff.TriggerReact(evaluatorResult.action);
                }
            }
        }
    }

    private bool IsContainsReactEventTyle(BaseBuff buff, ReactEventType reactEventType) {
        return buff.reactTypes != null && buff.reactTypes.Contains(reactEventType);
    }


    private TargetType GetReactEventTargetType(ReactEventType reactEventType) {
        if (selfGroup.Contains(reactEventType)) {
            return TargetType.SELF;
        } else if (alliesGroup.Contains(reactEventType)) {
            return TargetType.ALLIES;
        } else if (enemyGroup.Contains(reactEventType)) {
            return TargetType.ENEMIE_PARTY;
        } else if (playerGroup.Contains(reactEventType)) {
            return TargetType.PLAYER;
        }

        return TargetType.NONE;
    }

    private List<ReactEventType> GetReactTypes(ActionType actionType) {
        switch (actionType) {
            case ActionType.BaseAttack:
                return baseAttackReaction;
            case ActionType.CastSkill:
                return castSkillReaction;
            default:
                return new List<ReactEventType>();
        }
    }

}

//BaseAttack,
//CastSkill,
//Charge,

//ApplyBuff,
//Buff,
//Debuff,
//TriggerBuff,

//Heal,
//Shield,
//Die,
//Thorns,
//Control,
//Summon,
//Damage,
//Critical,
//Physical,
//Magical,