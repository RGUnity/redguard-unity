using UnityEngine;
using UnityEngine.SceneManagement;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
        if (SceneManager.GetActiveScene().name == "Main")
        {
            SceneManager.LoadScene("Scenes/Launcher", LoadSceneMode.Additive);
        }
    }
}