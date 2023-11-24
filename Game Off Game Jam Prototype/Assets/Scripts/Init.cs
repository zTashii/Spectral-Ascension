using UnityEngine;

public static class Init
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() => GameObject.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
}