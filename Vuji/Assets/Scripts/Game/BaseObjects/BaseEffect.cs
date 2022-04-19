using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    public string effectName = "New Effect";
    public string description = "Effect description";

    public float duration = 5f;

    public virtual void ApplyEffect(BaseEntity entity)
    {
        Debug.Log("Apply Effect " + effectName + " on entity " + entity.name);
    }
    
}
