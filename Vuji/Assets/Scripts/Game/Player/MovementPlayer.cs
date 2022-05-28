using Photon.Pun;
using UnityEngine;
using System.Collections;

public class MovementPlayer : MonoBehaviour
{
    private PhotonView _view;
    private BaseEntity _player;
    private float _moveSpeed;
    private Rigidbody2D _rb2d;

    public bool canMove = true;

    #region KeybindMovement
    private bool keybindMovement = false;
    #endregion


    private void Start()
    {
        _player = GetComponent<BaseEntity>();
        _moveSpeed = _player.GetMoveSpeed();
        _rb2d = GetComponent<Rigidbody2D>();
        _view = GetComponent<PhotonView>();
        SettingsManager.keybindMovementToggled += ToggleKeybindMovement;
    }
    private void OnDestroy()
    {
        SettingsManager.keybindMovementToggled -= ToggleKeybindMovement;
    }
    void ToggleKeybindMovement(bool isOn)
    {
        keybindMovement = isOn;
    }

    public void cancelMovement(float stopTime)
    {
        StartCoroutine(movementStopCoroutine(stopTime));
    }

    private IEnumerator movementStopCoroutine(float stopTime)
    {
        canMove = false;
        yield return new WaitForSeconds(stopTime);
        canMove = true;
    }

    private void FixedUpdate()
    {
        _moveSpeed = _player.GetMoveSpeed();
        if(!canMove) return;
        var keyHandler = KeyHandler.instance;
        if (keyHandler.IsPaused()) return;

        if (_view.IsMine)
        {
            if (!keybindMovement)
            {
                Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                _rb2d.MovePosition(_rb2d.position + movement * _moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                int velX = 0, velY = 0;
                if (Input.GetKey(keyHandler.GetKeybind("Right")))
                {
                    velX += 1;
                }
                if (Input.GetKey(keyHandler.GetKeybind("Left")))
                {
                    velX -= 1;
                }
                if (Input.GetKey(keyHandler.GetKeybind("Forward")))
                {
                    velY += 1;
                }
                if (Input.GetKey(keyHandler.GetKeybind("Backward")))
                {
                    velY -= 1;
                }
                Vector2 movement = new Vector2(velX, velY);
                _rb2d.MovePosition(_rb2d.position + movement * _moveSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
