

public class Inventory
{
    private List<Item> _items = new List<Item>();
    public bool IsActive { get; set; }
    public MenuList _itemMenu = new MenuList();
    public PlayerCharacter _owner;
    public Inventory(PlayerCharacter owner)
    {
        _owner = owner;
    }
    
    public void Add(Item item)
    {
        if (_items.Count >= 10) return;
        
        _items.Add(item);
        _itemMenu.Add(item.Name, item.Use);
        item.Inventory = this;
        item.Owner = _owner;
    }

    public void Remove(Item item)
    {
        // _items[_itemMenu.CurrentIndex] = null;
        // _items.RemoveAt(_itemMenu.CurrentIndex);
        _items.Remove(item);
        _itemMenu.Remove();
    }

    // public void Use()
    // {
    //     _items[_itemMenu.CurrentIndex].Use();
    //     Remove();
    // }
    
    public void Render()
    {
        if (!IsActive) return;
        _itemMenu.Render(15, 1);
    }

    public void Select()
    {
        if (!IsActive) return;
        _itemMenu.Select();
    }

    public void SelectUp()
    {
        if (!IsActive) return;
        _itemMenu.SelectUp();
    }

    public void SelectDown()
    {
        if (!IsActive) return;
        _itemMenu.SelectDown();
    }

    
}