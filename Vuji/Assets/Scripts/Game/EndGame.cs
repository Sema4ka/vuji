using System;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public static Action<string> OnGameEnd;

    private void Start()
    {
        Debug.Log("Waiting end game");
    }

    public void TeamOneWin()
    {
        Debug.Log("TEAM ONE WIN");
        OnGameEnd?.Invoke("TeamOne");
    }
    public void TeamTwoWin()
    {
        Debug.Log("TEAM TWO WIN");
        OnGameEnd?.Invoke("TeamTwo");
    }
}
