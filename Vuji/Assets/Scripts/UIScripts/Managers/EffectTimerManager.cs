using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectTimerManager : MonoBehaviour
{
    [SerializeField] Image targetedSprite;
    [SerializeField] Text targetedText;
    [SerializeField] TooltipTextUI tooltipText;


    private BaseEffect targetedEffect;

    private float timerInterval = 0f;
    private float timerValue = 0f;
    // Start is called before the first frame update
    void Start()
    {
        targetedText.text = "";
    }

    public void SetEffect(BaseEffect effect)
    {
        targetedSprite.sprite = effect.effectSprite;
        timerInterval = effect.duration;
        targetedEffect = effect;
        tooltipText.text = "\"" + effect.effectName + "\"\n" + effect.description + "\n" + "Duration: " + effect.duration + "s";
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedEffect == null) return;
        timerValue += Time.deltaTime;
        float current = timerInterval - timerValue;
        if (current < 0f)
        {
            current = 0f;
            Destroy(gameObject);
        }
        targetedText.text = current > 0f ? Convert.ToInt32(current).ToString() + "s" : "";
    }
}
