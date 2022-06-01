using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Вспомогательный скрипт, включает все объекты (список задается в редакторе юнити)
/// </summary>
public class ENABLESUKA : MonoBehaviour
{
    [SerializeField, Tooltip("Список объектов, которые необходимо включить")] GameObject[] allObjects;
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
