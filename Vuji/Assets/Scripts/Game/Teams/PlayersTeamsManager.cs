using System;
using UnityEngine;
using GameSettings;

public class PlayersTeamsManager : MonoBehaviour
{
    private int _lifePlayersInTeamOne;
    private int _lifePlayersInTeamTwo;
    private EndGame _endGame;
    private void Start()
    {
        _endGame = gameObject.GetComponent<EndGame>();
        _lifePlayersInTeamOne = GameSettingsOriginal.MaxPlayersInGame / 2;
        _lifePlayersInTeamTwo = GameSettingsOriginal.MaxPlayersInGame / 2;
    }
    
    public void PlayerInTeamOneDied()
    {
        _lifePlayersInTeamOne -= 1;
        if (_lifePlayersInTeamOne == 0)
        {
            _endGame.TeamOneWin();
        }
    }
    
    public void PlayerInTeamTwoDied()
    {
        _lifePlayersInTeamTwo -= 1;
        if (_lifePlayersInTeamTwo == 0)
        {
            _endGame.TeamTwoWin();
        }
    }

    
}