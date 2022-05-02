using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControllerPlayer : MonoBehaviour
{
    private BaseEntity _player;
    
    void Start()
    {
        _player = this.gameObject.GetComponent<BaseEntity>();    
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Input");
            _player.UseSkill(0);
        }
    }
}
