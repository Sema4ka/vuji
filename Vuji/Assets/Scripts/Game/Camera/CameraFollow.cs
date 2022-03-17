using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [SerializeField] private int cameraSpeed;
    private Vector3 _playerVector;
   

    void LateUpdate()
    {
        _playerVector = player.position;
        _playerVector.z = -2;
        transform.position = Vector3.Lerp(transform.position, _playerVector, cameraSpeed * Time.deltaTime);
    }
}