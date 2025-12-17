using UnityEngine;

public enum GameMode
{
    Normal,
    Timer,
    Health,
    Tasks
}

public class GameModeManager : MonoBehaviour
{
    public static GameMode CurrentMode = GameMode.Normal;
}
