using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsListManager : MonoBehaviour
{
    [SerializeField] RectTransform effectsPanel;
    [SerializeField] GameObject effectPrefab;
    private BaseEntity targetEntity;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers.OnSpawn += OnSpawn;
    }
    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
        if (targetEntity != null) targetEntity.OnEffectApply -= OnEffect;
    }

    void OnSpawn(GameObject player)
    {
        targetEntity = player.GetComponent<BaseEntity>();
        targetEntity.OnEffectApply += OnEffect;
    }

    void OnEffect(BaseEffect effect, BaseEntity entity)
    {
        if (targetEntity != entity) return;
        GameObject effectTimer = Instantiate(effectPrefab);
        effectTimer.transform.SetParent(effectsPanel, false);
        effectTimer.GetComponent<EffectTimerManager>().SetEffect(effect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
