using System.Collections.Generic;
using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseEntity : MonoBehaviour
{
    #region Entity Stats
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private int baseDamage = 5;
    [SerializeField] private int defense = 0;
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float maxHealthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float energy = 100.0f;
    [SerializeField] private float maxEnergy = 100.0f;
    [SerializeField] private float healthRegeneration = 1.0f;
    [SerializeField] private float energyRegeneration = 5.0f;
    #endregion

    #region Skills
    //Аналог словаря для юнити инспектора
    [Serializable]
    public struct Skill
    {
        public string key;
        public GameObject skill;
    }
    public Skill[] skills;
    private Dictionary<string, GameObject> _skills = new Dictionary<string, GameObject>();

    public Action<string, bool> OnSkillSelectionChange;

    [SerializeField] private List<GameObject> _passiveSkills = new List<GameObject>();
    #endregion

    #region Private fields
    // Обязятальный префаб для выпадения предметов
    public GameObject _droppedItemPrefab;

    private PhotonView _view;

    private float _regenerationTick = 1;
    private float _currentTick;
    private bool _isSkill1Cooldown = false;
    private bool _isSkill2Cooldown = false;
    private string _selectedSkill = "";
    #endregion

    #region DisplayedInformation
    [SerializeField] public HealthBarManager healthBar;
    [SerializeField] public EntityNameManager displayedName;
    #endregion

    private void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();
        _currentTick = _regenerationTick;

        // Заполнение обычного словаря скилов из словаря из инспектора
        if (skills.Length != 0)
            for (int i = 0; i < skills.Length; i++)
            {
                Debug.Log(skills[i].key + " " + skills[i].skill);
                this._skills[skills[i].key] = skills[i].skill;
            }


        maxHealthPoints = Mathf.Max(maxHealthPoints, healthPoints);
        maxEnergy = Mathf.Max(maxEnergy, energy);
        float height = 1.0f;
        healthBar.SetOffset(new Vector3(0, height * 0.6f, 0));
        healthBar.SetHealth(healthPoints, maxHealthPoints);
        displayedName.SetOffset(new Vector3(0, height * 0.6f, 0));
    }

    private void Update()
    {
        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    [PunRPC]
    public void IncreaseMaxHealth(float toAdd)
    {
        maxHealthPoints += toAdd;
    }
    [PunRPC]
    public void IncreaseMaxEnergy(float toAdd)
    {
        maxEnergy += toAdd;
    }
    [PunRPC]
    public void Heal(float hp) {
        healthPoints = healthPoints + hp > maxHealthPoints ? maxHealthPoints : healthPoints + hp;
    }

    [PunRPC]
    void IncreasePoints(){
        healthPoints = healthPoints + healthRegeneration > maxHealthPoints?maxHealthPoints:healthPoints + healthRegeneration;
        energy = energy + energyRegeneration > maxEnergy?maxEnergy:energy + energyRegeneration;
    }
    
    #region Public Methods

    public void AddEffect(BaseEffect effect)
    {
        effect.ApplyEffect(this);
    }

    public void TickPoints()
    {
        _currentTick -= Time.deltaTime;
        if (_currentTick <= 0)
        {
            _view.RPC("IncreasePoints", RpcTarget.All);
            _currentTick = _regenerationTick;
        }
    }

    public void UseSkill()
    {
        if (_selectedSkill.StartsWith("Skill"))
        {
            Debug.Log("Used " + _selectedSkill);
            StartCoroutine(_skills[_selectedSkill].GetComponent<BaseSkill>().UseSkill(this.gameObject, _selectedSkill));
            deSelectSkill();
        }
        else
        {
            Debug.Log("Skill is not selected");
        }
    }

    public void selectSkill(string skillName)
    {
        if ((skillName == "Skill 1" && !_isSkill1Cooldown) || (skillName == "Skill 2" && !_isSkill2Cooldown))
        {
            Debug.Log("Selected " + skillName);
            this._selectedSkill = skillName;
            OnSkillSelectionChange?.Invoke(_selectedSkill, true);
        }
        else
        {
            Debug.Log(skillName + " COOLDOWN");
        }
    }

    public void deSelectSkill()
    {
        Debug.Log("Deselected" + _selectedSkill);
        OnSkillSelectionChange?.Invoke(_selectedSkill, false);
        this._selectedSkill = "";
    }

    public string GetSelectedSkill()
    {
        return _selectedSkill;
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

    public float GetEnergyPoints()
    {
        return energy;
    }

    public float GetMaxEnergyPoints()
    {
        return maxEnergy;
    }

    public int GetBaseDamage()
    {
        return baseDamage;
    }

    public string GetEntityName()
    {
        return entityName;
    }

    public int GetDefense()
    {
        return defense;
    }

    public void IncreaseDefense(int addDefense)
    {
        this.defense += addDefense;
    }

    public void DecreaseDefense(int addDefense)
    {
        this.defense -= addDefense;
    }

    public bool spendEnergy(float energyCost)
    {
        if (energyCost > this.energy) return false;

        this.energy -= energyCost;
        return true;
    }

    public void TakeDamage(int healthDamage)
    {
        healthPoints = healthPoints - healthDamage + defense;
        if (healthPoints <= 0)
        {
            Death();
        }

        Debug.Log(entityName + " hp is " + healthPoints);
    }
    #endregion
    private void Death()
    {
        DropAllItems();
        if(gameObject.CompareTag("Player"))
            gameObject.GetComponent<PlayerScript>().KillPlayer();
        else
            PhotonNetwork.Destroy(gameObject);
    }

    private void DropAllItems()
    {
        Inventory inventory = GetComponent<Inventory>();
        var items = inventory.GetAllItems();
        foreach (BaseItem itemData in items)
        {
            Vector2 position = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y - 0.2f);


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
    
}
