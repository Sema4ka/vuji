using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENABLESUKA : MonoBehaviour
{
    [SerializeField] GameObject[] allObjects;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in allObjects)
        {
            obj.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
