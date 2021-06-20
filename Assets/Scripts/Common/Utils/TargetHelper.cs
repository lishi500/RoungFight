using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetHelper : Singleton<TargetHelper>
{
    private TargetType[] unsourceTargetList = new TargetType[] { TargetType.PLAYER, TargetType.BOSS, TargetType.BOARD };

    public List<Creature> SearchTargets(Creature from, TargetType targetType) {
        List<GameObject> objs = SearchTargets(from.gameObject, targetType);
        List<Creature> creatures = ToCreatures(objs);
        return creatures;
    }

    public List<GameObject> SearchTargets(GameObject from, TargetType targetType) {
        PartyType partyType = PartyType.None;
        Creature self = null;
        List<GameObject> targetGameObjects = new List<GameObject>();
        if (from != null && from.GetComponent<Creature>() != null) {
            self = from.GetComponent<Creature>();
            partyType = self.partyType;
        }
        if (self == null) {
            if (!unsourceTargetList.Contains(targetType)) {
                Debug.LogError("Try to find a target [" + targetType + "]  from NO Source ");
                return targetGameObjects;
            }
        }

        switch (targetType) {
            case TargetType.SELF: {
                    targetGameObjects.Add(from);
                    break;
                }
            case TargetType.SELF_AND_PLAYER: {
                    if (partyType == PartyType.Player) {
                        targetGameObjects.Add(self.playerParty.player.gameObject);
                    } else {
                        targetGameObjects.Add(from);
                    }
                    break;
                }
            case TargetType.ALLIES: {
                if (partyType == PartyType.Player) {
                    List<GameObject> catObjs = ToGameObject(self.playerParty.cats.Cast<Creature>().ToList());
                    targetGameObjects.Add(RandomObjectExcludeSelf(from, catObjs));
                    break;
                } else {
                    if (self == self.enemyParty.boss) {
                        if (self.enemyParty.creeps != null && self.enemyParty.creeps.Count > 0) {
                            List<GameObject> creeps = ToGameObject(self.enemyParty.creeps.Cast<Creature>().ToList());
                            targetGameObjects.Add(RandomObjectFromList(creeps));
                            break;
                        }
                    } else { // creeps
                        List<GameObject> creeps = ToGameObject(self.enemyParty.creeps.Cast<Creature>().ToList());
                        targetGameObjects.Add(RandomObjectExcludeSelf(from, creeps));
                        break;
                    }
                }
                break;
            }
                
            case TargetType.ENEMY: {
                if (partyType == PartyType.Player) {
                    List<GameObject> enemyObjects = new List<GameObject>();
                    List<GameObject> creeps = ToGameObject(self.enemyParty.creeps.Cast<Creature>().ToList());

                    enemyObjects.Add(self.enemyParty.boss.gameObject);
                    enemyObjects.Concat(creeps);
                    targetGameObjects.Add(RandomObjectFromList(enemyObjects));
                    break;
                } else {
                    List<GameObject> playerObjects = new List<GameObject>();
                    List<GameObject> catObjs = ToGameObject(self.playerParty.cats.Cast<Creature>().ToList());

                    playerObjects.Add(self.playerParty.player.gameObject);
                    playerObjects.Concat(catObjs);
                    targetGameObjects.Add(RandomObjectFromList(playerObjects));
                    break;
                }
            }
            case TargetType.ENEMIE_PARTY: {
                List<GameObject> enemyObjects = new List<GameObject>();
                List<GameObject> creeps = ToGameObject(self.enemyParty.creeps.Cast<Creature>().ToList());

                enemyObjects.Add(self.enemyParty.boss.gameObject);
                enemyObjects.Concat(creeps);
                targetGameObjects.AddRange(enemyObjects);
                break;
            }
                
            case TargetType.PLAYER_PARTY: {
                List<GameObject> playerObjects = new List<GameObject>();
                List<GameObject> catObjs = ToGameObject(self.playerParty.cats.Cast<Creature>().ToList());

                playerObjects.Add(self.playerParty.player.gameObject);
                playerObjects.Concat(catObjs);
                targetGameObjects.AddRange(playerObjects);
                break;
            }
               
            case TargetType.CAT: {
                List<GameObject> catObjs = ToGameObject(self.playerParty.cats.Cast<Creature>().ToList());
                targetGameObjects.Add(RandomObjectFromList(catObjs));
                break;
            }
               
            case TargetType.CATS: {
                List<GameObject> catObjs = ToGameObject(self.playerParty.cats.Cast<Creature>().ToList());
                targetGameObjects.Add(RandomObjectFromList(catObjs));
                break;
            }
              
            case TargetType.BOSS: {
                targetGameObjects.Add(self.enemyParty.boss.gameObject);
                break;
            }
               
            case TargetType.PLAYER: { 
                targetGameObjects.Add(self.playerParty.player.gameObject);
                break;
            }

            default:
                break;
        }
        return targetGameObjects;
    }

    public List<Creature> ToCreatures(List<GameObject> objects) {
        if (objects != null) {
            return objects
                    .Select(obj => obj.gameObject.GetComponent<Creature>())
                    .Where(creature => creature != null)
                    .ToList();
        }
        return new List<Creature>();
    }

    public List<GameObject> ToGameObject(List<Creature> creatures) {
        if (gameObject != null) {
            return creatures.Select(creature => creature.gameObject).ToList();
        }
        return new List<GameObject>();
    }

    private GameObject RandomObjectFromList(List<GameObject> objects) {
        return objects.ElementAt(Random.Range(0, objects.Count));
    }

 

    private GameObject RandomObjectExcludeSelf(GameObject self, List<GameObject> objs) {
        if (objs != null && objs.Count > 0) {
            List<GameObject> otherCreature = objs.Where(creature => creature != self).ToList();
            if (otherCreature.Count > 0) {
                System.Random rnd = new System.Random();
                int index = rnd.Next(otherCreature.Count);
                return otherCreature[index];
            }
        }
        return null;
    }
}
