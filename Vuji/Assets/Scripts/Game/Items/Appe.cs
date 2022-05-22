using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[CreateAssetMenu(fileName = "Apple", menuName = "ScriptableObject/Items/Apple")]
public class Appe : BaseItem
{
    public override bool UseItem(GameObject owner)
    {
        var _view = owner.GetComponent<PhotonView>();
        owner.GetComponent<BaseEntity>().GetHealthPoints();
        _view.RPC("Heal", RpcTarget.All, 100.0f);
        return base.UseItem(owner);
    }

}
