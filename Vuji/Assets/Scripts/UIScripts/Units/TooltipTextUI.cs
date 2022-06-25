using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
/// <summary>
/// Модуль для объектов UI, при наведении на которых будет показана подсказка
/// </summary>
public class TooltipTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Tooltip("Inner tooltip name")] public string tooltipName;
    public BaseEffect effect;
    public BaseItem item;
    public BaseSkill skill;
    /*public BasePassiveSkill passiveSkill;
    public BaseProjectile projectile;*/


    void IPointerEnterHandler.OnPointerEnter(PointerEventData e)
    {
        if (effect != null) TooltipManager.SetEffectTooltip(tooltipName, effect);
        if (item != null) TooltipManager.SetItemTooltip(tooltipName, item);
        if (skill != null) TooltipManager.SetSkillTooltip(tooltipName, skill);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData e)
    {
        TooltipManager.ClearTooltip(tooltipName);
    }

    void OnDestroy()
    {
        TooltipManager.ClearTooltip(tooltipName);
    }
}