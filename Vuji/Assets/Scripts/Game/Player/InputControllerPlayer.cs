using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControllerPlayer : MonoBehaviour
{
    private BaseEntity _player;
    
    void Start()
    {
        KeyHandler.keyPressed += OnKeyPressed;
        _player = this.gameObject.GetComponent<BaseEntity>();    
    }

    void OnKeyPressed(string name, KeyCode code)
    {
        //if (name == "Use Skill") _player.UseSkill();
        //else if (name.StartsWith("Skill")) _player.selectSkill(name);
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Input");
            _player.UseSkill(0);
        }
        */
    }
}
