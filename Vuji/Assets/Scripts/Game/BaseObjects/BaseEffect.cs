using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    [SerializeField] public string effectName = "New Effect";
    [SerializeField] public string description = "Effect description";
    [SerializeField] public float duration = 5f;

    [SerializeField] public Sprite effectSprite;

    public virtual void ApplyEffect(GameObject entity)
    {
        Debug.Log("Apply Effect " + effectName + " on entity " + entity.name);
    }
    
}
