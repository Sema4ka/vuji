using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Модуль управления панелью списка участников команды
/// </summary>
public class TeamHpPanelManager : MonoBehaviour
{
    [SerializeField, Tooltip("Панель для отображения хп игроков команды")] RectTransform targetTransform;
    [SerializeField, Tooltip("Префаб слайдера для отображения хп игрока команды")] GameObject sliderPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PlayerEntity.teamSpawn += OnSpawn;
    }
    private void OnDestroy()
    {
        PlayerEntity.teamSpawn -= OnSpawn;
    }
    void OnSpawn(BaseEntity player, string name)
    {
        GameObject slider = Instantiate(sliderPrefab);
        slider.transform.SetParent(targetTransform);
        slider.GetComponent<TeamHPBarUI>().SetEntity(player, name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
