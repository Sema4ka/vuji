using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPreviewManager : MonoBehaviour
{
    public RectTransform skillsPanel;
    public RectTransform inventoryPanel;
    public Image charachterPreview;
    public Text charachterName;


    [SerializeField] GameObject skillPrefab;

    [Tooltip("Список префабов всех персонажей")] public List<GameObject> previewList = new List<GameObject>();
    private int index = 0;

    private List<GameObject> displayedIcons = new List<GameObject>();
    private List<GameObject> displayedSkills = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Render();
        SetSkills();
        Redraw();
    }

    public void SetCharacter(GameObject prefab)
    {
        index = previewList.IndexOf(prefab);
        Debug.Log(index);

        Render();
        SetSkills();
        Redraw();
    }

    private void Render()
    {
        GameObject player = previewList[index];
        charachterPreview.sprite = player.GetComponent<PlayerEntity>().IdleSprite;
        charachterName.text = player.GetComponent<PlayerEntity>().GetEntityName();
    }

    private void SetSkills()
    {
        
        
        GameObject trackedPlayer = previewList[index];
        BaseEntity trackedPlayerScript = trackedPlayer.GetComponent<BaseEntity>();
        ClearDisplayedSkills();
        foreach (BaseEntity.Skill skillStruct in trackedPlayerScript.skills)
        {
            BaseSkill skill = skillStruct.skill.GetComponent<BaseSkill>();
            var skillIcon = new GameObject(name: "Skill");
            skillIcon.AddComponent<Image>().sprite = skill.GetSprite();
            skillIcon.AddComponent<TooltipTextUI>().tooltipName = skill.GetName();
            skillIcon.GetComponent<TooltipTextUI>().skill = skill;
            skillIcon.transform.SetParent(skillsPanel);
            displayedSkills.Add(skillIcon);
        }
    }

    void ClearDisplayedSkills()
    {
        for (var i = 0; i < displayedSkills.Count; i++)
        {
            Destroy(displayedSkills[i]);
        }
        displayedSkills.Clear();
    }

    void Redraw()
    {
        GameObject player = previewList[index];
        Inventory playerInventory = player.GetComponent<Inventory>();
        ClearDisplayedItems();
        for (var i = 0; i < playerInventory.inventoryItems.Count; i++)
        {
            var item = playerInventory.inventoryItems[i];

            if (item != null)
            {
                var icon = new GameObject(name: "Icon");
                icon.AddComponent<Image>().sprite = item.GetImage();
                icon.AddComponent<TooltipTextUI>().tooltipName = item.GetItemName();
                icon.GetComponent<TooltipTextUI>().item = item;
                icon.transform.SetParent(inventoryPanel);
                displayedIcons.Add(icon);
            }
        }
    }

    void ClearDisplayedItems()
    {
        for (var i = 0; i < displayedIcons.Count; i++)
        {
            Destroy(displayedIcons[i]);
        }
        displayedIcons.Clear();
    }
    
    public void PreviousCharachter()
    {
        index--;
        if (index < 0) index = previewList.Count - 1;
        if (index >= previewList.Count) index = 0;
        Render();
        SetSkills();
        Redraw();
    }

    public void NextCharachter()
    {
        index++;
        if (index < 0) index = previewList.Count - 1;
        if (index >= previewList.Count) index = 0;
        Render();
        SetSkills();
        Redraw();
    }

}
