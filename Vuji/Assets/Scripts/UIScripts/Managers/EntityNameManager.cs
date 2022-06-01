using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// Модуль для упраления и корректировки позиции имени над сущностью
/// </summary>
public class EntityNameManager : MonoBehaviour
{
    [Tooltip("Целевое текстовое поле")] public Text entityName; // Целевое текстовое поле

    private Vector3 offset; // Отклонение от позиции сущности
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Изменение позиции текста на экране
    /// </summary>
    void Update()
    {
        Vector3 temp = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
        entityName.transform.position = new Vector3(temp.x, temp.y + 20, 0);
    }
    /// <summary>
    /// Изменение сохраненного отклонения от позиции сущности
    /// </summary>
    /// <param name="newOffset">Новое отклонение</param>
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Сохраненное отклонение от позиции сущности</returns>
    public Vector3 GetOffset()
    {
        return offset;
    }

}
