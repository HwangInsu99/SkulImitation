using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Option _optionPanel;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _deathMenu;
    [SerializeField] private ChoiceData _choice;
    private bool _isStop;

    public bool IsStop => _isStop;

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
        GameManager.Instance.StopGame(false);
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

    public void OpenChoice()
    {
        _choice.StageClear();
        _choice.gameObject.SetActive(true);
        _isStop = true;
    }

    public void CloseChoice()
    {
        _choice.gameObject.SetActive(false);
        GameManager.Instance.NextStage();
        _isStop = false;
    }

    public void OpenDeathMenu()
    {
        _deathMenu.SetActive(true);
        _isStop = true;
    }

    public void ReturnCatsle()
    {
        _deathMenu.SetActive(false);
        GameManager.Instance.StopGame(false);
        SceneChanger.Instance.MoveScene(EScenes.Catsle);
        _isStop = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
