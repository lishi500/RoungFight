using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
   SELF,
   SELF_AND_PLAYER, // if enemy party then enemy, if palyer party then player
   ALLIES,
   ENEMY,
   CAT,
   PLAYER,
   BOSS
   //BOARD
}
