using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChanger : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            EScenes stage = (EScenes)Random.Range((int)EScenes.Stage_01, (int)EScenes.Stage_03 +1);
            SceneChanger.Instance.MoveScene(stage);
        }
    }
}
