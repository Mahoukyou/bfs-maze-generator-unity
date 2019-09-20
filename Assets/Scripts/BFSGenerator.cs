using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


public class BFSGenerator
{
    private int size_x, size_y;

    public enum EWall
    {
        north = 0x1,
        east = 0x2,
        south = 0x4,
        west = 0x8
    };

    private int[] walls;

    public BFSGenerator(int size_x, int size_y)
    {
        if (size_x <= 0 || size_y <= 0)
        {
            throw new ArgumentException("Size of the maze is invalid");
        }

        this.size_x = size_x;
        this.size_y = size_y;
    }

    public void GenerateMaze()
    {
        InitializeMaze();
        RemoveWalls();

        int x = 0;
    }

    public int GetWall(int index)
    {
        if(index < 0 || index >= walls.Length)
        {
            throw new IndexOutOfRangeException("Out of range...");
        }

        return walls[index];
    }

    public int GetWall(int x, int y)
    {
        // todo bound check
        return GetWall(XYToIndex(x, y));
    }

    private void InitializeMaze()
    {
        walls = new int[size_x * size_y];
        for (int i = 0; i < walls.Length; ++i)
        {
            walls[i] = (int)(EWall.east | EWall.north | EWall.south | EWall.west);
        }
    }

    private void RemoveWalls()
    {
        Stack<int> backtracing_stack = new Stack<int>();
        bool[] visited = new bool[size_x * size_y];

        backtracing_stack.Push(0);
        visited[0] = true;

        Dictionary<Vector2Int, EWall> neighbours_directions = new Dictionary<Vector2Int, EWall> {
            { new Vector2Int(-1, 0), EWall.north },
            { new Vector2Int(0, 1), EWall.east },
            { new Vector2Int(1, 0), EWall.south },
            { new Vector2Int(0, -1), EWall.west } };
        while (backtracing_stack.Count != 0)
        {
            Vector2Int current_pos = IndexToXY(backtracing_stack.Peek());

            List<int> unvisited_neighbours = new List<int>();
            foreach(var neighbour_direction in neighbours_directions)
            {
                Vector2Int neighbour_pos = current_pos + neighbour_direction.Key;
                int? neighbour_index = GetNeighbourIndex(neighbour_pos.x, neighbour_pos.y);

                if(neighbour_index != null && !visited[neighbour_index.Value])
                {
                    unvisited_neighbours.Add(neighbour_index.Value);
                }
            }

            if (unvisited_neighbours.Count != 0)
            {
                int neighbour_index = unvisited_neighbours[UnityEngine.Random.Range(0, unvisited_neighbours.Count)];
                Vector2Int neighbour_pos = IndexToXY(neighbour_index);

                EWall wall_direction;
                neighbours_directions.TryGetValue(neighbour_pos - current_pos, out wall_direction);
                walls[backtracing_stack.Peek()] &= ~(int)wall_direction;

                neighbours_directions.TryGetValue((neighbour_pos - current_pos) * (-1), out wall_direction);
                walls[neighbour_index] &= ~(int)wall_direction;

                backtracing_stack.Push(neighbour_index);
                visited[neighbour_index] = true;
            }
            else
            {
                backtracing_stack.Pop();
            }

        }
    }

    public void RemoveDoubleWalls()
    {
        for(int x = 0; x < size_x - 1; ++x)
        {
            for(int y = 0; y < size_y - 1; ++y)
            {
                walls[XYToIndex(x, y)] &= ~((int)EWall.east | (int)EWall.south);
            }
        }
    }

    int? GetNeighbourIndex(int x, int y)
    {
        if (x < 0 || x >= size_x || y < 0 || y >= size_y)
        {
            return null;
        }

        return XYToIndex(x, y);
    }

    public int XYToIndex(int x, int y)
    {
        return x * size_y + y;
    }

    public Vector2Int IndexToXY(int index)
    {
        int x = index / size_y;
        int y = index % size_y;

        return new Vector2Int(x, y);
    }
}
