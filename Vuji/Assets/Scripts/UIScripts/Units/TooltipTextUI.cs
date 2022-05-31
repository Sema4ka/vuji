using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public string text;

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