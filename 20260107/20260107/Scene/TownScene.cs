

public class TownScene : Scene
{
    private Tile[,] _field = new Tile[10, 20];
    private PlayerCharacter _player;
    private List<Monster> _monsters = new List<Monster>();
    private Random _random = new Random();
    private Tile[,] _tiles;
    public TownScene(PlayerCharacter player) => Init(player);
    
    public void Init(PlayerCharacter player)
    {
        _player = player;
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                Vector pos = new Vector(x, y);
                _field[y, x] = new Tile(pos);
            }
        }
        _player.Field = _field;
        SpawnMonster();
        SpawnObject();
        _player.Position = new Vector(4, 3);
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;
    }
    private void SpawnObject()
    {
        for (int i = 0; i < 2; i++)
        {
            int x = randX();
            int y = randY();
            if (_field[x, y].OnTileObject == null)
            {
                _field[x, y].OnTileObject = new Potion() { Name = $"Potion{i+1}"};
            }
        }
    }
    private void SpawnMonster()
    {
        Monster slime = new Slime();
        Monster goblin = new Goblin();
        _monsters.Add(slime);
        _monsters.Add(goblin);
        int x = randX();
        int y = randY();
        while (true)
        {
            if (_field[x, y].OnTileObject == null)
            {
                _field[x, y].OnTileObject = slime;
                break;
            }
        }
        x = randX();
        y = randY();
        while (true)
        {
            if (_field[x, y].OnTileObject == null)
            {
                _field[x, y].OnTileObject = goblin;
                break;
            }
        }
    }
    public override void Enter()
    {
        for (int i = _monsters.Count - 1; i >= 0; i--)
        {
            if (_monsters[i].IsDead)
            {
                RemoveMonster(_monsters[i]);
                _monsters.RemoveAt(i);
            }
        }
        if (_monsters.Count <= 0)
        {
            GameManager.IsGameOver = true;
        }
        _player.Field = _field;
        if (_field[_player.Position.Y, _player.Position.X].OnTileObject != _player)
        {
            _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;
        }
    }
    
    private void RemoveMonster(Monster monster)
    {
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                if (_field[y, x].OnTileObject == monster)
                {
                    _field[y, x].OnTileObject = null;
                }
            }
        }
    }
    public override void Update()
    {
        _player.Update();
    }

    public override void Render()
    {
        PrintField();
        _player.Render();
    }

    public override void Exit()
    {
        _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
        _player.Field = null;
    }

    public void PrintField()
    {
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                _field[y, x].Print();
            }

            Console.WriteLine();
        }
    }

    public int randX()
    {
        return _random.Next(0, 9);
    }
    public int randY()
    {
        return _random.Next(0,20);
    }
}