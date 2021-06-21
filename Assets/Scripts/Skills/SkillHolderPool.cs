using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class SkillHolderPool : Singleton<SkillHolderPool>
{
    public Dictionary<String, List<GameObject>> pool;

    protected override void Awake() {
        base.Awake();
        pool = new Dictionary<String, List<GameObject>>();
    }

    public void EnPool(GameObject skillController, String skillName) {
        if (skillController.activeSelf) {
            skillController.SetActive(false);
        }

        if (pool.ContainsKey(skillName)) {
            pool[skillName].Add(skillController);
        } else {
            List<GameObject> objs = new List<GameObject>();
            objs.Add(skillController);
            pool.Add(skillName, objs);
        }
    }

    public GameObject DePool(String skillName) {
        if (pool.ContainsKey(skillName)) {
            List<GameObject> objs = pool[skillName];
            if (objs.Count > 0) {
                GameObject obj = objs[0];
                objs.RemoveAt(0);
                return obj;
            }
        }
        return null;
    }
}
