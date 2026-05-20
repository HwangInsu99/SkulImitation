using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerController _controller;
    [SerializeField] private List<Skul> _skulList;

    private float _speed = 5f;
    private float _dashSpeed = 15f;
    private float _attack = 5f;
    private float _armor;
    private float _hp;
    private float _maxHp;
    private int _currentSkul = 0;
    private int _changeSkul = 0;

    public float Speed => _speed;
    public float DashSpeed => _dashSpeed;
    public float Attack => _attack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

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
            SkulChange();
        }
    }

    void SkulChange()
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

    public void Damaged(float damage)
    {
        Debug.Log("공격받음");
    }

    void SceneChange()
    {
        transform.position = new Vector3(-9, -2, 0);        
    }
}
