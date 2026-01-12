

public class BattleScene : Scene
{
    private Tile[,] _battleField = new Tile[10,30];
    private PlayerCharacter _player;
    private Monster _monster;
    private MenuList _battleMenu;
    private Vector _prevPlayerPosition;
    private Tile[,] _prevPlayerField;
    private BattleTurn _battleTurn;
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
        _battleTurn = BattleTurn.pTurn;
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
        if (_battleTurn == BattleTurn.mTurn)
        {
            mAttack();
            return;
        }
        if (_battleTurn == BattleTurn.pTurn)
        {
            if (InputManager.GetKey(ConsoleKey.UpArrow))
                _battleMenu.SelectUp();

            if (InputManager.GetKey(ConsoleKey.DownArrow))
                _battleMenu.SelectDown();

            if (InputManager.GetKey(ConsoleKey.Enter))
                _battleMenu.Select();
        }
    }

    public override void Render()
    {
        Console.Clear();
        Console.SetCursorPosition(10, 0);
        Console.WriteLine("전투 시작");
        DrawBattle();
        PrintField();
        Console.SetCursorPosition(6, 3);
        if (_battleTurn == BattleTurn.pTurn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("【 플레이어 턴 】");
            Console.ResetColor();
            _battleMenu.Render(_battleField.GetLength(1) / 4, _battleField.GetLength(0) -4);
        }
        else if (_battleTurn == BattleTurn.mTurn)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("【 몬스터 턴 】");
            Console.ResetColor();
            Console.SetCursorPosition(_battleField.GetLength(1) / 4, _battleField.GetLength(0) -4);
            Console.WriteLine("대기중......");
        }
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
        if (_battleTurn != BattleTurn.pTurn) return;
        int Damage = 10;
        _monster.Health.Value -= Damage;
        if (_monster.Health.Value <= 0)
        {
            _monster.Health.Value = 0;
            Victory();
        }
        _battleTurn = BattleTurn.mTurn;
        _battleTurn = BattleTurn.mTurn;
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
        Console.Write($"{_monster.Health.Value} / {_monster._maxHealth}");
        Console.SetCursorPosition(_battleField.GetLength(1) - 11, 2);
        Console.Write("HP: ");
        Console.SetCursorPosition(_battleField.GetLength(1) - 7, 2);
        _monster.DrawHealthGauge();
    }
    
    public void Victory()
    {
        Console.SetCursorPosition(_battleField.GetLength(1) / 4, _battleField.GetLength(0) -4);
        Console.WriteLine("몬스터를 쓰러트렸다");
        _player.Field = _prevPlayerField;
        _player.Position = _prevPlayerPosition;
        
        if (_prevPlayerField != null)
        {
            _prevPlayerField[_prevPlayerPosition.Y, _prevPlayerPosition.X].OnTileObject = _player;
        }
        
        _player.ExitBattle();
        SceneManager.Change("Town");
    }

    public void mAttack()
    {
        if (_battleTurn != BattleTurn.mTurn) return;
        int mDamage = 10;
        _player.Health.Value -= mDamage;
        if (_player.Health.Value <= 0)
        {
            _player.Health.Value = 0;
            GameManager.IsGameOver = true;
        }
        _battleTurn = BattleTurn.pTurn;
    }
}

public enum BattleTurn
{
    pTurn,
    mTurn,
}