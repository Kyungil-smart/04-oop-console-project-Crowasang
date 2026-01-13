

public class PlayerCharacter : GameObject
{
    public const int _maxHealth = 100;
    public ObservableProperty<int> Health = new ObservableProperty<int>(_maxHealth);
    public Tile[,] Field { get; set; }
    private Inventory _inventory;
    private BattleScene _battleScene;
    private string _healthGauge;
    private string _manaGauge;
    private ConsoleColor _healthColor;
    public bool IsActiveControl { get; private set; }
    public PlayerCharacter() => Init();
    
    public void Init()
    {
        Symbol = 'P';
        IsActiveControl = true;
        Health.AddListener(SetHealthGauge);
        _inventory = new Inventory(this);
        SetHealthGauge(_maxHealth);
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
        
    }
    
    public void Render()
    {
        _inventory.Render();
    }

    public void AddItem(Item item)
    {
        _inventory.Add(item);
    }
    
    public void DrawHealthGauge()
    {
        // Console.SetCursorPosition(0, 2);
        _healthGauge.Print(_healthColor);
    }
    public void SetHealthGauge(int health)
    {
        float ratio = (float)health / _maxHealth;
        if (ratio >= 0.8f) _healthGauge = "■■■■■";
        else if (ratio >= 0.6f) _healthGauge = "■■■■□";
        else if (ratio >= 0.4f) _healthGauge = "■■■□□";
        else if (ratio >= 0.2f) _healthGauge = "■■□□□";
        else if (ratio > 0) _healthGauge = "■□□□□";
        else
        {
            _healthGauge = "□□□□□";
        }
        _healthColor = getHpColor(ratio);
    }
    public void Heal(int value)
    {
        Health.Value += value;
    }
    public void EnterBattle()
    {
        IsActiveControl = false;
        SetHealthGauge(Health.Value);
    }
    public void ExitBattle()
    {
        IsActiveControl = true;
    }

    ConsoleColor getHpColor(float ratio)
    {
        if (ratio >= 0.7f) return ConsoleColor.Green;
        else if (ratio >= 0.3f) return ConsoleColor.Yellow;
        return ConsoleColor.Red;
    }
}