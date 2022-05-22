using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{
    [SerializeField] Slider EnergyBar;
    [SerializeField] Text EnergyBarText;
    private BaseEntity entity;
    // Start is called before the first frame update
    void Start()
    {
        EnergyBar.minValue = 0f;
        SpawnPlayers.OnSpawn += OnSpawn;
    }
    void OnSpawn(GameObject player){
        entity = player.GetComponent<BaseEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        EnergyBar.maxValue = entity.GetMaxEnergyPoints();
        EnergyBar.value = entity.GetEnergyPoints();
        EnergyBarText.text = entity.GetEnergyPoints().ToString() + "/" + entity.GetMaxEnergyPoints().ToString();
    }
    public void SetEntity(BaseEntity player){
        entity = player;
    }
}
