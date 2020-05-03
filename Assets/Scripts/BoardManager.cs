using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 20;
    public int rows = 20;
    public Count roofCount = new Count(5, 9);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] roofTiles;
    public GameObject[] yamaTiles;
    public GameObject[] grandmaTiles;
    public GameObject[] schoolTiles;
    private Transform player;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Map").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantLate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantLate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantLate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Debug.Log("OnCollisionEnter2D");
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        int enemyCount = 3;
        Maze maze = new Maze(rows, columns, boardHolder);
        maze.Build(roofTiles);
        maze.AddEnemies(schoolTiles, enemyCount, enemyCount, "school");
        maze.AddEnemies(grandmaTiles, enemyCount, enemyCount, "grandma");
        maze.AddObstacles(yamaTiles, enemyCount, enemyCount);
        maze.AddExit(exit);
        Vector3 v = maze.getPlayerPosition();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = v;
        Debug.Log(player);
        Debug.Log(v.x);
        Debug.Log(v.y);
        Debug.Log(maze.movements[0].school);
        Debug.Log(maze.movements[0].grandma);
        Debug.Log(maze.movements[0].obstacle);
    }
    /*
    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        //LayoutObjectAtRandom(roofTiles, roofCount.minimum, roofCount.maximum);
        Maze maze = new Maze(rows, columns, roofTiles, boardHolder);
        maze.Build();
        int enemyCount = 1;
        LayoutObjectAtRandom(yamaTiles, enemyCount, enemyCount);
        LayoutObjectAtRandom(grandmaTiles, enemyCount, enemyCount);
        LayoutObjectAtRandom(schoolTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0F), Quaternion.identity);
    }
    */
}
