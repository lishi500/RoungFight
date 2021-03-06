using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    SELF,
    SELF_AND_PLAYER, // if enemy party then enemy, if palyer party then player
    ALLIES,
    ENEMY,
    BOSS,
    // ENEMY_CREEPS, if enemy party, creeps first then boss
    CAT,
    CATS,
    PLAYER,
    PLAYER_PARTY,
    ENEMIE_PARTY,
    INHERITED,
    BOARD,

    NONE
}
