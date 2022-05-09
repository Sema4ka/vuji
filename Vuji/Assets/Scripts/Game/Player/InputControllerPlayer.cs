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
        string[] words = name.Split(' ');
        if (words[0] != "Skill") return;
        int skillNum = Convert.ToInt32(words[1]) - 1;
        _player.UseSkill(name, code);
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
