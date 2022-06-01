using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Вспомогательный скрипт, выключает все объекты (список задается в редакторе юнити)
/// </summary>
public class DISABLESUKA : MonoBehaviour
{
    [SerializeField, Tooltip("Список объектов, которые необходимо отключить")] GameObject[] allObjects;
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
