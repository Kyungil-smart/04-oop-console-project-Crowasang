

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
        _player.Field = _battleField;
        _player.Position = new Vector(5, 5);
        _battleField[5, 5].OnTileObject = _player;

        // 몬스터 오른쪽에 배치
        _monster.Position = new Vector(24, 5);
        _battleField[5, 24].OnTileObject = _monster;
    }

    public override void Update()
    {
        
    }

    public override void Render()
    {
        
    }

    public override void Exit()
    {
        
    }
}