using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChangerCatsle : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            SceneChanger.Instance.MoveNormalStage();
        }
    }
}
