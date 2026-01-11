using UnityEngine;

public class Tools
{
    public static int Modulo(int value, int mode)
    {
        return (value % mode + mode) % mode;
    }
}
