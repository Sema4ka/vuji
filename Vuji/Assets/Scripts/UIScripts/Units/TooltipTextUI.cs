using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
/// <summary>
/// Модуль для объектов UI, при наведении на которых будет показана подсказка
/// </summary>
public class TooltipTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Tooltip("Текст всплывающей подсказки")] public string text;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData e)
    {
        UIHintManager.text = text;
        UIHintManager.isUI = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData e)
    {
        UIHintManager.isUI = false;
    }

    void OnDestroy()
    {
        UIHintManager.isUI = false;
    }
}