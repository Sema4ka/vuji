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
    [SerializeField] private List<GameObject> skills = new List<GameObject>();
    private PhotonView _view;
    public GameObject _droppedItemPrefab;


    #region Information
    [SerializeField] public HealthBarManager healthBar;
    [SerializeField] public EntityNameManager displayedName;
    #endregion
    private void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();

        maxHealthPoints = Mathf.Max(maxHealthPoints, healthPoints);
        float height = 1.0f;
        healthBar.SetOffset(new Vector3(0, height * 0.6f, 0));
        healthBar.SetHealth(healthPoints, maxHealthPoints);
        displayedName.SetOffset(new Vector3(0, height * 0.6f, 0));
        displayedName.SetText(entityName);
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
        Debug.Log("UseSkill");
        StartCoroutine(skills[skillNum].GetComponent<BaseSkill>().UseSkill(this.gameObject));
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