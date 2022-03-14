using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;


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


    public void ChangeHealthPoints(int newHealthPoint)
    {
        this.healthPoints -= newHealthPoint;
        Debug.Log("New HP: " + this.healthPoints);
    }
}