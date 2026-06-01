using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerController _controller;
    [SerializeField] private List<Skul> _skulList;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _dashSpeed = 15f;
    private float _attack = 5f;
    private float _armor = 5;
    private float _hp;
    private float _maxHp = 100;
    private int _currentSkul = 0;
    private int _changeSkul = 0;

    public float Speed => _speed;
    public float DashSpeed => _dashSpeed;
    public float Attack => _attack;
    public float CoolTime => _skulList[_currentSkul].SkillCool;
    public float CoolTimeMax => _skulList[_currentSkul].SkillCoolMax;
    public Sprite HeadIcon => _skulList[_currentSkul].HeadIcon;
    public Sprite SkillIcon => _skulList[_currentSkul].SkillIcon;
    public float MaxHp => _maxHp;
    public event Action<float> HpChange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        SceneChanger.Instance.OnSceneChange -= SceneChange;
    }

    void Start()
    {
        _hp = _maxHp;
        _controller.SetSkul(_skulList[_currentSkul]);
        _skulList[_currentSkul].gameObject.SetActive(true);
        SceneChanger.Instance.OnSceneChange += SceneChange;
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
            SkulChange(_changeSkul);
        }
    }

    public void SkulChange(int change)
    {
        if (change == _currentSkul || !_controller.CanChange())
        {
            return;
        }
        _skulList[_currentSkul].ChangeSkul();
        _skulList[_currentSkul].gameObject.SetActive(false);
        _skulList[change].gameObject.SetActive(true);
        _skulList[change].SkulFlip(_skulList[_currentSkul].Flip);
        _controller.SetSkul(_skulList[change]);
        _currentSkul = change;
        // 폭탄병의 자폭으로 인해 교체되는 경우가 있으므로 초기화
        _changeSkul = change;
    }

    public void Damaged(float damage)
    {
        _hp -= damage - (_armor * _skulList[_currentSkul].SkulArmor);
    }

    void SceneChange()
    {
        transform.position = new Vector3(-9, -2, 0);        
    }

    public void ChangeStat(EPlayerStat stat, float value)
    {
        switch (stat)
        {
            case EPlayerStat.Hp:
                _hp += value;
                if (_hp >= _maxHp)
                {
                    _hp = _maxHp;
                }
                break;
            case EPlayerStat.Attack:
                _attack *= value;
                break;
            case EPlayerStat.Armor:
                _armor *= value;
                break;
            case EPlayerStat.Speed:
                _speed *= value;
                _dashSpeed *= value;
                break;
        }
    }

    public void AddHead(Skul newHead)
    {
        _skulList.Add(newHead);
        newHead.gameObject.SetActive(false);
    }
}