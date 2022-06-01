using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль для управления отображаемым объектом скила
/// </summary>
public class TimerWithSpritemanager : MonoBehaviour
{
    [SerializeField, Tooltip("Целевое изображение для отображения спрайта скила")] Image targetedSprite;
    [SerializeField, Tooltip("Текстовое поле для отоюражения оставшегося времени перезарядки скила")] Text targetedText;
    [SerializeField, Tooltip("Обхект панели, закрывающей скрипт во время перезарядки")] GameObject coverPanel;
    [SerializeField, Tooltip("Обхект панели выбора скила")] GameObject selectionPanel;
    [SerializeField, Tooltip("Текстовое поле для отображения названия ключа действия для выбора скила")] Text keyText;
    [SerializeField, Tooltip("Модуль для изменения текста всплывающей подсказки")] TooltipTextUI tooltipText;

    private GameObject targetPlayer;
    private BaseSkill targetSkill;

    private float timerInterval = 0f;
    private float timerValue = 0f;

    private string keyName;

    public void SetTime(float time)
    {
        timerInterval = time;
        timerValue = 0f;
    }

    private void OnDestroy()
    {
        if (targetSkill != null) targetSkill.onRelease += SetTime;
    }
    /// <summary>
    /// Установить сущность, скил и ключ для отслеживания
    /// </summary>
    /// <param name="player">Целевая сущность</param>
    /// <param name="skill">Целевой скилл</param>
    /// <param name="keyName">Название ключа действия дял скила</param>
    public void SetEntity(GameObject player, BaseSkill skill, string keyName)
    {
        targetPlayer = player;
        targetSkill = skill;
        this.keyName = keyName;
        targetedSprite.sprite = skill.GetSprite();
        keyText.text = KeyHandler.NormalizeKeybind(KeyHandler.instance.GetKeybind(keyName));
        targetedText.text = "";
        skill.onRelease += SetTime;
        tooltipText.text = "\"" + skill.GetName() + "\"\n" + skill.GetDescription() + "\n" + "Cast time: " + skill.GetCastTime().ToString() + "s\n" + "Energy cost: " + skill.GetCost().ToString() + "\n" + "Cooldown: " + skill.GetCooldownTime().ToString() + "s";
        player.GetComponent<BaseEntity>().OnSkillSelectionChange += SetSelection;
    }

    /// <summary>
    /// Установить выбор скила на указанных
    /// </summary>
    /// <param name="skillKey">Название ключа для скила</param>
    /// <param name="selected">Выбран ли скил по указанному ключу</param>
    void SetSelection(string skillKey, bool selected)
    {
        if (skillKey == keyName)
        {
            selectionPanel.SetActive(selected);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timerValue += Time.deltaTime;
        float current = timerInterval - timerValue;
        if (current < 0f)
        {
            current = 0f;
        }
        keyText.text = KeyHandler.NormalizeKeybind(KeyHandler.instance.GetKeybind(keyName));
        targetedText.text = current > 0f ? Convert.ToInt32(current).ToString() + "s" : "";
        coverPanel.SetActive(current > 0f);
    }
}
