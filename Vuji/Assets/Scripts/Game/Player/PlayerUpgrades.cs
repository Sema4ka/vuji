using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] private BaseEntity player;
    [SerializeField] private RectTransform upgradesPanel;
    [SerializeField] private int xpPoints;
    [SerializeField] private RectTransform classPanel;
    [SerializeField] private RectTransform subclassPanel;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            upgradesPanel.gameObject.SetActive(!upgradesPanel.gameObject.activeSelf);
        }    
    }

    public int GetXp()
    {
        return xpPoints;
    }
    public void AddXp(int points)
    {
        if (points < 0) return;
        xpPoints += points;
    }
    public void AddUpgrade(Button btn)
    {
        if (xpPoints >= btn.GetComponent<BaseUpgrade>().getCost())
        {
            xpPoints -= btn.GetComponent<BaseUpgrade>().getCost();
            btn.onClick.RemoveAllListeners();
            Color col = btn.GetComponent<Image>().color;
            col.a = 0.1f;
            btn.GetComponent<BaseUpgrade>().applyUpgrade(player);
        }        
        
    }
    public void SwitchPanels(bool toClass=true)
    {
        classPanel.gameObject.SetActive(toClass);
        subclassPanel.gameObject.SetActive(!toClass);
    }
}
