using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EntityNameManager : MonoBehaviour
{
    public Text entityName;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
        entityName.transform.position = new Vector3(temp.x, temp.y + 20, 0);
    }

    public void SendText(PhotonView _view)
    {
        _view.RPC("UpdateText", RpcTarget.All, entityName.text);
    }
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
    public Vector3 GetOffset()
    {
        return offset;
    }
    [PunRPC]
    public void UpdateText(string newText) {
        entityName.text = newText;
    }
}
