using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EScenes
{
    Title,
    Catsle,

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

    EScenes currentScene = EScenes.Title;

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

        //SoundManager.Instance.PlayBgm(stageRegistry.GetStageDataByID(NowScene()).BgmType);
        yield return new WaitForSecondsRealtime(0.5f);

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
}
