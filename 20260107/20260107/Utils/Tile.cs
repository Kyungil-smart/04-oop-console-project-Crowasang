
public struct Tile
{
    // 타일 위에 뭐가 올라와있는지?
    public GameObject OnTileObject { get; set; }
    // 자기 좌표
    public Vector Position { get; set; }
    
    public bool HasGameObject => OnTileObject != null;

    public Tile(Vector position)
    {
        Position = position;
    }
    public void Print()
    {
        if (HasGameObject)
        {
            OnTileObject.Symbol.Print();
        }
        else
        {
            ' '.Print();
        }
    }
}