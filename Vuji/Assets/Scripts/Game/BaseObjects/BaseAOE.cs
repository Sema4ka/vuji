using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class BaseAOE : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject effect;
    [SerializeField] private string aoeName = "New Projectile";
    [SerializeField] private string aoeDescription = "";
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private GameObject senderGameObject;

    // список тегов которым должен наноситься урон и где проджектайл должен уничтожаться
    private List<string> _damageTags = new List<string>() {"Entity", "Player"};
    private List<string> _destroyTags = new List<string>() {"Entity", "Player", "Wall"};

    #endregion

    #region Public Methods

    public void SetSenderCollider(GameObject senderObject)
    {
        senderGameObject = senderObject;
    }

    public string GetAoeName()
    {
        return aoeName;
    }

    public string GetAoeDescription()
    {
        return aoeDescription;
    }

    public void SetDestroyTags(List<string> newDestroyTags)
    {
        _destroyTags = newDestroyTags;
    }

    public void SetDamageTags(List<string> newDamageTags)
    {
        _damageTags = newDamageTags;
    }


    #endregion

    void Start()
    {
        StartCoroutine(DestroyAfterLifeTime());
    }

    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D colliderObject)
    {
        GameObject enemyGameObject = colliderObject.gameObject;
        if (CanDamageThisEntity(enemyGameObject) && PhotonNetwork.IsMasterClient)
        {
            colliderObject.gameObject.GetComponent<BaseEntity>().AddEffect(effect);
        }
    }

    /// <summary>
    /// Метод проверяет:
    /// 1) Можем ли мы наносить урон этому объекту
    /// 2) Урон по себе
    /// 3) Урон игроку из своей команды
    /// </summary>
    /// <param name="enemyGameObject">Объект которому наноситсся урон</param>
    /// <returns>true - можно наносить урон; false - нельзя наносить урон</returns>
    private bool CanDamageThisEntity(GameObject enemyGameObject)
    {
        if (!_damageTags.Contains(enemyGameObject.tag))
        {
            return false;
        }

        if (enemyGameObject == senderGameObject)
        {
            return false;
        }

        if (enemyGameObject.CompareTag("Player"))
        {
            var otherPlayerView = enemyGameObject.GetComponent<PhotonView>();

            if (otherPlayerView.Owner.GetPhotonTeam().Name ==
                senderGameObject.GetComponent<PhotonView>().Owner.GetPhotonTeam().Name)
            {
                return false;
            }
        }

        return true;
    }
}