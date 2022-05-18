using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMelee : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] int damage = 10;
    [SerializeField] float attackTimeout;

    private bool _isTimeout = false;

    private Vector3 _attackPoint;


    public void Attack(GameObject target)
    {
        if (!_isTimeout)
        {
            StartCoroutine("AttackTiemout");

            var xLen = target.transform.position.x - transform.position.x;
            var yLen = target.transform.position.y - transform.position.y;
            var xyLen = (float) (Mathf.Sqrt(Mathf.Pow(xLen, 2) + Mathf.Pow(yLen, 2)));
            var x = (xLen * attackDistance) / xyLen + transform.position.x;
            var y = (yLen * attackDistance) / xyLen + transform.position.y;

            _attackPoint = new Vector3(x, y, 0);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                GameObject enemyGameObject = enemy.transform.parent.gameObject;
                if (enemyGameObject != gameObject)
                    enemyGameObject.GetComponent<BaseEntity>().TakeDamage(damage);
            }
        }
    }

    IEnumerator AttackTiemout()
    {
        _isTimeout = true;
        yield return new WaitForSeconds(attackTimeout);
        _isTimeout = false;
    }
}