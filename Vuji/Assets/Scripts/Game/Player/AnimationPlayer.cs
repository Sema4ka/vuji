using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;


public class AnimationPlayer : MonoBehaviour
{
    private PhotonView _view;
    private Animator _animator;
    private string _currentAnimation = "";

    private AudioSource stepSound;
    private bool isStepPlaying;

    private float y;
    private float x;
    private bool isMoving;
    public string movingState;

    public readonly string _attack = "attack_";
    public readonly string _move = "move_";
    public readonly string _drink = "drink_";
    public readonly string _idle = "idle_";
    public readonly string _shot = "shot_";

    public readonly string _left = "left";
    public readonly string _right = "right";
    public readonly string _up = "back";
    public readonly string _down = "front";


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
        if (_view.IsMine && GetComponent<MovementPlayer>().canMove)
        {
            getCurrentMovingState();
            if (!isMoving)
            {
                ChangePlayerAnimation_q(_idle);
                //Debug.Log(_idle + movingState + "nemove");
            }
            else
            {
                ChangePlayerAnimation_q(_move);
                //Debug.Log(_move + movingState+"mofe");
                playStep();
            }
        }
        isMoving = false;
    }
    public void ChangePlayerAnimation_q(string newAnim)
    {
        if(!_animator.GetCurrentAnimatorStateInfo(-1).IsName(_attack+movingState))
            _view.RPC("ChangePlayerAnimation", RpcTarget.All, newAnim + movingState);
    }
    public string getCurrentMovingState()
    {
        y = Input.GetAxisRaw("Vertical");
        x = Input.GetAxisRaw("Horizontal");

        if (x != 0 || y != 0)
        {
            isMoving = true;
            if (y > 0)
                movingState = _up;
            else
                movingState = _down;
            if (x > 0)
                movingState = _right;
            else if (x != 0)
                movingState = _left;
        }
        return movingState;
    }

    void playStep()
    {
        if (!isStepPlaying)
        {
            stepSound.Play();
            stepSound.pitch = new System.Random().Next(80, 100) / 100;
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
    public void ChangePlayerAnimation(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;
        _currentAnimation = newAnimation;
        //Debug.Log(newAnimation+"Photon");
        _animator.Play(_currentAnimation);
    }
}