using UnityEngine;

public class TimeUtils : MonoBehaviour
{
    public static TimeUtils instance;
	/// <summary>
	/// Optimization: static to avoid going to c++ every Time.deltaTime call. This should be called instead.
	/// </summary>
	public static float deltaTime { get; private set; }
	/// <summary>
	/// Optimization: static to avoid going to c++ every Time.unscaledDeltaTime call. This should be called instead.
	/// </summary>
	public static float unscaledDeltaTime { get; private set; }
	/// <summary>
	/// Optimization: static to avoid going to c++ every Time.timeScale call. This should be called instead.
	/// </summary>
	public static float timeScale { get; private set; }
    public static int frameCount { get; private set; }
    public static float realtimeSinceStartup { private set; get; }
    public static float time { private set; get; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("OnBeforeSceneLoadRuntimeMethod - TimeUtils Initialize");
        if(instance)
            return;

        GameObject gObject = new GameObject("TimeUtils");
        DontDestroyOnLoad(gObject);
        instance = gObject.AddComponent<TimeUtils>();
        instance.Update();
    }
    private void Update()
    {
        deltaTime = Time.deltaTime;
        unscaledDeltaTime = Time.unscaledDeltaTime;
        timeScale = Time.timeScale;
        frameCount = Time.frameCount;
        realtimeSinceStartup = Time.realtimeSinceStartup;
        time = Time.time;
    }
    private void OnDestroy()
    {
        instance = null;
    }
}
