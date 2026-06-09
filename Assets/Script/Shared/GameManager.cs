using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UIManager _uiManager;

    private int _clearStageNum;

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
        _clearStageNum = 0;
    }
    void Update()
    {
        if (_uiManager.IsStop)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {            
            if (Time.timeScale == 0.0f)
            {
                _uiManager.CloseAll();
                StopGame(false);
            }
            else
            {
                _uiManager.MenuOpen();
                StopGame(true);
            }
        }
    }

    public void StopGame(bool stop)
    {
        Time.timeScale = stop ? 0.0f : 1.0f;
    }

    public void StageClear()
    {
        StopGame(true);
        _uiManager.OpenChoice();
    }

    public void NextStage()
    {
        StopGame(false);
        _clearStageNum++;
        if (_clearStageNum >= 5)
        {
            SceneChanger.Instance.MoveScene(EScenes.BossStage);
        }
        else
        {
            SceneChanger.Instance.MoveNormalStage();
        }
    }

    public void PlayerDie()
    {
        StopGame(true);
        _uiManager.OpenDeathMenu();
    }
}
