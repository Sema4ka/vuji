using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviourPunCallbacks
{
    #region Fields

    [SerializeField] private Text teamOneText;
    [SerializeField] private Text teamTwoText;
    [SerializeField] private GameObject gameManager;
    private PhotonTeam _teamOne, _teamTwo;
    private PhotonTeamsManager _teamsManager;

    #endregion


    #region LeaveFromGame

    public void LeaveRoomButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    #endregion

    #region Unity Methods

    private void Update()
    {
        _teamsManager = gameManager.GetComponent<PhotonTeamsManager>();
        _teamOne = _teamsManager.GetAvailableTeams()[0];
        _teamTwo = _teamsManager.GetAvailableTeams()[1];
        teamOneText.text = "Team 1:\n";
        teamTwoText.text = "Team 2:\n";
        SetTextAboutTeams(teamOneText,
            new[]
            {
                _teamOne.Name,
                _teamOne.Code.ToString(),
                _teamsManager.GetTeamMembersCount(_teamOne.Code).ToString()
            });
        SetTextAboutTeams(teamTwoText,
            new[]
            {
                _teamTwo.Name,
                _teamTwo.Code.ToString(),
                _teamsManager.GetTeamMembersCount(_teamTwo.Code).ToString()
            });
    }

    private void SetTextAboutTeams(Text teamText, string[] args)
    {
        string info = "";
        foreach (string arg in args)
        {
            Debug.Log("ARG: " + arg);
            info += arg;
            info += "\n";
        }

        teamText.text += info;
    }

    #endregion
}