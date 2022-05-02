using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private List<GameObject> skills = new List<GameObject>();
    private PhotonView _view;
    public GameObject _droppedItemPrefab;


    #region Public Methods

    private void Start()
    {
        // _droppedItemPrefab = Resources.Load("Assets/InternalAssets/Prefabs/BaseObjects/DroppedItem") as GameObject;
        _view = gameObject.GetComponent<PhotonView>();
    }

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