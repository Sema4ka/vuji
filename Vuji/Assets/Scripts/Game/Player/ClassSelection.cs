using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelection : MonoBehaviour
{
    bool classSet = false;

    [SerializeField] int height;
    [SerializeField] GameObject classSelection;
    [SerializeField] GameObject SubclassButton;
    [SerializeField] SpawnPlayers spawnPlayers;
    #region UI Panels
    [SerializeField] private Text firstClassName;
    [SerializeField] private Text secondClassName;
    [SerializeField] private Text thirdClassName;

    [SerializeField] private RectTransform firstClass;
    [SerializeField] private RectTransform secondClass;
    [SerializeField] private RectTransform thirdClass;

    [SerializeField] private GameObject[] firstClassSubclasses;
    [SerializeField] private GameObject[] secondClassSubclasses;
    [SerializeField] private GameObject[] thirdClassSubclasses;

    [SerializeField] private RectTransform firstClassSubclassesPanel;
    [SerializeField] private RectTransform secondClassSubclassesPanel;
    [SerializeField] private RectTransform thirdClassSubclassesPanel;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        TimerManager.timerEnd += OnTimerEnd;
        firstClassSubclassesPanel.sizeDelta = new Vector2(356, firstClassSubclasses.Length * height + 50);
        secondClassSubclassesPanel.sizeDelta = new Vector2(356, secondClassSubclasses.Length * height + 50);
        thirdClassSubclassesPanel.sizeDelta = new Vector2(356, thirdClassSubclasses.Length * height + 50);

        SpawnSubclassButtons(firstClassSubclassesPanel, firstClassSubclasses);
        SpawnSubclassButtons(secondClassSubclassesPanel, secondClassSubclasses);
        SpawnSubclassButtons(thirdClassSubclassesPanel, thirdClassSubclasses);

    }

    void OnTimerEnd(bool ended)
    {
        if (ended && !classSet)
        {
            var classNum = UnityEngine.Random.Range(1, 3);
            var cls = firstClassSubclasses;
            switch (classNum)
            {
                case 1:
                    cls = firstClassSubclasses;
                    break;
                case 2:
                    cls = secondClassSubclasses;
                    break;
                case 3:
                    cls = thirdClassSubclasses;
                    break;
                default:
                    break;
            }

            SetPlayerClass(cls[UnityEngine.Random.Range(0, cls.Length)]);
        }
        Debug.Log(classSelection.activeSelf);
        classSelection.SetActive(false);
    }

    void SpawnSubclassButtons(RectTransform parent, GameObject[] prefabs)
    {
        int posY = -75;
        foreach (GameObject playerPrefab in prefabs)
        {
            var subclass = Instantiate(SubclassButton, new Vector3(0, 0, 0), Quaternion.identity);
            subclass.transform.SetParent(parent.transform, false);
            subclass.GetComponent<RectTransform>().sizeDelta = new Vector2(336, height);
            subclass.transform.localPosition = new Vector3(0, posY, 0);
            subclass.GetComponent<Button>().onClick.AddListener(() => { SetPlayerClass(playerPrefab); });
            subclass.GetComponentInChildren<Text>().text = playerPrefab.GetComponent<BaseEntity>().GetEntityName();
            // subclass.GetComponent<Image>().sprite = playerPrefab.GetComponent<Image>().sprite;
            posY -= height + 10;
        }
    }

    void SetPlayerClass(GameObject playerPrefab)
    {
        if (classSet) return;
        firstClass.gameObject.SetActive(false);
        secondClass.gameObject.SetActive(false);
        thirdClass.gameObject.SetActive(false);
        Debug.Log(playerPrefab.GetComponent<BaseEntity>().GetEntityName());
        spawnPlayers.SetPlayerObject(playerPrefab);
        classSet = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
