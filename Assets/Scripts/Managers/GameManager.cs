using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
    }

    public void SetFPS(bool sixtyFps)
    {
        Application.targetFrameRate = sixtyFps ? 60 : 30;
    }
}
