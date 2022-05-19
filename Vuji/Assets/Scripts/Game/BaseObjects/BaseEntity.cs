using System.Collections.Generic;
using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float maxHealthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float energy = 100.0f;

    //Аналог словаря для юнити инспектора
    [Serializable]
    public struct Skill
    {
        public string key;
        public GameObject skill;
    }
    public Skill[] skills;

    public GameObject _droppedItemPrefab;

    private PhotonView _view;

    private bool _isSkill1Cooldown = false;
    private bool _isSkill2Cooldown = false;
    private string _selectedSkill = "";
    private Dictionary<string, GameObject> _skills = new Dictionary<string, GameObject>();

    #region Information
    [SerializeField] public HealthBarManager healthBar;
    [SerializeField] public EntityNameManager displayedName;
    #endregion
    private void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();

        // Заполнение обычного словаря скилов из словаря из инспектора
        if (skills.Length != 0)
            for (int i = 0; i < skills.Length; i++)
            {
                Debug.Log(skills[i].key + " " + skills[i].skill);
                this._skills[skills[i].key] = skills[i].skill;
            }

        maxHealthPoints = Mathf.Max(maxHealthPoints, healthPoints);
        float height = 1.0f;
        healthBar.SetOffset(new Vector3(0, height * 0.6f, 0));
        healthBar.SetHealth(healthPoints, maxHealthPoints);
        displayedName.SetOffset(new Vector3(0, height * 0.6f, 0));
        if (_view.IsMine)
        {
            displayedName.SetText(PhotonNetwork.NickName); // Replace with Username
        }
        else
        {
            displayedName.SetText(entityName);
        }
    }

    private void Update()
    {
        // Костыль для инпутов, заменить потом на keyhandler
        if (gameObject.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.UseSkill(_selectedSkill, KeyCode.Mouse0);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.UseSkill("Skill 1", KeyCode.Q);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.UseSkill("Skill 2", KeyCode.E);
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
        // Если скилл не выбран
        if (_selectedSkill == "")
        {
            selectSkill(skillName);
        }
        else
        {
            // Если скилл использован
            if (key == KeyCode.Mouse0)
            {
                Debug.Log("Used " + skillName);
                StartCoroutine(_skills[skillName].GetComponent<BaseSkill>().UseSkill(this.gameObject, _selectedSkill));
                deSelectSkill();
            }
            // Если скилл выбран, но тот же скилл выбрали ещё раз 
            else if (_selectedSkill == skillName) deSelectSkill();
            else selectSkill(skillName);
        }
    }

    private void deSelectSkill()
    {
        Debug.Log("Deselected" + _selectedSkill);
        this._selectedSkill = "";
    }

    private void selectSkill(string skillName)
    {
        if (skillName == "")
        {
            Debug.Log("Skill is not selected");
            return;
        }

        if ((skillName == "Skill 1" && !_isSkill1Cooldown) || (skillName == "Skill 2" && !_isSkill2Cooldown))
        {
            Debug.Log("Selected " + skillName);
            this._selectedSkill = skillName;
        }
        else
        {
            Debug.Log(skillName + " COOLDOWN");
        }
    }

    public void setIsCooldown(string key, bool value)
    {
        if (key == "Skill 1") this._isSkill1Cooldown = value;
        if (key == "Skill 2") this._isSkill2Cooldown = value;
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

    public bool spendEnergy(float energyCost)
    {
        if (energyCost > this.energy) return false;

        this.energy -= energyCost;
        return true;
    }

    public void TakeDamage(int healthDamage)
    {
        healthPoints -= healthDamage;
        if (healthPoints <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                PlayerDeath();
            }
            else
            {
                EntityDeath();
            }
        }

        Debug.Log(entityName + " hp is " + healthPoints);
    }

    private void EntityDeath()
    {
        DropAllItems();
        PhotonNetwork.Destroy(gameObject);
    }

    private void PlayerDeath()
    {
        DropAllItems();
        gameObject.GetComponent<PlayerScript>().KillPlayer();
    }

    private void DropAllItems()
    {
        Inventory inventory = GetComponent<Inventory>();
        var items = inventory.GetAllItems();
        foreach (BaseItem itemData in items)
        {
            Vector2 position = new Vector2(Random.Range(-3.0f, 3.0f) + gameObject.transform.position.x,
                Random.Range(-3.0f, 3.0f) + gameObject.transform.position.y);


            GameObject droppedItem = Instantiate(_droppedItemPrefab, position, Quaternion.identity);
            droppedItem.GetComponent<DroppedItem>().SetItem(itemData);
        }
    }

    [PunRPC]
    private void TakeDamageRemote(int photonID, int newDamage)
    {
        GameObject obj = PhotonView.Find(photonID).gameObject;
        obj.GetComponent<BaseEntity>().TakeDamage(newDamage);
    }
    #endregion
}
