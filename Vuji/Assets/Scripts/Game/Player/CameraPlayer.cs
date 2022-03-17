
using UnityEngine;
using Photon.Pun;

public class CameraPlayer : MonoBehaviour
{
    private PhotonView _view;
    private void Start()
    {
        _view = GetComponent<PhotonView>();
        if (_view.Owner.IsLocal)
        {
            Camera.main.GetComponent<CameraFollow>().player = gameObject.transform;
        }
    }
}