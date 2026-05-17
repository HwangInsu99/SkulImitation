using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootSystem : MonoBehaviour
{
    [SerializeField] StageRegistry_SO stage;

    private void Awake()
    {
        SO_Set();
    }

    void Start()
    {
        
    }

    void SO_Set()
    {
        stage.MakeDic();
    }
}
