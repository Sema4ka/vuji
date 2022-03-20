using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwingAttack : MonoBehaviour
{

    public GameObject hitMarker;
    // Типо тегов, только можно выбирать несколько
    public LayerMask enemyLayers;

    public float attackDistance = 1f;
    public float attackRange = 1f;

    private Vector3 attackPoint;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        //Получаем позицию курсора
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mouseWorldPosition.z = 0;

        // Получаем координату точки удара
        float x_len = mouseWorldPosition.x - this.transform.position.x;
        float y_len = mouseWorldPosition.y - this.transform.position.y;

        float xy_len = (float)(Math.Sqrt(Math.Pow(x_len, 2) + Math.Pow(y_len, 2)));  

        float x = (x_len * attackDistance) / xy_len + this.transform.position.x;
        float y = (y_len * attackDistance) / xy_len + this.transform.position.y;

        attackPoint = new Vector3(x, y, 0);

        Debug.Log("mouseWorldPosition: " + mouseWorldPosition);
        Debug.Log("this.transform.position: " + this.transform.position);
        Debug.Log("attackPoint: " + attackPoint);

        Instantiate(hitMarker, attackPoint, Quaternion.identity);

        // Получаем массив врагов которые попали под удар
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Damaged " + enemy.GetComponent<BaseEntity>().name + " by 10 damage");
            enemy.GetComponent<BaseEntity>().TakeDamage(10);
        } 

    }

 
    // Подсветка выделения радиуса удара для эдитора
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint, attackRange);
        Gizmos.DrawLine(this.transform.position, attackPoint);
    }
}
