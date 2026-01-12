

public class BattleScene : Scene
{
    private Tile[,] _battleField = new Tile[10,30];
    private PlayerCharacter _player;
    private Monster _monster;
    private MenuList _battleMenu;
    private Vector _prevPlayerPosition;
    private Tile[,] _prevPlayerField;
    
    public bool IsActive { get; set; }
    public BattleScene(PlayerCharacter player, Monster monster) => Init(player, monster);

    public void Init(PlayerCharacter player, Monster monster)
    {
        _player = player;
        _monster = monster;
        _prevPlayerPosition = _player.Position;
        _prevPlayerField = _player.Field;
        for (int y = 0; y < _battleField.GetLength(0); y++)
        {
            for (int x = 0; x < _battleField.GetLength(1); x++)
            {
                Vector pos = new Vector(x, y);
                _battleField[y, x] = new Tile(pos);
            }
        }
        DrawBattle();
        _battleMenu = new MenuList();
        _battleMenu.Add("공격", Attack);
        _battleMenu.Add("도망치기", Escape);
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
        _monster.Position = new Vector(25, 3);
        _battleField[_monster.Position.Y, _monster.Position.X].OnTileObject = _monster;
        _player.EnterBattle();
        _monster.EnterBattle();
    }

    public override void Update()
    {
        
        if (InputManager.GetKey(ConsoleKey.UpArrow))
            _battleMenu.SelectUp();

        if (InputManager.GetKey(ConsoleKey.DownArrow))
            _battleMenu.SelectDown();

        if (InputManager.GetKey(ConsoleKey.Enter))
            _battleMenu.Select();
        if (InputManager.GetKey(ConsoleKey.T))
        {
            _player.Health.Value--;
            _monster.Health.Value--;
        }
    }

    public override void Render()
    {
        Console.Clear();
        Console.SetCursorPosition(10, 0);
        Console.WriteLine("전투 시작");
        DrawBattle();
        PrintField();
        _battleMenu.Render(_battleField.GetLength(1) / 4, _battleField.GetLength(0) -4);
    }

    public override void Exit()
    {
        // _player.ExitBattle();
        _battleMenu.Reset();
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

    public void Attack()
    {
        
    }

    public void Escape()
    {
        _battleField[_player.Position.Y, _player.Position.X].OnTileObject = null;
        _battleField[_monster.Position.Y, _monster.Position.X].OnTileObject = null;
        _player.Field = _prevPlayerField;
        _player.Position = _prevPlayerPosition;
        if (_prevPlayerField != null)
        {
            _prevPlayerField[_prevPlayerPosition.Y, _prevPlayerPosition.X].OnTileObject = _player;
        }
        _player.ExitBattle();
        SceneManager.Change("Town");
    }

    public void DrawBattle()
    {
        Console.SetCursorPosition(0, 1);
        Console.Write($"{_player.Health.Value} / 100");
        Console.SetCursorPosition(0, 2);
        Console.Write("HP: ");
        Console.SetCursorPosition(4, 2);
        _player.DrawHealthGauge();
        Console.SetCursorPosition(_battleField.GetLength(1) - 9, 1);
        Console.Write($"{_monster.Health.Value} / 50");
        Console.SetCursorPosition(_battleField.GetLength(1) - 11, 2);
        Console.Write("HP: ");
        Console.SetCursorPosition(_battleField.GetLength(1) - 7, 2);
        _monster.DrawHealthGauge();
    }
}