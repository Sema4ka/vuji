using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressionTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<EnemyAI>().AgressionStart(other.gameObject);
        }
    }
}
