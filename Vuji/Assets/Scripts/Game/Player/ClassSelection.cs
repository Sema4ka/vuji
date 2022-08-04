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
    [SerializeField, Tooltip("Канвас для тултипа превью персонажа")] GameObject tooltipToDisable;
    [SerializeField, Tooltip("Канвас для тултипа в игре")] GameObject tooltipToEnable;
    [SerializeField, Tooltip("Скрипт управления превью")] LobbyPreviewManager previewManager;
    [SerializeField, Tooltip("Кнопка подтверждения выбора персонажа")] GameObject selectButton;
    #region UI Panels

    [SerializeField] private RectTransform linkPanel;

    [SerializeField, Tooltip("Список объектов (префабов) для подклассов первого класса")] private GameObject[] firstClassSubclasses;
    [SerializeField, Tooltip("Список объектов (префабов) для подклассов второго класса")] private GameObject[] secondClassSubclasses;
    [SerializeField, Tooltip("Список объектов (префабов) для подклассов третьего класса")] private GameObject[] thirdClassSubclasses;
    #endregion

    GameObject selectedPreview = null;
    // Start is called before the first frame update
    void Start()
    {
        selectedPreview = firstClassSubclasses[0];
        TimerManager.timerEnd += OnTimerEnd;

        SpawnSubclassButtons(linkPanel, firstClassSubclasses);
        SpawnSubclassButtons(linkPanel, secondClassSubclasses);
        SpawnSubclassButtons(linkPanel, thirdClassSubclasses);

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

            selectedPreview = cls[UnityEngine.Random.Range(0, cls.Length)];
            SetPlayerClass();
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
        foreach (GameObject playerPrefab in prefabs)
        {
            var subclass = new GameObject();
            subclass.AddComponent<Image>().sprite = playerPrefab.GetComponent<PlayerEntity>().IdleSprite;
            subclass.transform.SetParent(parent, false);
            subclass.AddComponent<Button>().onClick.AddListener(() => { PreviewPlayerClass(playerPrefab); });
        }
    }

    void PreviewPlayerClass(GameObject playerPrefab)
    {
        if (selectedPreview != playerPrefab)
        {
            previewManager.SetCharacter(playerPrefab);
            selectedPreview = playerPrefab;
        }
    }

    /// <summary>
    /// Функция установки класса/подкласса пользователя
    /// </summary>
    /// <param name="playerPrefab">Объект/префаб подкласса</param>
    public void SetPlayerClass()
    {

        if (classSet) return;
        if (!selectedPreview) return;
        linkPanel.gameObject.SetActive(false);
        Debug.Log(selectedPreview.GetComponent<BaseEntity>().GetEntityName());
        spawnPlayers.SetPlayerObject(selectedPreview);
        classSet = true;
        tooltipToDisable.SetActive(false);
        tooltipToEnable.SetActive(true);
        selectButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }


}
