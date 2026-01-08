

public static class SceneManager
{
    public static Action OnChangeScene;
    // 현재 상태가 뭔지?
    public static Scene Current { get; private set; }
    private static Scene _prev;

    // 어떤 상태들이 있는지?
    private static Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

    public static void AddScene(string key, Scene scene)
    {
        if (_scenes.ContainsKey(key)) return;
        _scenes.Add(key, scene);
    }

    public static void ChangePrevScene()
    {
        Change(_prev);
    }

    public static void Change(string key)
    {
        if (!_scenes.ContainsKey(key)) return;
        Change(_scenes[key]);
        
    }
    
    // 상태 바꾸는 기능
    public static void Change(Scene scene)
    {
        Scene next = scene;
        
        if (Current == next) return;
        
        Current?.Exit();
        next.Enter();
        
        _prev = Current;
        Current = next;
        OnChangeScene?.Invoke();
    }

    public static void Update()
    {
        Current?.Update();
    }

    public static void Render()
    {
        Current?.Render();
    }
}