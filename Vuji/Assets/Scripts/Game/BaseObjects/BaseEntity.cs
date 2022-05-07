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
    private bool _isQCooldown = false;
    private bool _isEColldown = false;
    private string _selectedSkill = "";

    #region HealthBar
    [SerializeField] HealthBarManager healthBar;
    #endregion
    private void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();
        
        // KeyHandler.keyPressed += UseSkill;

        maxHealthPoints = Mathf.Max(maxHealthPoints, healthPoints);
        float height = 1.0f;
        healthBar.SetOffset(new Vector3(0, height * 0.6f, 0));
        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    private void Update()
    {
        healthBar.SetHealth(healthPoints, maxHealthPoints);


        if (_view.IsMine){
            if (Input.GetMouseButtonDown(0))
            {
                this.UseSkill(_selectedSkill, KeyCode.Mouse0);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.UseSkill(_selectedSkill, KeyCode.Q);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.UseSkill(_selectedSkill, KeyCode.E);
            }
        }   
    }

    #region Public Methods

    public void AddEffect(BaseEffect effect)
    {
        effect.ApplyEffect(this);
    }

    public void UseSkill(string skillName, KeyCode key)
    {
        if(key == KeyCode.Q){

            if(_isQCooldown){
                Debug.Log("Q COOLDOWN");
                return;
            }
            if (_selectedSkill == "")
            {
                _selectedSkill = "q";
                Debug.Log("Selected q skill");
            } 
            else 
            {
                _selectedSkill =  "";
                Debug.Log("DeSelected q skill");
            }
        }
        if(key == KeyCode.E)
        {
            if(_isEColldown){
                Debug.Log("E COOLDOWN");
                return;
            }

            if (_selectedSkill == "")
            {
                Debug.Log("Selected e skill");
                 _selectedSkill = "e";
            }
            else 
            {
                Debug.Log("DeSelected e skill");
                _selectedSkill =  "";
            }
        }
        
        if(key == KeyCode.Mouse0)
        {
            if(_selectedSkill == "q")
            {
                Debug.Log("Used q skill");
                StartCoroutine(qSkill.GetComponent<BaseSkill>().UseSkill(this.gameObject, _selectedSkill));
            } 
            if(_selectedSkill == "e")
            {
                Debug.Log("Used e skill");
                StartCoroutine(eSkill.GetComponent<BaseSkill>().UseSkill(this.gameObject, _selectedSkill));
            }
            _selectedSkill = "";
        }
    }

    private void setCurrentSkill(string skillName)
    {
        this._selectedSkill = skillName;
    }

    public void setIsCooldown(string key, bool value)
    {
        if(key == "q") this._isQCooldown = value;
        if(key == "e") this._isEColldown = value;
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