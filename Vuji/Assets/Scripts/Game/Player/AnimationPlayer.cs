using System;
using UnityEngine;
using Photon.Pun;

public class AnimationPlayer : MonoBehaviour
{
    private PhotonView _view;
    private Animator _animator;
    private string _currentAnimation = "Front";
    private const string FrontPlayerAnimation = "Front";
    private const string BackPlayerAnimation = "Back";
    private const string LeftPlayerAnimation = "Left";
    private const string RightPlayerAnimation = "Right";
    private AudioSource stepSound;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        stepSound = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (_view.IsMine)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, FrontPlayerAnimation);
                stepSound.PlayOneShot(stepSound.clip);

            }


            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, BackPlayerAnimation);
                stepSound.PlayOneShot(stepSound.clip);

            }

            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, RightPlayerAnimation);
                stepSound.PlayOneShot(stepSound.clip);

            }

            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, LeftPlayerAnimation);
                stepSound.PlayOneShot(stepSound.clip);

            }


            // возможность включить анимацию покоя
            // if (Input.GetAxisRaw("Vertical") == 0 & Input.GetAxisRaw("Horizontal") == 0){}
        }
    }

    [PunRPC]
    private void ChangePlayerAnimation(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;
        _currentAnimation = newAnimation;
        _animator.Play(_currentAnimation);
    }
}