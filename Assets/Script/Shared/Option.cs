using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Slider _master;
    [SerializeField] private Slider _bgm;
    [SerializeField] private Slider _sfx;

    /*
    private void Start()
    {
        _master.value = SaveData.Master;
        _bgm.value = SaveData.BGM;
        _sfx.value = SaveData.SFX;
    }
    */

    public void MasterVolume(float value)
    {
        SoundManager.Instance.MasterVolumeChange(value);
    }

    public void BgmVolume(float value)
    {
        SoundManager.Instance.BGMVolumeChange(value);
    }

    public void SfxVolume(float value)
    {
        SoundManager.Instance.SfxVolumeChange(value);
    }

    public void OptionClose()
    {
        _uiManager.OptionClose();
    }
}
