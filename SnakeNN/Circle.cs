public class WorldObject
{
    public int x { get; set; }
    public int y { get; set; }
    public WorldStates type { get; set; }

    public WorldObject(int givenX, int givenY, WorldStates givenType)
    {
        x = givenX;
        y = givenY;
        type = givenType;
    }
}

