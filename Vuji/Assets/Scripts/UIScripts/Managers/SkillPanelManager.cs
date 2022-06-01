using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Модуль для управления панелью скилов сущности
/// </summary>
public class SkillPanelManager : MonoBehaviour
{
    [SerializeField, Tooltip("Префаб отображаемого скила")] GameObject timerPrefab;
    [SerializeField, Tooltip("Панель для привязки отображаемых скилов")] RectTransform skillPanel;
    public GameObject trackedPlayer;
    private BaseEntity trackedPlayerScript;

    private List<TimerWithSpritemanager> displayedSkills = new List<TimerWithSpritemanager>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers.OnSpawn += OnSpawn;
    }

    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
    }
    /// <summary>
    /// Функция для события появления игока
    /// </summary>
    /// <param name="player">объект игрока</param>
    void OnSpawn(GameObject player)
    {
        trackedPlayer = player;
        trackedPlayerScript = player.GetComponent<BaseEntity>();
        foreach (BaseEntity.Skill skill in trackedPlayerScript.skills)
        {
            GameObject timer = Instantiate(timerPrefab);
            timer.transform.SetParent(skillPanel, false);
            timer.GetComponent<TimerWithSpritemanager>().SetEntity(trackedPlayer, skill.skill.GetComponent<BaseSkill>(), skill.key);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
