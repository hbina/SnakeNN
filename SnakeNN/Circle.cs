public class Circle
{
    public int x { get; set; }
    public int y { get; set; }
    public CartesianStates type { get; set; }

    public Circle(int givenX, int givenY, CartesianStates givenType)
    {
        x = givenX;
        y = givenY;
        type = givenType;
    }
}

