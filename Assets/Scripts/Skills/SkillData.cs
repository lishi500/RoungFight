//using DuloGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData", order = 1)]
public class SkillData : ScriptableObject {
    public int ID;
    public string Name;
    public SkillType type = SkillType.ACTIVE;
    public List<SkillBehaviorType> skillBehaviorTypes;
    public TargetType targetType;
    public List<SkillFactor> factors;
    public Sprite Icon;
    public string Description;
    //public float liveDuration;
    public int Cooldown;
    public bool CanDuplicate = true;
    public bool IsMultiTarget = false;

    //public UISpellInfo ConvertToUISpellInfo() {
    //    UISpellInfo uISpellInfo = new UISpellInfo();
    //    uISpellInfo.ID = this.ID;
    //    uISpellInfo.Name = this.Name;
    //    uISpellInfo.Icon = this.Icon;
    //    uISpellInfo.Description = (this.Description == null || this.Description == "") ? this.Name : this.Description;
    //    uISpellInfo.Cooldown = this.Cooldown;

    //    return uISpellInfo;
    //}
}
