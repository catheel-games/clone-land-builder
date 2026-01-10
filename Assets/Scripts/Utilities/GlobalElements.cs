public class GlobalElements : Singleton<GlobalElements>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
