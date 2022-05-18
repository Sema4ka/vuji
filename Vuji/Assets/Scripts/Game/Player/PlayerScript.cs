using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private GameObject _gameManager;
    private PlayersTeamsManager _playersTeamsManager;
    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _playersTeamsManager = _gameManager.GetComponent<PlayersTeamsManager>();
    }

    public void KillPlayer()
    {
        var myPlayerView = gameObject.GetComponent<PhotonView>();

        Debug.Log("PLAYER DIED FROM: " + myPlayerView.Owner.GetPhotonTeam().Name);
        if (myPlayerView.Owner.GetPhotonTeam().Name == "TeamOne")
        {
            _playersTeamsManager.PlayerInTeamOneDied();
        }
        
        if (myPlayerView.Owner.GetPhotonTeam().Name == "TeamTwo")
        {
            _playersTeamsManager.PlayerInTeamTwoDied();
        }
        gameObject.SetActive(false);
        //PhotonNetwork.Destroy(gameObject);
    }
}