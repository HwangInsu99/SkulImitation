using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimit : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private BoxCollider2D _limit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            PlayerCamera cam = other.GetComponentInParent<PlayerCamera>();
            if (cam != null)
            {
                cam.SetBound(_limit);
            }
        }
    }
}
