using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private CircularBuffer.Types[] components;  //start with the North tile component, then go clockwise when uploading start with upper - go to \ then / then _ etc.
    [SerializeField] private CircularBuffer.Types centerIdentity;
    private CircularBuffer buffer;

    public Tile() {
  
        buffer = new CircularBuffer(components);
    
    }
    //Tile rotation, just plug the rotation angle from real world, whoever is going to make the actual Tile rotations make sure each rotation step is 60 degrees
    public void TileRotate(int x) {  
        if (x > 0)
        {
            buffer.AngleRotateRight(x);
        }
        else if (x < 0)
        {
            buffer.AngleRotateLeft(x);
        }
        else {

            Debug.Log("There was no rotation");
        }
    
    }
    //this function is just a Geo Compass based type getter, it is going to be needed when making the type matching system
    //for example notice that for the north-east part of the tile the matching part should be another tile's north-west one, and so on
    //good luck on tile matching system making
    public CircularBuffer.Types GeoTileGetter(string compass) { 
        switch (compass)
        {
            case "N":
                return buffer.Get(0); //gets the type of North Tile

            case "NE":
                return buffer.Get(1); // North East Tile Type

            case "SE":
                return buffer.Get(2);

            case "S":
                return buffer.Get(3);

            case "SW":
                return buffer.Get(4);

            case "NW":
                return buffer.Get(5);
            default:
                print("Oops, something went wrong with your GeoTileGetter, recheck the input argument");
                return CircularBuffer.Types.Null;
        }
    }





}
