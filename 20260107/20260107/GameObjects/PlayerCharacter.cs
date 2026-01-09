

public class PlayerCharacter : GameObject
{
    public ObservableProperty<int> Health = new ObservableProperty<int>(5);
    public ObservableProperty<int> Mana = new ObservableProperty<int>(5);
    public Tile[,] Field { get; set; }
    private Inventory _inventory;
    private BattleScene _battleScene;
    private string _healthGauge;
    private string _manaGauge;
    
    public bool IsActiveControl { get; private set; }
    public PlayerCharacter() => Init();
    
    public void Init()
    {
        Symbol = 'P';
        IsActiveControl = true;
        Health.AddListener(SetHealthGauge);
        Mana.AddListener(SetManaGauge);
        _inventory = new Inventory(this);
    }

    public void Update()
    {
        if (InputManager.GetKey(ConsoleKey.I))
        {
            HandleControl();
        }

        if (InputManager.GetKey(ConsoleKey.UpArrow))
        {
            Move(Vector.Up);
            _inventory.SelectUp();

        }
        
        if (InputManager.GetKey(ConsoleKey.DownArrow))
        {
            Move(Vector.Down);
            _inventory.SelectDown();
        }

        if (InputManager.GetKey(ConsoleKey.LeftArrow))
            Move(Vector.Left);

        if (InputManager.GetKey(ConsoleKey.RightArrow))
        {
            Move(Vector.Right);
        }

        if (InputManager.GetKey(ConsoleKey.Enter))
        {
            _inventory.Select();
        }

        if (InputManager.GetKey(ConsoleKey.T))
        {
            Health.Value--;
            if (Health.Value <= 1)
            {
                Health.Value = 1;
            }
        }

        
        
    }
    

    public void HandleControl()
    {
        _inventory.IsActive = !_inventory.IsActive;
        IsActiveControl = !_inventory.IsActive;
    }
    
    
    private void Move(Vector direction)
    {
        if (Field == null || !IsActiveControl) return;
        
        Vector current = Position;
        Vector nextPos = Position + direction;
        
        // 1. 맵 바깥은 아닌지?
        if (nextPos.X < 0) nextPos.X = 0;
        if (nextPos.X >= Field.GetLength(1))  nextPos.X = Field.GetLength(1) - 1;
        if (nextPos.Y < 0) nextPos.Y = 0;
        if (nextPos.Y >= Field.GetLength(0))  nextPos.Y = Field.GetLength(0) - 1;
        // 2. 벽인지?

        GameObject nextTileObject = Field[nextPos.Y, nextPos.X].OnTileObject;
        if (nextTileObject != null)
        {
            if (nextTileObject is IInteractable)
            {
                (nextTileObject as IInteractable).Interact(this);
                if (nextTileObject is Monster)
                {
                    return;
                }
            }
        }
        Field[Position.Y, Position.X].OnTileObject = null;
        Field[nextPos.Y, nextPos.X].OnTileObject = this;
        Position = nextPos;
        
        // Debug.LogWarning($"플레이어 이동 : ({current.X},{current.Y}) -> ({nextPos.X},{nextPos.Y})");
    }
    
    public void Render()
    {
        // DrawHealthGauge();
        // DrawManaGauge();
        _inventory.Render();
    }

    public void AddItem(Item item)
    {
        _inventory.Add(item);
    }
    
    public void DrawManaGauge()
    {
        Console.SetCursorPosition(Position.X - 2, Position.Y - 1);
        _manaGauge.Print(ConsoleColor.Blue);
    }
    public void DrawHealthGauge()
    {
        Console.SetCursorPosition(Position.X - 2, Position.Y - 2);
        _healthGauge.Print(ConsoleColor.Red);
    }
    public void SetHealthGauge(int health)
    {

        switch (Health.Value)
        {
            case 5:
                _healthGauge = "■■■■■";
                break;
            case 4:
                _healthGauge = "■■■■□";
                break;
            case 3:
                _healthGauge = "■■■□□";
                break;
            case 2:
                _healthGauge = "■■□□□";
                break;
            case 1:
                _healthGauge = "■□□□□";
                break;
        }
    }

    public void SetManaGauge(int mana)
    {
        switch (Mana.Value)
        {
            case 5:
                _manaGauge = "■■■■■";
                break;
            case 4:
                _manaGauge = "■■■■□";
                break;
            case 3:
                _manaGauge = "■■■□□";
                break;
            case 2:
                _manaGauge = "■■□□□";
                break;
            case 1:
                _manaGauge = "■□□□□";
                break;
        }
    }
    public void Heal(int value)
    {
        Health.Value += value;
    }
    public void EnterBattle()
    {
        IsActiveControl = false;
    }
    public void ExitBattle()
    {
        IsActiveControl = true;
    }
}