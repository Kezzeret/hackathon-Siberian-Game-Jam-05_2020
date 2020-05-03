using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Tilemaps = UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using System.Collections.Specialized;

public class Maze : MonoBehaviour
{
    float placementThreshold = .1f;
    Transform boardHolder;
    GameObject[] tilesToUse;
    int rows;
    int cols;
    string type;
    string[,] map;
    bool[,] moveMap;

    int exitX;
    int exitY;
    public List<Movement> movements = new List<Movement>();


    public Maze(int r, int c, Transform board)
    {
        this.rows = r;
        this.cols = c;
        this.boardHolder = board;
        this.map = new string[r, c];
    }
    public void Build(GameObject[] tiles)
    {
        tilesToUse = tiles;
        this.type = "roof";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > this.placementThreshold)
                    {
                        this.Place(i, j);

                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        this.Place(i + a, j + b);
                    }
                }
            }
        }
    }
    public void AddEnemies(GameObject[] tiles, int minCount, int maxCount, string name)
    {
        this.tilesToUse = tiles;
        this.type = name;
        int objectCount = Random.Range(minCount, maxCount + 1);
        for (int i = 0; i <= objectCount; i++)
        {
            Vector3 vector = GetEmpty();
            this.Place(vector);
        }
    }
    public void AddObstacles(GameObject[] tiles, int minCount, int maxCount)
    {
        this.tilesToUse = tiles;
        this.type = "obstacle";
        int objectCount = Random.Range(minCount, maxCount + 1);
        for (int i = 0; i <= objectCount; i++)
        {
            Vector3 vector = GetEmpty();
            if (this.map[(int)vector.x, (int)vector.y + 1] == "roof" && this.map[(int)vector.x + 1, (int)vector.y] == "roof")
            {
                i--;
            }
            else if (this.map[(int)vector.x + 1, (int)vector.y] == "roof" && this.map[(int)vector.x, (int)vector.y - 1] == "roof")
            {
                i--;
            }
            else if (this.map[(int)vector.x, (int)vector.y - 1] == "roof" && this.map[(int)vector.x - 1, (int)vector.y] == "roof")
            {
                i--;
            }
            else if (this.map[(int)vector.x - 1, (int)vector.y] == "roof" && this.map[(int)vector.x, (int)vector.y + 1] == "roof")
            {
                i--;
            }
            else if (this.map[(int) vector.x + 1, (int) vector.y] == "roof")
            {
                this.Place(vector, this.tilesToUse[0]);
            }
            else
            {
                this.Place(vector, this.tilesToUse[1]);
            }
        }
    }
    public void AddExit(GameObject exit)
    {
        this.AddExitInternal(exit, this.rows - 1, this.cols - 1);
    }
    public Vector3 getPlayerPosition()
    {
        return this.Player(0, 0);
    }
    void CanMove(int x, int y, Movement mov)
    {
        if(this.moveMap[x,y])
        {
            return;
        }
        this.moveMap[x, y] = true;
        if (x == this.exitX && y == this.exitY)
        {
            this.movements.Add(mov);
            return;
        }
        if (this.map[x, y] != null)
        {
            if (this.map[x, y] == "school") mov.school++;
            if (this.map[x, y] == "grandma") mov.grandma++;
            if (this.map[x, y] == "obstacle") mov.obstacle++;
        }
        if (x < this.rows - 1 && this.map[x + 1, y] != "roof")
        {
            this.CanMove(x + 1, y, new Movement(mov));
        }
        if (y < this.cols - 1  && this.map[x, y + 1] != "roof")
        {
            this.CanMove(x, y + 1, new Movement(mov));
        }
        if (x > 0 && this.map[x - 1, y] != "roof")
        {
            this.CanMove(x - 1, y, new Movement(mov));
        }
        if (y > 0 && this.map[x, y - 1] != "roof")
        {
            this.CanMove(x, y - 1, new Movement(mov));
        }
    }
    Vector3 Player(int x, int y)
    {
        
        if (this.map[x,y] != null)
        {
            return this.Player(x + 1, y + 1);
        }
        this.movements.Clear();
        this.moveMap = new bool[this.rows, this.cols];
        this.CanMove(x, y, new Movement());
        if (this.movements.Count > 0)
        {
            return new Vector3(x, y, 0f);
        }
        return this.Player(x + 1, y + 1);
    }
    Vector3 GetEmpty()
    {
        int randomX = Random.Range(1, this.rows - 1);
        int randomY = Random.Range(1, this.cols - 1);
        if (this.map[randomX, randomY] == null)
        {
            return new Vector3(randomX, randomY, 0f);
        }
        return this.GetEmpty();
        
    }
    GameObject pick()
    {
        return tilesToUse[Random.Range(0, tilesToUse.Length)];
    }
    void Place(int x, int y)
    {
        this.Place(x, y, this.pick());
    }
    void Place(int x, int y, GameObject obj)
    {
        this.Place(new Vector3(x, y, 0f), obj);
    }
    void Place(Vector3 vector)
    {
        this.Place(vector, this.pick());
    }
    void Place(Vector3 vector, GameObject obj)
    {
        if (vector.x < 0 || vector.y < 0 || vector.x > this.rows || vector.y > this.cols)
        {
            return;
        }
        GameObject instance = Instantiate(obj, vector, Quaternion.identity) as GameObject;
        instance.transform.SetParent(boardHolder);
        this.map[(int) vector.x, (int) vector.y] = this.type;
    }
    void AddExitInternal(GameObject exit, int x, int y)
    {
        if (this.map[x, y] == null)
        {
            this.exitX = x;
            this.exitY = y;
            this.Place(x, y, exit);
        }
        else
        {
            this.AddExitInternal(exit, x, y - 1);
        }
    }
}
