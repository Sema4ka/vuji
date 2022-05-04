using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float maxHealthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private GameObject qSkill;
    [SerializeField] private GameObject eSkill;

    private PhotonView _view;
    public GameObject _droppedItemPrefab;
    private bool isQCooldown = false;
    private bool isEColldown = false;

    #region HealthBar
    [SerializeField] HealthBarManager healthBar;
    #endregion
    private void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();

        maxHealthPoints = Mathf.Max(maxHealthPoints, healthPoints);
        float height = 1.0f;
        healthBar.SetOffset(new Vector3(0, height * 0.6f, 0));
        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    private void Update()
    {
        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    #region Public Methods

    public void AddEffect(BaseEffect effect)
    {
        effect.ApplyEffect(this);
    }

    public void UseSkill(int skillNum)
    {
        Debug.Log(isQCooldown);
        if (!isQCooldown)
            if(Input.GetMouseButton(0))
                StartCoroutine(qSkill.GetComponent<BaseSkill>().UseSkill(this.gameObject, "q"));
        if (!isEColldown)
            if(Input.GetMouseButton(1))
                StartCoroutine(eSkill.GetComponent<BaseSkill>().UseSkill(this.gameObject, "e"));
    }

    public void setIsCooldown(string key, bool value)
    {
        if(key == "q") this.isQCooldown = value;
        if(key == "e") this.isEColldown = value;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetHealthPoints()
    {
        return healthPoints;
    }

    public float GetMaxHealthPoints()
    {
        return maxHealthPoints;
    }

    public string GetEntityName()
    {
        return entityName;
    }

    public void TakeDamage(int healthDamage)
    {
        if (gameObject.CompareTag("Player"))
        {
            DamagePlayerRemote(healthDamage);
            _view.RPC("DamagePlayerRemote", RpcTarget.Others, healthDamage);
        }
        else
        {
            healthPoints -= healthDamage;
            if (healthPoints <= 0)
            {
                Death();
            }
        }
        Debug.Log(entityName + " hp is " + healthPoints);
    }

    private void Death()
    {
        DropAllItems();
        Destroy(gameObject);
    }

    private void DropAllItems()
    {
        Inventory inventory = GetComponent<Inventory>();
        var items = inventory.GetAllItems();
        foreach(BaseItem itemData in items)
        {

            Vector2 position = new Vector2(Random.Range(-3.0f, 3.0f) + gameObject.transform.position.x, Random.Range(-3.0f, 3.0f) + gameObject.transform.position.y);

            GameObject droppedItem = Instantiate(_droppedItemPrefab, position, Quaternion.identity);
            droppedItem.GetComponent<DroppedItem>().SetItem(itemData); 
        }
        inventory.ClearInventory();
    }

    [PunRPC]
    private void DamagePlayerRemote(int healthDamage)
    {
        healthPoints -= healthDamage;
        if (healthPoints <= 0)
        {
            DropAllItems();
            gameObject.GetComponent<PlayerScript>().KillPlayer();
        }
    }

    #endregion
}