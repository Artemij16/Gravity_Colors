using UnityEngine;

public enum GameMode
{
    Normal,
    Timer,
    Health,
    Tasks
}

public class GameModeManager
{
    public static GameMode CurrentMode = GameMode.Normal;
}
