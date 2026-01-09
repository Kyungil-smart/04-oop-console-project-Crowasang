

public class BattleScene : Scene
{
    private Tile[,] _battleField = new Tile[10,30];
    private PlayerCharacter _player;
    private Monster _monster;
    private MenuList _battleMenu;
    private Tile[,] _returnField;
    private Vector _returnPosition;
    
    public bool IsActive { get; set; }
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
    }

    public override void Update()
    {
        
        if (InputManager.GetKey(ConsoleKey.UpArrow))
            _battleMenu.SelectUp();

        if (InputManager.GetKey(ConsoleKey.DownArrow))
            _battleMenu.SelectDown();

        if (InputManager.GetKey(ConsoleKey.Enter))
            _battleMenu.Select();
    }

    public override void Render()
    {
        Console.Clear();
        Console.SetCursorPosition(10, 0);
        Console.WriteLine("전투 시작");
        PrintField();
        _battleMenu.Render(_battleField.GetLength(1) / 4, _battleField.GetLength(0) -4);
        if (!IsActive) return;
        _battleMenu.Render(15, 1);
        
    }

    public override void Exit()
    {
        _player.ExitBattle();
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
        SceneManager.ChangePrevScene();
    }
    public void Select()
    {
        if (!IsActive) return;
        _battleMenu.Select();
    }

    public void SelectUp()
    {
        if (!IsActive) return;
        _battleMenu.SelectUp();
    }

    public void SelectDown()
    {
        if (!IsActive) return;
        _battleMenu.SelectDown();
    }
    
}