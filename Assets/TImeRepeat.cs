using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TImeRepeat : MonoBehaviour
{
     private int rows = 200;
     private int cols = 200;
     private int[,] map;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileBase1;
    [SerializeField] private TileBase tileBase2;

    [SerializeField] private int runs;
    [SerializeField] private float straightChance = 0.7f;
    [SerializeField] private int maxRoomSize = 7;
    [SerializeField] private int minRoomSize = 3;
    [SerializeField] private int comboNeeded = 0;
    [SerializeField] private int fillLenght = 2;


    private void Start()
    {
        map = new int[rows, cols];
        Walker();
        Texture();
        //InvokeRepeating("running", 2.0f, .2f);
    }
    //Iterates over the arry 
    private void resetMap()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = 0;
            }
        }
    }
    /*private void running()
    {
        resetMap();
        //More walkers means that the start will be closer to the center
        Walker();
        Walker();
        Walker();
        Walker();
        Walker();
        Walker();
        Texture();
    }*/

    //Applies a tile image to all of te grid icons (Binary)
    private void Texture() 
    {
        for (var i = 0; i < rows; ++i)
        {
            for (var j = 0; j < cols; ++j)
            {
                if (map[i,j] == 0)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tileBase1);
                }
                else if (map[i,j] == 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tileBase2);
                }
            }
        }
    }
   //Creates a walker that walks around an empty map and creates the map around them
    private void Walker()
    {
        int walkerX = rows/2;
        int walkerY = cols/2;
        int direction = 1;
        int combo = 0;

        for (int i = 0;i < runs; ++i)
        {
            //Possible actions
            //  Moving straight
            //  Turning 90
            //  Create a room on the walkers current location

            float action = Random.Range(0f, 1f);
            if (action > 1f - straightChance)
            {
                if (combo > comboNeeded) Room(walkerX, walkerY);
                else
                {
                    combo += 1;
                    int[] temp = new int[2];
                    temp = Forward(direction, walkerX, walkerY);
                    walkerX = temp[0];
                    walkerY = temp[1];
                }
            }
            else if (action > (1f - straightChance)/2)
            {
                direction = Rotate(1,direction);
                combo = 0;
            }
            else
            {
                direction = Rotate(-1, direction);
                combo = 0;
            }
            map[walkerX, walkerY] = 1;
            
        }
    }
    //Walker turns 90 degrees
    private int Rotate(int rotation, int direction)
    {
        direction += rotation;
        if (direction == 5)
        {
            direction = 1;
        }
        else if (direction == 0)
        {
            direction = 4;
        }
        return direction;
    }
    //Walker moves forward in its current dirrection
    private int[] Forward(int direction, int walkerX, int walkerY)
    {
        int[] sendBack = new int[3];
        switch (direction)
        {
            case 1:
                if (walkerY > cols - maxRoomSize)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = Rotate(1, direction);
                    else if (action == 2) direction = Rotate(-1, direction);
                }
                else walkerY += 1;
                break;
            case 2:
                if (walkerX > rows - maxRoomSize)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = Rotate(1, direction);
                    else if (action == 2) direction = Rotate(-1, direction);
                }
                else walkerX += 1;
                break;
            case 3:
                if (walkerY < maxRoomSize - 1)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = Rotate(1, direction);
                    else if (action == 2) direction = Rotate(-1, direction);
                }
                else walkerY -= 1;
                break;
            case 4:
                if (walkerX < maxRoomSize - 1)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = Rotate(1, direction);
                    else if (action == 2) direction = Rotate(-1, direction);
                }
                else walkerX -= 1;
                break;
            default:
                break;
        }
        sendBack[0] = walkerX;
        sendBack[1] = walkerY;
        sendBack[2] = direction;
        return sendBack;
    }
    //Walker creates a random room size on its current location :D
    private void Room(int walkerX, int walkerY)
    {
        int size = Random.Range(minRoomSize, maxRoomSize);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                map[walkerX - (size/2) + i, walkerY - (size/2) + j] = 1;
            }
        }
    }
    private void Fill()
    {
        for (int i = 0; i < rows - maxRoomSize/2 ; i++)
        {
            for (int k = 0; k < cols - maxRoomSize/2 ; k++)
            {
                if (map[i + maxRoomSize, k + maxRoomSize] == 0)
                {
                    int total = 0;
                    bool validRight = false;
                    bool validLeft = false;
                    bool validUp = false;
                    bool validDown = false;
                    for (int j = 0; j < fillLenght; j++)
                    {
                        if (map[i + maxRoomSize + j, k + maxRoomSize] == 1)
                        {
                            validUp = true;
                        }      
                    }
                }
            }
        }
    }
}
