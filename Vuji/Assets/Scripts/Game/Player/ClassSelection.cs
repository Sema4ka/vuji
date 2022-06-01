using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
/// <summary>
/// Модуль управления панелью выбора класса пользователя
/// </summary>
public class ClassSelection : MonoBehaviour
{
    bool classSet = false; // Индикатор выбора класса

    [SerializeField, Tooltip("Высота каждой кнопки выбора клачча")] int height;
    [SerializeField, Tooltip("Объект панели выбора класса")] GameObject classSelection;
    [SerializeField, Tooltip("Префаб для кнопки выбора класса/подкласса")] GameObject SubclassButton;
    [SerializeField, Tooltip("Модуль появления игрока")] SpawnPlayers spawnPlayers;
    #region UI Panels
    [SerializeField, Tooltip("Поле названия первого класса")] private Text firstClassName;
    [SerializeField, Tooltip("Поле названия второго класса")] private Text secondClassName;
    [SerializeField, Tooltip("Поле названия третьего класса")] private Text thirdClassName;

    [SerializeField] private RectTransform firstClass;
    [SerializeField] private RectTransform secondClass;
    [SerializeField] private RectTransform thirdClass;

    [SerializeField, Tooltip("Список объектов (префабов) для подклассов первого класса")] private GameObject[] firstClassSubclasses;
    [SerializeField, Tooltip("Список объектов (префабов) для подклассов второго класса")] private GameObject[] secondClassSubclasses;
    [SerializeField, Tooltip("Список объектов (префабов) для подклассов третьего класса")] private GameObject[] thirdClassSubclasses;

    [SerializeField, Tooltip("Панель для привязки кнопок выбора подклассов первого класса")] private RectTransform firstClassSubclassesPanel;
    [SerializeField, Tooltip("Панель для привязки кнопок выбора подклассов второго класса")] private RectTransform secondClassSubclassesPanel;
    [SerializeField, Tooltip("Панель для привязки кнопок выбора подклассов третьего класса")] private RectTransform thirdClassSubclassesPanel;
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
    /// <summary>
    /// Фукнция для события окончания таймера выбора класса
    /// </summary>
    /// <param name="ended"></param>
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
        TimerManager.timerEnd -= OnTimerEnd;
        classSelection.SetActive(false);
    }
    /// <summary>
    /// Функция для добавления кнопок выбора класса/подкласса к целевой панели
    /// </summary>
    /// <param name="parent">Целевая панель</param>
    /// <param name="prefabs">Целевой список префабор/подклассов</param>
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
    /// <summary>
    /// Функция установки класса/подкласса пользователя
    /// </summary>
    /// <param name="playerPrefab">Объект/префаб подкласса</param>
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
