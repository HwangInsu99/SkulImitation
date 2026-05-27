using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTackle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _paramTackle = "tTackle";

    private int _hashTackle;

    private void Awake()
    {
        _hashTackle = Animator.StringToHash(_paramTackle);
    }

    public void TackleStart(bool flip)
    {
        _renderer.flipX = flip;
        _animator.SetTrigger(_hashTackle);
    }
}
