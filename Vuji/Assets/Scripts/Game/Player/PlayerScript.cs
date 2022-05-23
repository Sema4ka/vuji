using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private BaseEntity _playerEntitiy;

    private GameObject _gameManager;
    private PlayersTeamsManager _playersTeamsManager;
    private PhotonView _view;
    

    private void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();
        _playerEntitiy = gameObject.GetComponent<BaseEntity>();

        _gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _playersTeamsManager = _gameManager.GetComponent<PlayersTeamsManager>();

        if(_view.IsMine)
            KeyHandler.keyPressed += OnKeyPressed;

        if (_view.IsMine)
            _playerEntitiy.displayedName.SetText(PhotonNetwork.NickName==""?"Player" : PhotonNetwork.NickName); // Replace with Username
        else
            _playerEntitiy.displayedName.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if (_view.IsMine) KeyHandler.keyPressed -= OnKeyPressed;
    }

    private void Update() {
        if(_view.IsMine)
            _playerEntitiy.TickPoints();
    }
 
    // Использование скиллов по кнопке
    void OnKeyPressed(string name, KeyCode key)
    {
        Debug.Log("KeyPressed" + name);
        if (name == "Attack") GetComponent<PlayerMelee>().MasterCheckMeleeAttack();
        if (name == "Use Skill") _playerEntitiy.UseSkill();
        else if (name.StartsWith("Skill")) _playerEntitiy.selectSkill(name);
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