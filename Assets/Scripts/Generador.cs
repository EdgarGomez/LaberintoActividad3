using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generador : MonoBehaviour
{
    [SerializeField] private Laberinto celdaPrefab;

    [SerializeField] private int width;
    [SerializeField] private int height;

    private Laberinto[,] grid;
    void Start()
    {
        grid = new Laberinto[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i,j] = Instantiate(celdaPrefab, new Vector3(i, 0, j), Quaternion.identity);
            }
        }

        GenerarMaze(null, grid[0,0]);
    }

    private void GenerarMaze(Laberinto previousCell, Laberinto currentCell)
    {
        currentCell.Visitar();
        ClearWalls(previousCell, currentCell);

        new WaitForSeconds(0.05f);

        Laberinto nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null) 
            { 
                GenerarMaze(currentCell, nextCell);
            }

        } while (nextCell != null);
    }




private Laberinto GetNextUnvisitedCell(Laberinto currentCell)
    {
        var unvisitedCells = GetUnvisitedCell(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<Laberinto> GetUnvisitedCell(Laberinto currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < width)
        {
            var cellToRight = grid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = grid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
            yield return cellToLeft;
            }
        }

        if (z + 1 < height)
        {
            var cellToFront = grid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
            yield return cellToFront;
            }
        }

        if (z-1 >= 0)
        {
            var cellToBack = grid[x, z-1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }
    private void ClearWalls(Laberinto previousCell, Laberinto currentCell)
    {
        if (previousCell == null)
        {
            return;
        }
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearParedDcha();
            currentCell.ClearParedIzq();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearParedIzq();
            currentCell.ClearParedDcha();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearParedFront();
            currentCell.ClearParedAtras();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearParedAtras();
            currentCell.ClearParedFront();
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
