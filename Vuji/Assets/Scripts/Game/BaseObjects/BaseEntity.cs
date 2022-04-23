using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private List<BaseSkill> skills = new List<BaseSkill>();
    private PhotonView _view;

    #region Public Methods

    private void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();
    }

    public void AddEffect(BaseEffect effect)
    {
        effect.ApplyEffect(this);
    }

    public void UseSkill(BaseSkill skill)
    {
        skill.UseSkill(this);
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
            _view.RPC("DamagePlayerRemote", RpcTarget.All, healthDamage);
        }
        else
        {
            healthPoints -= healthDamage;
            if (healthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }


        Debug.Log(entityName + " hp is " + healthPoints);
    }

    [PunRPC]
    private void DamagePlayerRemote(int healthDamage)
    {
        healthPoints -= healthDamage;
        if (healthPoints <= 0)
        {
            gameObject.GetComponent<PlayerScript>().KillPlayer();
        }
    }

    #endregion
}