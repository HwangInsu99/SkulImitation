using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _info;
    [SerializeField] private Image _icon;

    private StageSelectStat_SO _stat;
    private StageSelectSkul_SO _skul;

    public void OnChoice()
    {
        if (_stat != null)
        {
            Player.Instance.UpgradeStat(_stat.Stat, _stat.Value);
        }
        else
        {
            Player.Instance.AddHead(_skul.SkulPrefab);
        }
        _stat = null;
        _skul = null;
        UIManager.Instance.CloseChoice();
    }

    public void SetStat(StageSelectStat_SO stat)
    {
        _skul = null;
        _stat = stat;
        SetButtonInfo(true);
    }

    public void SetSkul(StageSelectSkul_SO skul)
    {
        _stat = null;
        _skul = skul;
        SetButtonInfo(false);
    }

    // 어떤걸 참조받을지 if로 분기
    void SetButtonInfo(bool stat)
    {
        if (stat)
        {
            _icon.sprite = _stat.Icon;
            _info.text = _stat.Info;
        }
        else
        {
            _icon.sprite = _skul.Icon;
            _info.text = _skul.Info;
        }
    }
}
