using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance {  get; private set; }

    [SerializeField] private Image _headIcon;
    [SerializeField] private Image _skiilIcon;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Image _hpImage;
    [SerializeField] private TMP_Text _coolText;
    [SerializeField] private Image _coolImage;
    [SerializeField] private Player _player;

    private float _cool;
    private float _maxCool;
    private float _maxHp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        _player = Player.Instance;
        _maxHp = _player.MaxHp;
        _hpText.text = _maxHp.ToString() + "/" + _maxHp.ToString();
        _player.HpChange += HpChange;
        SetCool();
    }

    void Update()
    {
        _cool = _player.CoolTime;
        if (_cool <= 0)
        {
            if (_coolImage.gameObject.activeSelf)
            {
                _coolImage.gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            if (!_coolImage.gameObject.activeSelf)
            {
                _coolImage.gameObject.SetActive(true);
            }
        }

        _coolText.text = _cool.ToString("F1");
        _coolImage.fillAmount = _cool / _maxCool;
    }

    public void HeadChange()
    {
        _skiilIcon.sprite = _player.SkillIcon;
    }

    public void ShowNextHead()
    {
        StartCoroutine(CO_Show());
    }
   
    IEnumerator CO_Show()
    {
        float time = 0f;
        float duration = 0.1f;

        RectTransform rect = _headIcon.rectTransform;

        // 1 ¡æ 0
        while (time < duration)
        {
            time += Time.deltaTime;

            float scaleX = Mathf.Lerp(1f, 0f, time / duration);
            rect.localScale = new Vector3(scaleX, 1f, 1f);

            yield return null;
        }

        rect.localScale = new Vector3(0f, 1f, 1f);

        _headIcon.sprite = _player.HeadIcon;

        time = 0f;

        // 0 ¡æ 1
        while (time < duration)
        {
            time += Time.deltaTime;

            float scaleX = Mathf.Lerp(0f, 1f, time / duration);
            rect.localScale = new Vector3(scaleX, 1f, 1f);

            yield return null;
        }

        rect.localScale = Vector3.one;
    }
    

    void HpChange(float hp)
    {
        _hpText.text = hp.ToString("F0") + "/" + _maxHp.ToString("F0");
        _hpImage.fillAmount = hp / _maxHp;
    }

    public void SetCool()
    {
        _cool = _player.CoolTime;
        _maxCool = _player.CoolTimeMax;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
