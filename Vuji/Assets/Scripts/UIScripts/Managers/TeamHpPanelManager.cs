using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHpPanelManager : MonoBehaviour
{
    [SerializeField] RectTransform targetTransform;
    [SerializeField] GameObject sliderPrefab;

    // Start is called before the first frame update
    void Start()
    {
        BaseEntity.teamSpawn += OnSpawn;
    }
    private void OnDestroy()
    {
        BaseEntity.teamSpawn -= OnSpawn;
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
