using System.Collections.Generic;
using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseEntity : MonoBehaviour
{
    /// Базовая статистика энтити 
    #region Entity Stats
    [SerializeField] protected string entityName = "baseEntityName";
    [SerializeField] protected int baseDamage = 5;
    [SerializeField] protected int defense = 0;
    [SerializeField] protected float healthPoints = 100.0f;
    [SerializeField] protected float maxHealthPoints = 100.0f;
    [SerializeField] protected float moveSpeed = 3.0f;
    [SerializeField] protected float energy = 100.0f;
    [SerializeField] protected float maxEnergy = 100.0f;
    [SerializeField] protected float healthRegeneration = 1.0f;
    [SerializeField] protected float energyRegeneration = 5.0f;
    #endregion

    #region Skills
    /// Аналог словаря для юнити инспектора
    [Serializable]
    public struct Skill
    {
        public string key;
        public GameObject skill;
    }
    public Skill[] skills;
    protected Dictionary<string, GameObject> _skills = new Dictionary<string, GameObject>();

    #endregion

    #region protected fields
    /// Обязятальный префаб для выпадения предметов
    public GameObject _droppedItemPrefab;

    protected PhotonView _view;

    protected float _regenerationTick = 1;
    protected float _currentTick;

    #endregion

    #region DisplayedInformation
    [SerializeField] public HealthBarManager healthBar;
    public bool isDead { get; protected set; } = false;

    public Action<BaseEffect, BaseEntity> OnEffectApply;
    #endregion

    protected virtual void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();
        _currentTick = _regenerationTick;

        /// Заполнение обычного словаря скилов из словаря из инспектора
        if (skills.Length != 0)
            for (int i = 0; i < skills.Length; i++)
            {
                Debug.Log(skills[i].key + " " + skills[i].skill);
                this._skills[skills[i].key] = skills[i].skill;
            }

        
        maxHealthPoints = Mathf.Max(maxHealthPoints, healthPoints);
        maxEnergy = Mathf.Max(maxEnergy, energy);
        healthBar.SetOffset(new Vector3(0, 1.0f * 0.6f, 0));
        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    protected virtual void Update()
    {
        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

    protected virtual void UseSkill()
    {
        _skills["Skill 1"].GetComponent<BaseSkill>().UseSkill(this.gameObject, "Skill 1");
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

    public void AddEffect(GameObject effect)
    {
        OnEffectApply?.Invoke(effect.GetComponent<BaseEffect>(), this);
        effect.GetComponent<BaseEffect>().ApplyEffect(this.gameObject);
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


    public void IncreaseDamage(int addDamage)
    {
        this.baseDamage += addDamage;
    }

    public void DecreaseDamage(int addDamage)
    {
        this.baseDamage -= addDamage;
    }

    public void IncreaseSpeed(int addSpeed)
    {
        this.moveSpeed += addSpeed;
    }

    public void DecreaseSpeed(int addSpeed)
    {
        this.moveSpeed -= addSpeed;
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
        // Проверка на полное поглощение урона
        if(defense - healthDamage >= 0) return;

        healthPoints = healthPoints - healthDamage + defense;
        if (healthPoints <= 0)
        {
            Death();
        }

        Debug.Log(entityName + " hp is " + healthPoints);
    }
    #endregion
    /// <summary>
    /// Смерть энтити, при смерти выпадают все предметы и уничтожается объект
    /// </summary>
    public virtual void Death()
    {
        isDead = true;
        DropAllItems();
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// Выбрасываем все предметы из инвентаря под энтити
    /// </summary>
    protected void DropAllItems()
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
    /// <summary>
    /// Метод получения урона на всех клиентах
    /// </summary>
    /// <param name="photonID">Айди получателя урона</param>
    /// <param name="newDamage">Урон</param>
    protected void TakeDamageRemote(int photonID, int newDamage)
    {
        GameObject obj = PhotonView.Find(photonID).gameObject;
        obj.GetComponent<BaseEntity>().TakeDamage(newDamage);
    }
}
