using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TImeRepeat : MonoBehaviour
{
    private int rows = 200;
    private int cols = 200;
    private int[,] map;

    public Tilemap tilemap;
    public TileBase tileBase1;
    public TileBase tileBase2;

    public int runs = 1500;
    public float straightChance = 0.7f;
    public int maxRoomSize = 7;
    public int minRoomSize = 3;
    public int comboNeeded = 0;


    private void Start()
    {
        map = new int[rows, cols];
        InvokeRepeating("running", 2.0f, .2f);
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
    //Just runs the program once 
    private void running()
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
    }
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
                if (combo > comboNeeded) room(walkerX, walkerY);
                else
                {
                    combo += 1;
                    int[] temp = new int[2];
                    temp = forward(direction, walkerX, walkerY);
                    walkerX = temp[0];
                    walkerY = temp[1];
                }
            }
            else if (action > (1f - straightChance)/2)
            {
                direction = rotate(1,direction);
                combo = 0;
            }
            else
            {
                direction = rotate(-1, direction);
                combo = 0;
            }
            map[walkerX, walkerY] = 1;
            
        }
    }
    //Walker turns 90 degrees
    private int rotate(int rotation, int direction)
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
    private int[] forward(int direction, int walkerX, int walkerY)
    {
        int[] sendBack = new int[3];
        switch (direction)
        {
            case 1:
                if (walkerY > cols - 5)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = rotate(1, direction);
                    else if (action == 2) direction = rotate(-1, direction);
                }
                else walkerY += 1;
                break;
            case 2:
                if (walkerX > rows - 5)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = rotate(1, direction);
                    else if (action == 2) direction = rotate(-1, direction);
                }
                else walkerX += 1;
                break;
            case 3:
                if (walkerY < 4)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = rotate(1, direction);
                    else if (action == 2) direction = rotate(-1, direction);
                }
                else walkerY -= 1;
                break;
            case 4:
                if (walkerX < 4)
                {
                    int action = Random.Range(0, 2);
                    if (action == 1) direction = rotate(1, direction);
                    else if (action == 2) direction = rotate(-1, direction);
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
    //Walker creates a random room size on its current location
    private void room(int walkerX, int walkerY)
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
}
