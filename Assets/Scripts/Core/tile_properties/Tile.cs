using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Component[] components;  //start with the North tile component, then go clockwise when uploading start with upper - go to \ then / then _ etc.
    [SerializeField] private Component centerIdentity;
    private CircularBuffer buffer;
    private Component.Types[] types;
    bool isChosen;  //if Tile was not chosen then it shouldnt rotate, there is a setter function below to change this boolean

    public Tile() {
        
        for (int i = 0; i < components.Length; i++)
        {
            types[i] = components[i].GetType();
        }
        buffer = new CircularBuffer(types);
        isChosen = false;
    }
    //Tile rotation, just plug the rotation angle from real world, whoever is going to make the actual Tile rotations make sure each rotation step is 60 degrees
    public void TileRotate(int x) {
        if (isChosen)
        {
            if (x > 0)
            {
                buffer.AngleRotateRight(x);
            }
            else if (x < 0)
            {
                buffer.AngleRotateLeft(x);
            }
            else
            {

                Debug.Log("There was no rotation");
            }

        }
        else {
            Debug.Log("The Tile was not chosen");
        }


    }
    //this function is just a Geo Compass based type getter, it is going to be needed when making the type matching system
    //for example notice that for the north-east part of the tile the matching part should be another tile's north-west one, and so on
    //good luck on tile matching system making
    public Component.Types GeoTileGetter(string compass) { 
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
                return Component.Types.Null;
        }
    }

    public void ChooseTile() { 
        isChosen = true;    
    }



}
