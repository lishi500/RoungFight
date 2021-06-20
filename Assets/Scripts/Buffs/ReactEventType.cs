using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReactEventType
{
    SELF_ATTACK,
    SELF_CAST_SKILL,
    SELF_ENERGY_RECEIVE,
    SELF_GET_HIT, // PLAYER_GET_HIT

    PLAYER_HEAL,
    PLAYER_SHIELD,

    ALLIES_ATTACK,
    ALLIES_CAST_SKILL,
    ALLIES_ENERGY_RECEIVE,

    ENEMY_GET_HIT,
    ENEMY_ATTACK,
    ENEMY_CAST_SKILL,
    // other enemy...

    SKILL_READY,
    ENERGY_RECEIVE,

    TURN_CHANGE,
    ROUND_CHANGE,
    DIE,
   // board event
    ON_PIECE_REMOVE,
}
