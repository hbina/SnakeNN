public enum ObjectDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NULL
};

public class WorldObject
{
    public int X { get; set; }
    public int Y { get; set; }
    public WorldState State { get; set; }
    public ObjectDirection Direction;

    public WorldObject(int GivenX, int GivenY, WorldState GivenType, ObjectDirection GivenDirection)
    {
        X = GivenX;
        Y = GivenY;
        State = GivenType;
        Direction = GivenDirection;
    }

    public WorldObject(int GivenX, int GivenY, WorldState GivenType)
    {
        X = GivenX;
        Y = GivenY;
        State = GivenType;
        Direction = ObjectDirection.NULL;
    }
}

