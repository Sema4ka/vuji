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

    private bool isTimeout = false;

    private GameObject target;

    private Vector3 attackPoint;


    public void Attack(GameObject target)
    {
        if(!isTimeout)
        {
            StartCoroutine("AttackTiemout");

            var xLen = target.transform.position.x - transform.position.x;
            var yLen = target.transform.position.y - transform.position.y;
            var xyLen = (float) (Mathf.Sqrt(Mathf.Pow(xLen, 2) + Mathf.Pow(yLen, 2)));
            var x = (xLen * attackDistance) / xyLen + transform.position.x;
            var y = (yLen * attackDistance) / xyLen + transform.position.y;

            attackPoint = new Vector3(x, y, 0);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                if(enemy.gameObject != gameObject)
                    enemy.GetComponent<BaseEntity>().TakeDamage(damage);
            }

 
        }
    }

    IEnumerator AttackTiemout()
    {
        isTimeout = true;
        yield return new WaitForSeconds(attackTimeout);
        isTimeout = false;
    }
}
