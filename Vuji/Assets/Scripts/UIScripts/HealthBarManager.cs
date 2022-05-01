using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Color Low;
    [SerializeField] Color High;
    private Vector3 offset;

    public void SetHealth(float health, float maxHp)
    {
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
