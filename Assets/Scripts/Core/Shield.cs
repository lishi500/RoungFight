using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Shield
{
    public float amount {
        get { return GetShieldAmount(); }
    }
    List<ShieldFragment> shields;
    Creature owner;

    public delegate void ShieldAddEvent();
    public event ShieldAddEvent notifyShieldAdd;
    public delegate void ShieldReduceEvent();
    public event ShieldReduceEvent notifyShieldReduce;
   
    // if same shield can stack, then we can have multiple shield come from same source
    public Shield(Creature owner) {
        shields = new List<ShieldFragment>();
        this.owner = owner;
        owner.party.notifyPartyRoundEnd += OnRoundEnd;
    }
    public void ReduceShield(DamageDef damageDef) {
        float damge = damageDef.damage;
        foreach (ShieldFragment shield in shields) {
            if (shield.amount >= damge) {
                shield.amount -= damge;
                damge = 0;
                // shield reduce all damage event
                break;
            } else {
                damge -= shield.amount;
                shield.amount = 0;
            }
        }
        OnShieldReduce();

        CleanShieldFragment();
        SortShieldByRoundToLive(); 

        damageDef.damage = damge;
    }

    public void AddShield(float amount, int roundToLive, bool canStack, Type classType) {
        if (canStack) {
            AddShieldFragment(amount, roundToLive, classType);
        } else {
            if (IsContainShieldType(classType)) {
                RefreshShieldFragment(amount, roundToLive, classType);
            } else {
                AddShieldFragment(amount, roundToLive, classType);
            }
        }
        OnShieldAdd();
    }

    private void OnRoundEnd() {
        shields.ForEach(shield => shield.roundToLive -= 1);
        CleanShieldFragment();
        SortShieldByRoundToLive();
    }

    private void AddShieldFragment(float amount, int roundToLive, Type classType) {
        ShieldFragment shield = new ShieldFragment(amount, roundToLive, classType);
        shields.Add(shield);
        SortShieldByRoundToLive();
    }

    private void RefreshShieldFragment(float amount, int roundToLive, Type classType) {
        ShieldFragment shield = shields.Find(shield => shield.classType == classType);
        if (shield != null) {
            shield.amount = amount;
            shield.maxAmount = amount;
            shield.roundToLive = roundToLive;
            SortShieldByRoundToLive();
        }
    }
    private float GetShieldAmount() {
        if (shields != null) {
            return shields.Sum(shield => shield.amount);
        }
        
        return 0;
    }
    private void SortShieldByRoundToLive() {
        shields.Sort(delegate (ShieldFragment a, ShieldFragment b) {
            return a.roundToLive > b.roundToLive ? 1 : -1;
        });
    }

    private void CleanShieldFragment() {
        shields.RemoveAll(fragment => fragment.amount <= 0 || fragment.roundToLive <= 0);
    }

    private bool IsContainShieldType(Type classType) {
        return shields.Find(shield => shield.classType == classType) != null;
    }

    private void OnShieldAdd() {
        if (notifyShieldAdd != null) {
            notifyShieldAdd();
        }
    }
    private void OnShieldReduce() {
        if (notifyShieldReduce != null) {
            notifyShieldReduce();
        }
    }

    [Serializable]
    public class ShieldFragment {
        public float amount;
        public float maxAmount;
        public int roundToLive;
        public Type classType;

        public ShieldFragment(float amount, int roundToLive, Type classType) {
            this.amount = amount;
            this.roundToLive = roundToLive;
            this.classType = classType;
        }
    }
}
