using System;
using UnityEngine;
using Photon.Pun;

public class MovementPlayer : MonoBehaviour
{
    PhotonView view;
    BaseEntity player;
    private float moveSpeed;
    private Rigidbody2D rb2d;

    private void Start()
    {
        player = GetComponent<BaseEntity>();
        moveSpeed = player.GetMoveSpeed();
        rb2d = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            rb2d.MovePosition(rb2d.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}