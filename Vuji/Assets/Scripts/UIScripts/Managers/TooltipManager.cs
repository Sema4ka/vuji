using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{

    public static TooltipManager instance;
    #region skillTooltip
    public GameObject skillTooltipBox;
    [SerializeField] Text skillName;
    [SerializeField] Text skillDescription;
    [SerializeField] Text skillCooldown;
    [SerializeField] Text skillCost;
    #endregion
    #region itemTooltip
    public GameObject itemTooltipBox;
    [SerializeField] Text itemName;
    [SerializeField] Text itemDescription;
    [SerializeField] Text itemAmount;
    #endregion
    #region effectTooltip
    public GameObject effectTooltipBox;
    [SerializeField] Text effectName;
    [SerializeField] Text effectDescription;
    #endregion
    private static string current;

    private void Start()
    {
        instance = this;
    }

    public static void SetSkillTooltip(string name, BaseSkill skill)
    {
        current = name;
        instance.skillName.text = skill.GetName();
        instance.skillDescription.text = skill.GetDescription();
        instance.skillName.text = skill.GetCooldownTime().ToString();
        instance.skillName.text = skill.GetCost().ToString();
        instance.skillTooltipBox.SetActive(true);
    }

    public static void SetItemTooltip(string name, BaseItem item)
    {
        current = name;
        instance.itemName.text = item.GetItemName();
        instance.itemDescription.text = item.GetDescription();
        instance.itemAmount.text = item.GetAmount().ToString();
        instance.itemTooltipBox.SetActive(true);
    }
    public static void SetEffectTooltip(string name, BaseEffect effect)
    {
        current = name;
        instance.effectName.text = effect.effectName;
        instance.effectDescription.text = effect.description;
        instance.effectTooltipBox.SetActive(true);
    }

    public static void ClearTooltip(string name)
    {
        if (current != name)
        {
            return;
        }
        current = "";
        instance.skillName.text = "";
        instance.skillDescription.text = "";
        instance.skillCooldown.text = "";
        instance.skillCost.text = "";
        instance.itemName.text = "";
        instance.itemDescription.text = "";
        instance.itemAmount.text = "";
        instance.effectName.text = "";
        instance.effectDescription.text = "";
        instance.skillTooltipBox.SetActive(false);
        instance.itemTooltipBox.SetActive(false);
        instance.effectTooltipBox.SetActive(false);
    }
}
