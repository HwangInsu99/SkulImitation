using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private PlayerController _controller;
    [SerializeField] private List<Skul> _skulList;

    private float _speed = 5f;
    private float _dashSpeed = 15f;
    private float _attack;
    private float _armor;
    private float _hp;
    private float _maxHp;
    private int _currentSkul = 0;
    private int _changeSkul = 0;

    public float Speed => _speed;
    public float DashSpeed => _dashSpeed;

    private void Awake()
    {
        Skul[] skuls = GetComponentsInChildren<Skul>(true);

        foreach (Skul skul in skuls)
        {
            skul.gameObject.SetActive(false);
            _skulList.Add(skul);
        }
        if (_controller == null)
        {
            _controller = GetComponent<PlayerController>();
        }
    }

    void Start()
    {
        _controller.SetSkul(_skulList[_currentSkul]);
        _skulList[_currentSkul].gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _changeSkul--;
            if (_changeSkul < 0)
            {
                _changeSkul = _skulList.Count - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _changeSkul++;
            if (_changeSkul >= _skulList.Count)
            {
                _changeSkul = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_changeSkul == _currentSkul)
            {
                return;
            }
            _skulList[_currentSkul].gameObject.SetActive(false);
            _skulList[_changeSkul].gameObject.SetActive(true);
            _skulList[_changeSkul].SkulChange(_skulList[_currentSkul].Flip);
            _controller.SetSkul(_skulList[_changeSkul]);
            _currentSkul = _changeSkul;
        }
    }

    public void Damaged(float damage)
    {

    }
}
