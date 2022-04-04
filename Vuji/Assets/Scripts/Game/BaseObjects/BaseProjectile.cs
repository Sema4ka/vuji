using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    #region Fields

    [SerializeField] private string projectileName = "New Projectile";
    [SerializeField] private string projectileDescription = "";
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private float velocity = 1.0f;
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private bool destroyOnCollide = true;
    [SerializeField] private Vector3 aimDirection;
    [SerializeField] private float aimAngle;
    [SerializeField] private GameObject senderGameObject;

    // список тегов которым должен наноситься урон и где проджектайл должен уничтожаться
    private List<string> _damageTags = new List<string>() {"Entity", "Player"};
    private List<string> _destroyTags = new List<string>() {"Entity", "Player", "Wall"};

    private Rigidbody2D _projectileRb2d;

    #endregion

    #region Public Methods

    public void SetSenderCollider(GameObject senderObject)
    {
        senderGameObject = senderObject;
    }

    public string GetProjectileName()
    {
        return projectileName;
    }

    public string GetProjectileDescription()
    {
        return projectileDescription;
    }

    public void SetAimDirection(Vector3 newAimDirection)
    {
        aimDirection = newAimDirection;
    }

    public void SetAimAngel(float newAimAngel)
    {
        aimAngle = newAimAngel;
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
        _projectileRb2d = GetComponent<Rigidbody2D>();
        SendProjectile();
        StartCoroutine(DestroyAfterLifeTime());
    }

    void SendProjectile()
    {
        _projectileRb2d.velocity = new Vector2(aimDirection.x, aimDirection.y).normalized * velocity;
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D colliderObject)
    {
        if (_damageTags.Contains(colliderObject.gameObject.tag) & (senderGameObject != colliderObject.gameObject))
        {
            colliderObject.GetComponent<BaseEntity>().TakeDamage(projectileDamage);
        }

        // Contains как мне кажется не лучший метод для "содержиться ли элемент в массиеве"
        if (destroyOnCollide & _destroyTags.Contains(colliderObject.gameObject.tag) &
            (senderGameObject != colliderObject.gameObject))
        {
            Destroy(gameObject);
        }
    }
}