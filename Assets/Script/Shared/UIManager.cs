using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Option _optionPanel;
    [SerializeField] private GameObject _menu;

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

    public void MenuOpen()
    {
        _menu.SetActive(true);
    }

    public void MenuClose()
    {
        _menu.SetActive(false);
    }

    public void OptionOpen()
    {
        _optionPanel.gameObject.SetActive(true);
    }

    public void OptionClose()
    {
        _optionPanel.gameObject.SetActive(false);
    }

    public void CloseAll()
    {
        OptionClose();
        MenuClose();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
