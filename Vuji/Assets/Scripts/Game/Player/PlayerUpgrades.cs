using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        KeyHandler.keyPressed += KeyPressed;
        SpawnPlayers.OnSpawn += OnSpawn;
    }
    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
        KeyHandler.keyPressed -= KeyPressed;
    }

    void OnSpawn(GameObject playerObject)
    {
        player = playerObject.GetComponent<BaseEntity>();
    }

    void Update()
    {
    }

    void KeyPressed(string name, KeyCode key)
    {
        if (name == "Upgrades")
        {
            upgradesPanel.gameObject.SetActive(!upgradesPanel.gameObject.activeSelf);
            KeyHandler.instance.SetUIOpened(upgradesPanel.gameObject.activeSelf);

        }
        if (name == "EscapeMenu")
        {
            if (upgradesPanel?.gameObject.activeSelf == true)
            upgradesPanel.gameObject.SetActive(false);
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
        if (xpPoints >= btn.GetComponent<BaseUpgrade>().GetCost())
        {
            xpPoints -= btn.GetComponent<BaseUpgrade>().GetCost();
            btn.onClick.RemoveAllListeners();
            Color col = btn.GetComponent<Image>().color;
            col.a = 0.1f;
            btn.GetComponent<BaseUpgrade>().ApplyUpgrade(player);
        }        
        
    }
    public void SwitchPanels(bool toClass=true)
    {
        classPanel.gameObject.SetActive(toClass);
        subclassPanel.gameObject.SetActive(!toClass);
    }
}
