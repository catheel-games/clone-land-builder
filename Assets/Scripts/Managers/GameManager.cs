using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
    }

    public void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
}
