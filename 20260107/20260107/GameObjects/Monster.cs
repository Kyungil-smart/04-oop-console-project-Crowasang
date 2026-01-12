

public class Monster : GameObject, IInteractable
{
    public int _maxHealth = 50;
    public ObservableProperty<int> Health;
    public string Name { get; set; }
    private string _healthGauge;
    private ConsoleColor _healthColor;
    public Tile[,] Field { get; set; }
    public Monster()
    {
        Symbol = 'M';
        Name = "몬스터";
        Health = new ObservableProperty<int>(_maxHealth);
        Health.AddListener(SetHealthGauge);
    }
    
    public void DrawHealthGauge()
    {
        _healthGauge.Print(_healthColor);
    }
    private void SetHealthGauge(int health)
    {
        float ratio = (float)health / _maxHealth;

        if (ratio >= 0.8f) _healthGauge = "■■■■■";
        else if (ratio >= 0.6f) _healthGauge = "■■■■□";
        else if (ratio >= 0.4f) _healthGauge = "■■■□□";
        else if (ratio >= 0.2f) _healthGauge = "■■□□□";
        else if (ratio > 0) _healthGauge = "■□□□□";
        else _healthGauge = "□□□□□";

        _healthColor = getHpColor(ratio);
    }
    ConsoleColor getHpColor(float ratio)
    {
        if (ratio >= 0.7f) return ConsoleColor.Green;
        else if (ratio >= 0.3f) return ConsoleColor.Yellow;
        return ConsoleColor.Red;
    }
    public void Interact(PlayerCharacter player)
    {
        SceneManager.Change(new BattleScene(player, this));
    }
    public void EnterBattle()
    {
        SetHealthGauge(Health.Value);
    }
}