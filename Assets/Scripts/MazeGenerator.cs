using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int size_x, size_y;

    public GameObject floor_prefab, wall_prefab;
    public float grid_size, wall_thickness;

    void Start()
    {
        BFSGenerator gen = new BFSGenerator(size_x, size_y);
        gen.GenerateMaze();

        SpawnMaze(gen);
    }

    private void SpawnMaze(BFSGenerator gen)
    {
        SpawnFloor();
        SpawnWalls(gen);
    }

    private void SpawnFloor()
    {
        var floor = Instantiate(floor_prefab, new Vector3(grid_size * size_x / 2.0f, 0, grid_size * size_y / 2.0f), Quaternion.identity);
        floor.transform.localScale = new Vector3(grid_size * size_x, grid_size, grid_size * size_y);
    }

    private void SpawnWalls(BFSGenerator gen)
    {
        for (int x = 0; x < size_x; ++x)
        {
            for (int y = 0; y < size_y; ++y)
            {
                int walls = gen.GetWall(x, y);
                if ((walls & (int)BFSGenerator.EWall.north) == 1)
                {
                    GameObject wall = Instantiate(wall_prefab, new Vector3(x, 1, y + grid_size / 2.0f), Quaternion.identity);
                    wall.name = "North Wall[" + x + "][" + y + "]";
                    wall.transform.localScale = new Vector3(wall_thickness, 1, grid_size);
                }

                if ((walls & (int)BFSGenerator.EWall.east) == (int)BFSGenerator.EWall.east)
                {
                    GameObject wall = Instantiate(wall_prefab, new Vector3(x + grid_size / 2.0f, 1, y + grid_size), Quaternion.identity);
                    wall.name = "East Wall[" + x + "][" + y + "]";
                    wall.transform.localScale = new Vector3(grid_size, 1, wall_thickness);
                }

                if ((walls & (int)BFSGenerator.EWall.south) == (int)BFSGenerator.EWall.south)
                {
                    GameObject wall = Instantiate(wall_prefab, new Vector3(x + grid_size, 1, y + grid_size / 2.0f), Quaternion.identity);
                    wall.name = "South Wall[" + x + "][" + y + "]";
                    wall.transform.localScale = new Vector3(wall_thickness, 1, grid_size);
                }

                if ((walls & (int)BFSGenerator.EWall.west) == (int)BFSGenerator.EWall.west)
                {
                    GameObject wall = Instantiate(wall_prefab, new Vector3(x + grid_size / 2.0f, 1, y), Quaternion.identity);
                    wall.name = "West Wall[" + x + "][" + y + "]";
                    wall.transform.localScale = new Vector3(grid_size, 1, wall_thickness);
                }

            }
        }
    }
}
