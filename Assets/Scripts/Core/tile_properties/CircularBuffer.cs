using System;
using Unity.VisualScripting;
using UnityEngine;

public class CircularBuffer : MonoBehaviour
{
    //Just a demo types as I have no access to the miro to check the real ones, this will be updated
    public enum Types{ 
        Town,
        Grass,
        Forest,                 
        Lake,
        Null
    }

    private Types[] buffer;
    private int start;

    public CircularBuffer(Types[] items) {
        if (items.Length != 6) {
            throw new System.ArgumentException("Please have 6 elements in your array", nameof(items));
        }
        buffer = new Types[6];
        Array.Copy(items, buffer, items.Length);
        start = 0;
    }

    public void Clear() //just for wiping in case it is needed by other devs
    {
        start = 0;
        Array.Clear(buffer, 0, buffer.Length);
    }
    //this is mainly for implementation in my Tile class, the ones that should be used when making the tile rotation are there, below a circular buffer rotation is used for changing the tile part array after rotation
    public void AngleRotateRight(int x)  
    {
        start = (start + (x % 360) / 60) % 6; 
    }
    public void AngleRotateLeft(int x) {
        start = (start - Math.Abs(x) % 360 / 60 + 6) % 6;
    }
    // getter for the buffer
    public Types Get(int i) {
        return buffer[(start + i) % 6];
    } 
}
