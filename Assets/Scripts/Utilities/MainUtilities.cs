using UnityEngine;

public class MainUtilities
{
    public static int Modulo(int value, int mode)
    {
        return (value % mode + mode) % mode;
    }
}
