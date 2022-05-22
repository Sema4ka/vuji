using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;


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
    private bool isStepPlaying;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        isStepPlaying = false;
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
                playStep();
            }


            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, BackPlayerAnimation);
                playStep();
            }

            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, RightPlayerAnimation);
                playStep();
            }

            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, LeftPlayerAnimation);
                playStep();
            }


            // возможность включить анимацию покоя
            // if (Input.GetAxisRaw("Vertical") == 0 & Input.GetAxisRaw("Horizontal") == 0){}
        }
    }

    void playStep()
    {
        if (!isStepPlaying)
        {
            stepSound.Play();
            StartCoroutine(stepDelay(0.84f));
        }
    }

    private IEnumerator stepDelay(float delay)
    {
        isStepPlaying=true;
        yield return new WaitForSeconds(delay);
        isStepPlaying = false;
    }
    [PunRPC]
    private void ChangePlayerAnimation(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;
        _currentAnimation = newAnimation;
        _animator.Play(_currentAnimation);
    }
}