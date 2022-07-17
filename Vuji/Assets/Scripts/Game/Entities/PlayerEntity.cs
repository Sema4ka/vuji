using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;


/// <summary>
/// Класс игрока, наследуемый от базовой сущности.
/// </summary>
public class PlayerEntity : BaseEntity
{
    protected PhotonView _view;

    private GameObject _gameManager;
    private PlayersTeamsManager _playersTeamsManager;    

    public Action<string, bool> OnSkillSelectionChange;
    
    [SerializeField] public EntityNameManager displayedName;
    public static Action<BaseEntity, string> teamSpawn;
    public Controllers _controller;

    protected bool _isSkill1Cooldown = false;
    protected bool _isSkill2Cooldown = false;
    protected string _selectedSkill = "";

    protected override void Start()
    {
        base.Start();

        _view = gameObject.GetComponent<PhotonView>();

        _gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _playersTeamsManager = _gameManager.GetComponent<PlayersTeamsManager>();

        if(_view.IsMine)
            KeyHandler.keyPressed += OnKeyPressed;

        displayedName.SetOffset(new Vector3(0, 1.0f * 0.6f, 0));
        if (_view.IsMine)
        {
            _view.RPC("UpdateText", RpcTarget.All, "[" + PhotonNetwork.LocalPlayer.GetPhotonTeam().Name + "] ", PhotonNetwork.LocalPlayer.NickName);
        }
    }

    protected override void Update()
    {
        base.Update();
        


        if(_view.IsMine)
            TickPoints();
    }

    /// <summary>
    /// Перегрузка метода наложения эффекта для отображения эффекта на экране у игрока
    /// </summary>
    /// <param name="effect"></param>
    public override void AddEffect(GameObject effect)
    {
        base.AddEffect(effect);
        OnEffectApply?.Invoke(effect.GetComponent<BaseEffect>(), this);
    }

    /// <summary>
    /// Метод увеличивающий значение энергии и здоровья каждый тик. 
    /// </summary>
    public void TickPoints()
    {
        _currentTick -= Time.deltaTime;
        if (_currentTick <= 0)
        {
            _view.RPC("IncreasePoints", RpcTarget.All);
            _currentTick = _regenerationTick;
        }
    }
    
    /// <summary>
    /// Продолжение TickPoints.
    /// </summary>
    [PunRPC]
    void IncreasePoints(){
        healthPoints = healthPoints + healthRegeneration > maxHealthPoints?maxHealthPoints:healthPoints + healthRegeneration;
        energy = energy + energyRegeneration > maxEnergy?maxEnergy:energy + energyRegeneration;
    }

    /// <summary>
    /// Использование способности игроком. При активации выполняется логика прописанная в способности.
    /// </summary>
    protected override void UseSkill()
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

    /// <summary>
    /// Выбор скилла для последующего использования. Реализована механика переключения и снятия выбора.
    /// </summary>
    /// <param name="skillName"></param>
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

    /// <summary>
    /// Снятие выбора скилла
    /// </summary>
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

    /// <summary>
    /// Метод устанавливающий задержку на использование скилла
    /// Вызывается из использованного скилла
    /// </summary>
    /// <param name="key"> Название скилла </param>
    /// <param name="value"> Значиение true/false </param>
    public void setIsCooldown(string key, bool value)
    {
        if (key == "Skill 1") this._isSkill1Cooldown = value;
        if (key == "Skill 2") this._isSkill2Cooldown = value;
    }

    private void OnDestroy()
    {
        if (_view.IsMine) KeyHandler.keyPressed -= OnKeyPressed;
    }


 
    /// <summary>
    /// Функция привязанная к Action из KeyHandler для считывания нажатия кнопки скилла
    /// </summary>
    /// <param name="name">Название привязанное к кнопке</param>
    /// <param name="key">KeyCode кнопки</param>
    void OnKeyPressed(string name, KeyCode key)
    {
        Debug.Log("KeyPressed" + name);
        if (name == "Attack")
        {
            if(GetEntityName() == "Pirate") GetComponent<PlayerMelee>().MasterCheckMeleeAttack();
            if(GetEntityName() == "Mage") gameObject.GetComponent<PlayerProjectile>().Attack("Fireball", 1.5f);
            AnimationPlayer _anim = GetComponent<AnimationPlayer>();
            _anim.ChangePlayerAnimation_q(_anim._attack);
        }
        if (name == "Use Skill") UseSkill();
        else if (name.StartsWith("Skill"))
        {
            var current = GetSelectedSkill();
            deSelectSkill();
            if (current != name)
            {
                selectSkill(name);
            }
        }
    }

    /// <summary>
    /// Обновление текста над игроком
    /// </summary>
    /// <param name="teamTag">Команда игрока</param>
    /// <param name="newText">Новый текст для подстановки</param>
    [PunRPC]
    public void UpdateText(string teamTag, string newText)
    {
        GetComponentInChildren<Text>().text =  teamTag + newText;
        if ("[" + PhotonNetwork.LocalPlayer.GetPhotonTeam().Name + "] " == teamTag) teamSpawn?.Invoke(this, newText);
    }

    /// <summary>
    /// Метод для убийства имено игрока, т.к. используется фотон
    /// </summary>
    public override void Death()
    {
        Debug.Log("PLAYER DIED FROM: " + _view.Owner.GetPhotonTeam().Name);
        if (_view.Owner.GetPhotonTeam().Name == "TeamOne")
        {
            _playersTeamsManager.PlayerInTeamOneDied();
        }
        
        if (_view.Owner.GetPhotonTeam().Name == "TeamTwo")
        {
            _playersTeamsManager.PlayerInTeamTwoDied();
        }
        gameObject.SetActive(false);
        //PhotonNetwork.Destroy(gameObject);
    }        
}