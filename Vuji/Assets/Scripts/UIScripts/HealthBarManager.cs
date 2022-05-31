using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Slider slider;
    [SerializeField] Color Low;
    [SerializeField] Color High;
    private Vector3 offset;
    private void Start()
    {
        GetComponentInParent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        slider.fillRect.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        slider.fillRect.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        slider.fillRect.parent.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        slider.fillRect.parent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
    }
    public void SetHealth(float health, float maxHp)
    {
        gameObject.SetActive(health < maxHp);
        slider.value = health;
        slider.maxValue = maxHp;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, slider.normalizedValue);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
        slider.transform.position = new Vector3(temp.x, temp.y, 0);
    }
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
    public Vector3 GetOffset()
    {
        return offset;
    }
}
