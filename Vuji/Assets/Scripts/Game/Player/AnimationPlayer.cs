using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;


public class AnimationPlayer : MonoBehaviour
{
    private PhotonView _view;
    private Animator _animator;
    private string _currentAnimation = "Front";

    private AudioSource stepSound;
    private bool isStepPlaying;
    private float y;
    private float x;



    private bool isMoving;
    private string movingState;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        isStepPlaying = false;
        isMoving = false;
        _animator = GetComponent<Animator>();
        stepSound = GetComponent<AudioSource>();

    }
   // _view.RPC("ChangePlayerAnimation", RpcTarget.All, FrontPlayerAnimation);


    private void Update()
    {
        if (_view.IsMine)
        {
            y = Input.GetAxisRaw("Vertical");
            x = Input.GetAxisRaw("Horizontal");

            if(x!=0 || y!=0)
            {
                isMoving = true;
                if (y > 0)
                    movingState = "back";
                else
                    movingState = "front";
                if (x > 0)
                    movingState = "right";
                else if (x!=0)
                    movingState = "left";
            }
            if (!isMoving)
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, "iddle_"+movingState);
            }
            else
            {
                _view.RPC("ChangePlayerAnimation", RpcTarget.All, "move_"+movingState);
                playStep();
            }
            isMoving = false;
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