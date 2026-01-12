

public class TownScene : Scene
{
    private Tile[,] _field = new Tile[10, 20];
    private PlayerCharacter _player;
    private Tile[,] _prevField;
    private Vector _prevPosition;
    private List<Monster> _monsters = new List<Monster>();
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
        SpawnMonster();
        _field[3, 5].OnTileObject = new Potion() { Name = "Potion1"};
        _field[2, 15].OnTileObject = new Potion() { Name = "Potion2"};
        _player.Field = _field;
        _player.Position = new Vector(4, 3);
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;
    }
    private void SpawnMonster()
    {
        Monster slime = new Slime();
        Monster goblin = new Goblin();

        _monsters.Add(slime);
        _monsters.Add(goblin);
        _field[6, 17].OnTileObject = slime;
        _field[3, 15].OnTileObject = goblin;
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
}