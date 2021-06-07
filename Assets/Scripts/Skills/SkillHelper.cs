using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHelper : Singleton<SkillHelper>
{
    public GameObject GetSkillPrefab(Skill skill) {
        if (skill != null) {
            return GetSkillPrefab(skill.GetType().ToString());
        }
        return null;
    }

    public GameObject GetSkillPrefab(string skillName) {
        GameObject prefab = CommonUtil.Instance.FindPrafab(skillName, "Skills");
        return prefab;
    }

}
