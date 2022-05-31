using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerWithSpritemanager : MonoBehaviour
{
    [SerializeField] Image targetedSprite;
    [SerializeField] Text targetedText;
    [SerializeField] GameObject coverPanel;
    [SerializeField] GameObject selectionPanel;
    [SerializeField] Text keyText;

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

    public void SetEntity(GameObject player, BaseSkill skill, string keyName)
    {
        targetPlayer = player;
        targetSkill = skill;
        this.keyName = keyName;
        targetedSprite.sprite = skill.GetSprite();
        keyText.text = KeyHandler.NormalizeKeybind(KeyHandler.instance.GetKeybind(keyName));
        targetedText.text = "";
        skill.onRelease += SetTime;
        player.GetComponent<BaseEntity>().OnSkillSelectionChange += SetSelection;
    }

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
