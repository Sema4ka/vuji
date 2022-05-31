using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DISABLESUKA : MonoBehaviour
{
    [SerializeField] GameObject[] allObjects;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in allObjects)
        {
            obj.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
