using System;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Waiting end game");
    }

    public void TeamOneWin()
    {
        Debug.Log("TEAM ONE WIN");
    }
    public void TeamTwoWin()
    {
        Debug.Log("TEAM TWO WIN");
    }
}