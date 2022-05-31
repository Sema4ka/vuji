using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanelManager : MonoBehaviour
{
    [SerializeField] GameObject timerPrefab;
    [SerializeField] RectTransform skillPanel;
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

    void OnSpawn(GameObject player)
    {
        trackedPlayer = player;
        trackedPlayerScript = player.GetComponent<BaseEntity>();
        foreach (BaseEntity.Skill skill in trackedPlayerScript.skills) {
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
