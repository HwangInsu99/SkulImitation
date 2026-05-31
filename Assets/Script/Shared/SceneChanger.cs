using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EScenes
{
    Title,
    Catsle,
    Stage_01,
    Stage_02,
    Stage_03,
    Stage_04,
    Stage_05,
    BossStage,
}

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;

    [Serializable]
    private class SceneLib
    {
        public EScenes scene;
        public string name;
    }

    [SerializeField]
    private StageRegistry_SO stageRegistry;
    [SerializeField]
    private CanvasGroup faded;
    [SerializeField]
    private float fadeTime;

    Coroutine coroutine = null;

    [SerializeField] private EScenes currentScene = EScenes.Title;

    public event Action OnSceneChange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SoundManager.Instance.PlayBgm(stageRegistry.GetStageDataByID(NowScene()).SceneBGM);
        currentScene = stageRegistry.GetStageDataByID(SceneManager.GetActiveScene().name).SceneEnum;
    }

    public void MoveScene(EScenes target)
    {
        if (coroutine != null)
        {
            return;
        }

        currentScene = target;
        string _name = stageRegistry.GetStageDataByEnum(target);

        if (_name == null)
        {
            return;
        }
        coroutine = StartCoroutine(LoadSceneCoroutine(_name));
    }

    public void ReLoadScene()
    {
        if (coroutine != null)
        {
            return;
        }

        string _name = NowScene();

        coroutine = StartCoroutine(LoadSceneCoroutine(_name));
    }

    IEnumerator LoadSceneCoroutine(string _name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_name);


        op.allowSceneActivation = false;

        faded.alpha = 1;
        faded.blocksRaycasts = true;
        faded.interactable = true;

        float currentTime = 0;
        float t;

        //SoundManager.Instance.PlaySfx(ESfxType.Loading);

        while (currentTime < fadeTime)
        {
            currentTime += Time.unscaledDeltaTime;

            t = currentTime / fadeTime;

            t = MathF.Sin(t * MathF.PI * 0.5f);

            yield return null;
        }

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        op.allowSceneActivation = true;

        SoundManager.Instance.PlayBgm(stageRegistry.GetStageDataByID(NowScene()).SceneBGM);
        yield return new WaitForSecondsRealtime(0.5f);
        OnSceneChange?.Invoke();

        faded.alpha = 0;
        faded.blocksRaycasts = false;
        faded.interactable = false;

        ClearCoroutine();
        yield break;
    }

    void ClearCoroutine()
    {
        coroutine = null;
    }

    public string NowScene()
    {
        string nowScene = stageRegistry.GetStageDataByEnum(currentScene);

        if (nowScene == null)
        {
            return stageRegistry.GetStageDataByEnum(EScenes.Title);
        }

        return nowScene;
    }

    // 캐슬, 보스, 타이틀은 사용하지 않고 바로 무브씬
    public void MoveNormalStage()
    {
        EScenes stage;
        while (true)
        {
            stage = (EScenes)UnityEngine.Random.Range((int)EScenes.Stage_01, (int)EScenes.Stage_03 + 1);
            if (stage != currentScene)
            {
                break;
            }
        }        
        MoveScene(stage);
    }
}
