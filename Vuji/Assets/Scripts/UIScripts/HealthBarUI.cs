using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Slider HealthBar;
    [SerializeField] Text HealthBarText;
    private BaseEntity entity;
    // Start is called before the first frame update
    void Start()
    {
        HealthBar.minValue = 0f;
        SpawnPlayers.OnSpawn += OnSpawn;
    }
    void OnSpawn(GameObject player){
        entity = player.GetComponent<BaseEntity>();
    }
    // Update is called once per frame
    void Update()
    {
        HealthBar.maxValue = entity.GetMaxHealthPoints();
        HealthBar.value = entity.GetHealthPoints();
        HealthBarText.text = entity.GetHealthPoints().ToString() + "/" + entity.GetMaxHealthPoints().ToString();
    }
    public void SetEntity(BaseEntity player){
        entity = player;
    }
}
