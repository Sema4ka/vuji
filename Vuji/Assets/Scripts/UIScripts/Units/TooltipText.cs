using UnityEngine;
using System.Collections;
/// <summary>
/// Модуль для игровых объектов, при наведении на которых будет показана подсказка
/// </summary>
public class TooltipText : MonoBehaviour
{
    [Tooltip("Текст всплывающей подсказки")] public string text;

}