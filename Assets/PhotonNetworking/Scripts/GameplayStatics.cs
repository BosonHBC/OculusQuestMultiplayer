using UnityEngine;

public class GameplayStatics
{
    public static int MAX_PLAYER_IN_ROOM = 3;
    public static string MAIN_SCENE_NAME = "Main";

    public static void Log(string i_logText)
    {
        if (OVRManager.hasVrFocus)
            OVRDebugConsole.Log(i_logText);
        else
        {
            Debug.Log(i_logText);
        }
    }
    public static void LogError(string i_logText)
    {
        if (OVRManager.hasVrFocus)
            OVRDebugConsole.Log("[ERROR]: " + i_logText);
        else
        {
            Debug.LogError(i_logText);
        }
    }


}

