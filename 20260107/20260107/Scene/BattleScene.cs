

public class BattleScene : Scene
{
    private Tile[,] _battleField = new Tile[10,30];
    private PlayerCharacter _player;
    private Monster _monster;
    
    public BattleScene(PlayerCharacter player, Monster monster) => Init(player, monster);

    public void Init(PlayerCharacter player, Monster monster)
    {
        _player = player;
        _monster = monster;
        for (int y = 0; y < _battleField.GetLength(0); y++)
        {
            for (int x = 0; x < _battleField.GetLength(1); x++)
            {
                Vector pos = new Vector(x, y);
                _battleField[y, x] = new Tile(pos);
            }
        }
    }
    
    public override void Enter()
    {
        if (_player.Field != null)
        {
            _player.Field[_player.Position.Y, _player.Position.X].OnTileObject = null;
        }
        _player.Field = _battleField;
        _player.Position = new Vector(5, 3);
        _battleField[_player.Position.Y, _player.Position.X].OnTileObject = _player;
        
    }

    public override void Update()
    {
        _player.Update();
        // _player.Render();
        if (InputManager.GetKey(ConsoleKey.Escape))
        {
            SceneManager.ChangePrevScene();
        }
    }

    public override void Render()
    {
        Console.Clear();
        Console.SetCursorPosition(10, 0);
        Console.WriteLine("전투 시작");
        PrintField();
        
    }

    public override void Exit()
    {
        
        
    }
    
    public void PrintField()
    {
        for (int y = 0; y < _battleField.GetLength(0); y++)
        {
            for (int x = 0; x < _battleField.GetLength(1); x++)
            {
                _battleField[y, x].Print();
            }

            Console.WriteLine();
        }
    }
}