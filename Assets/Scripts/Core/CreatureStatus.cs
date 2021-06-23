using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreatureStatus
{
    private Dictionary<int, StatusType> statusMap;
    private StatusType[] CannotActionType = new StatusType[] { StatusType.Stun, StatusType.Freeze, StatusType.Stoned, StatusType.Sleep };
    private StatusType[] ImmuControlType = new StatusType[] { StatusType.Immune, StatusType.SuperArmor };
    public bool CanAction {
        get { return !IsContainsAny(CannotActionType); }
    }
    public bool CanCastSkill {
        get { return !IsContainsAny(CannotActionType) && !IsContainsAny(StatusType.Silence); }
    }
    public bool CanBaseAttack {
        get { return !IsContainsAny(CannotActionType) && !IsContainsAny(StatusType.DisArm); }
    }
    public bool CanBeControl {
        get { return IsContainsAny(ImmuControlType); }
    }
    public bool CanBeTarget {
        get { return !IsContainsAny(StatusType.Invisible); }
    }
    public bool CanBePhysicalDamage {
        get { return !IsContainsAny(StatusType.Immune) && !IsContainsAny(StatusType.PhysicalImmu); }
    }
    public bool CanBeMagicalDamage {
        get { return !IsContainsAny(StatusType.Immune) && !IsContainsAny(StatusType.MagicImmu); }
    }

    public void AddStatus(int id, StatusType status) {
        statusMap.Add(id, status);
    }

    public void RemoveStatusById(int id) {
        statusMap.Remove(id);
    }
   
    // if isForce == false, will check whether this buff can be dispal
    public void CleanStatusByType(StatusType statusType, bool isForce = false) {
        List<int> pendingCleanId = new List<int>();

        if (isForce) {
            foreach (int id in statusMap.Keys) {
                StatusType type = statusMap[id];
                if (type == statusType) {
                    pendingCleanId.Add(id);
                }
            }
        } else {
            foreach (int id in statusMap.Keys) {
                BaseBuff buff = GameManager.Instance.GetBuffById(id);
                if (buff != null && buff.canBeCleaned) {
                    pendingCleanId.Add(id);
                }
            }
        }

        RemoveStatusAndBuffByIds(pendingCleanId);
    }

    private bool IsContainsAny(StatusType[] status) {
        return status != null && statusMap.Values.Any(val => status.Contains(val));
    }
    private bool IsContainsAny(StatusType status) {
        return statusMap.Values.Contains(status);
    }

    private void RemoveStatusAndBuffByIds(List<int> ids) {
        //ids.ForEach(id => statusMap.Remove(id));
        ids.ForEach(id => {
            BaseBuff buff = GameManager.Instance.GetBuffById(id);
            if (buff != null) {
                buff.OnBuffClean();
            }
        });
    }

    public CreatureStatus() {
        statusMap = new Dictionary<int, StatusType>();
    }
}
