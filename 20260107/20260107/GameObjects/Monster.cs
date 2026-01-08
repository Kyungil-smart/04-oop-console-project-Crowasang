

public class Monster : GameObject, IInteractable
{
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }

    public Monster()
    {
        Symbol = 'M';
        Name = "몬스터";
        MaxHealth = 50;
        Health = 50;
        Attack = 5;
    }

    public void Interact(PlayerCharacter player)
    {
        
    }
}