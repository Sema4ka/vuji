using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StopSearchGame : MonoBehaviour
{
    public void CancelSearchGame()
    {
        // var myTeamID = PhotonNetwork.LocalPlayer.CustomProperties["team"].ToString();
        // foreach (var player in PhotonNetwork.PlayerList)
        // {
        //     var playerTeamID = player.CustomProperties["team"].ToString();
        //     if (myTeamID == playerTeamID)
        //     {
        //         var view = PhotonView.Get(this);
        //         view.RPC("LeaveFromSearch", RpcTarget.Others, player.UserId);
        //         
        //         LeaveFromSearch(PhotonNetwork.LocalPlayer.UserId);
        //     }
        // }
    }
    [PunRPC]
    private void LeaveFromSearch(string id)
    {
        if (PhotonNetwork.LocalPlayer.UserId == id)
        {
            PhotonNetwork.LeaveRoom();
        }
        
    }
}
