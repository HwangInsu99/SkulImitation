using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UIManager _uiManager;

    private int _cleatStageNum;

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
        _cleatStageNum = 0;
    }
    void Update()
    {
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
        _cleatStageNum++;
        if (_cleatStageNum >= 5)
        {
            SceneChanger.Instance.MoveScene(EScenes.BossStage);
        }
        else
        {
            SceneChanger.Instance.MoveNormalStage();
        }
    }
}
