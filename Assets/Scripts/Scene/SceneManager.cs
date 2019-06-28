using UnityEngine;
using System.Collections;

public enum SCENE
{
    NONE = -1,
    LogoScene = 0,
    MainScene,
    GameScene,
}

public class SceneManager : MonoSinglton<SceneManager>
{
    AsyncOperation async = null;
    public SCENE scene = SCENE.LogoScene;
    bool bInit = false;

    public static void Create()
    {
        if (I == null)
        {
            GameObject.Instantiate(Resources.Load("SceneManager"));
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = GameOption.Frame;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    public override void Constructor()
    {
        base.Constructor();

        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);

        Application.logMessageReceived += LogCallback;
    }

    public void Init()
    {
        if (bInit)
            return;

        bInit = true;

        UserDataFile.I.Init();
        TBLManager.I.Init();
        DeckManager.I.Init();
        MissionManager.I.Init();
        AchievementManager.I.Init();
        StageManager.I.ApplyStage(StageManager.I.SelectStageIndex);
        UnitylapManager.I.Init();
    }

    //
    // 요약:
    //     Use this delegate type with Application.logMessageReceived or Application.logMessageReceivedThreaded
    //     to monitor what gets logged.
    //
    // 매개 변수:
    //   condition:
    //
    //   stackTrace:
    //
    //   type:
    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
#if USE_DEV
			var popup = GlobalUI.GetUI<ErrorOk>(UI_TYPE.ErrorOK);

            if( popup )
            {
                popup.OnEnter();
                popup.Set( stackTrace );
            }
            else
            {
                Debug.LogError( stackTrace );
            }			
#endif
        }
    }

    public void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

#if !USE_PATCH
        if (Input.GetKeyUp(KeyCode.A))
        {
            GameScene.bDebugMode = !GameScene.bDebugMode;
        }
#endif 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (scene != SCENE.GameScene)
            {
                GlobalUI.I.OnBackButton();
            }
            else
            {
                if (GlobalUI.I.OnInGameBackButton() == false)
                    GameScene.I.OnBack();
            }

        }
    }


    float deltaTime = 0.0f;


    public void OnGUI()
    {

#if USE_DEV || UNITY_EDITOR
        float fps = 1.0f / deltaTime;
        GUILayout.Label(" " + fps.ToString());

        if (GameScene.bDebugMode)
            GUILayout.Label("DEBUG MODE ON ");
#endif

    }

    void Start()
    {
        //InventoryManager.I.0TestSetCard();

        ItemManager.I.TestItem();

        if (scene == SCENE.LogoScene)
        {

        }
    }
    public void ApplicationQuit()
    {
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    public override void ClearAll()
    {
        GlobalUI.I.ClearBackList();
    }

    public void ChangeScene(string SceneName)
    {
        ClearAll();

        if (SceneName == "MainScene")
        {
            InvenCardObjectPool.Apply();
            scene = SCENE.MainScene;
        }
        else if (SceneName == "StartScene")
        {
            InvenCardObjectPool.PushAll();
            scene = SCENE.LogoScene;
        }
        else if (SceneName == "GameScene")
        {
            InvenCardObjectPool.PushAll();

            //if( scene != SCENE.GameScene )
            GlobalUI.ShowUI(UI_TYPE.InGameLoadingUI);
            scene = SCENE.GameScene;
        }

        Debug.Log("load scene start : " + SceneName);

        StartCoroutine("StartLoad", (string)SceneName);
    }

    public bool IsLoading()
    {
        if (async != null && async.isDone == false)
            return true;

        return false;
    }

    public GameObject GetScene()
    {
        if (scene == SCENE.MainScene)
        {
            return GameObject.Find("[Main]");
        }
        else if (scene == SCENE.LogoScene)
        {
            return GameObject.Find("[Start]");
        }
        else if (scene == SCENE.GameScene)
        {
            return GameObject.Find("[Game]");
        }

        return null;
    }

    float timer = 0;
    float progress = 0;
    public IEnumerator StartLoad(string SceneName)
    {
        if (scene != SCENE.GameScene)
            GlobalUI.ShowUI(UI_TYPE.LoadingUI);
        timer = 0;
        progress = 0;
        async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);

        if (async == null)
            yield return null;

        if (SceneName == "GameScene")
            async.allowSceneActivation = false;

        //yield return async.progress;
        while (async.isDone == false)
        {
            yield return null;

            if (SceneName == "GameScene")
            {
                timer += Time.deltaTime;
                if (async.progress >= 0.9f)
                {
                    progress = Mathf.Lerp(progress, 1f, timer);

                    if (progress == 1.0f)
                        async.allowSceneActivation = true;
                }
                else
                {
                    progress = Mathf.Lerp(progress, async.progress, timer);
                    if (progress >= async.progress)
                    {
                        timer = 0f;
                    }

                }

                LoadingUI ui = GlobalUI.GetUI<LoadingUI>(UI_TYPE.InGameLoadingUI);
                ui.SetProgress(progress);
            }
        }

        GlobalUI.CloseUI(UI_TYPE.LoadingUI);

    }
}
