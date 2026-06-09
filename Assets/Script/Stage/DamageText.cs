using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _remainScreenTime = 0.6f;
    [SerializeField] private float _riseSpeed = 0.5f;
    [SerializeField] private Camera _cam;

    private float _timer;
    private Vector3 _targetPos;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        SceneChange();
    }

    private void Start()
    {
        SceneChanger.Instance.OnSceneChange += SceneChange;
    }

    private void OnDestroy()
    {
        SceneChanger.Instance.OnSceneChange -= SceneChange;
    }

    void Update()
    {
        _timer -= Time.unscaledDeltaTime;
        if (_timer <= 0)
        {
            _timer = 0;
            ReturnPool();
            return;
        }
        if (_cam == null)
        {
            return;
        }
        _targetPos += Vector3.up * _riseSpeed * Time.deltaTime;
        transform.position = _cam.WorldToScreenPoint(_targetPos);
    }

    public void SetText(float damage, Vector3 pos)
    {
        if (_cam == null)
        {
            return;
        }
        _text.text = Mathf.RoundToInt(damage).ToString();

        Vector2 offset = Random.insideUnitCircle * 0.5f;
        offset.y = Mathf.Abs(offset.y);
        _targetPos = pos + (Vector3)offset;

        transform.position = _cam.WorldToScreenPoint(_targetPos);
        _timer = _remainScreenTime;
    }

    public void ReturnPool()
    {
        DamageTextManager.Instance.ReturnPool(this);
    }

    void SceneChange()
    {
        _cam = Camera.main;
    }
}
