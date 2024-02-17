using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //[Header("데이터")]
    //public MazeData data;

    public int width = 10;
    public int height = 10;

    public float cellWidth = 0.83f;
    public float cellHeight = 0;

    public Cell cellPrefab;

    private Cell[,] cellMap;
    private List<Cell> cellHistoryList;

    public GameObject enemyParent;
    public GameObject enemy;
    public int enemyNumber = 10;

    public int chestNumber = 10;
    public GameObject[] chests;
    public GameObject chestsParent;

    public delegate void ChestHandle();
    public event ChestHandle OnChestGeneration;

    //private void Awake()
    //{
    //    data.LoadDataFromPrefs();

    //    width = data.width;
    //    height = data.height;

    //}

    void Start()
    {
        BatchCells();
        BatchChests();
        BatchEnemy();

        MakeMaze(cellMap[0, 0]);
        cellMap[0, 0].isBackWall = false;
        cellMap[width - 1, height - 1].isForwardWall = false;
        cellMap[0, 0].ShowWalls();
        cellMap[width - 1, height - 1].ShowWalls();


    }

    private void BatchCells()
    {
        cellMap = new Cell[width, height];
        cellHistoryList = new List<Cell>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell _cell = Instantiate<Cell>(cellPrefab, this.transform);
                _cell.index = new Vector2Int(x, y);
                _cell.name = "cell_" + x + "_" + y;
                _cell.transform.localPosition = new Vector3(x * cellWidth, cellHeight, y * cellWidth);

                cellMap[x, y] = _cell;

            }
        }
    }

    private void BatchChests()
    {
        HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();

        int diamond = Random.Range(0, chestNumber);

        for (int i = 0; i < chestNumber; i++)
        {
            Vector2Int position = GetRandomPosition(usedPositions);
            usedPositions.Add(position);

            int x = position.x;
            int y = position.y;

            // Instantiate 함수를 사용하여 프리팹을 인스턴스화
            GameObject chestInstance = Instantiate(chests[i % chests.Length] , chestsParent.transform);
            chestInstance.transform.localPosition = new Vector3(x * cellWidth, cellHeight, y * cellWidth );

            if(i==diamond)
            {
                ChestOperation chestOperation = chestInstance.GetComponent<ChestOperation>();
                chestOperation.SetDiamond(true);
            }
        }

        OnChestGeneration?.Invoke();

    }

    private void BatchEnemy()
    {
        HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < enemyNumber; i++)
        {
            Vector2Int position = GetRandomPosition(usedPositions);
            usedPositions.Add(position);

            int x = position.x;
            int y = position.y;

            // Instantiate 함수를 사용하여 프리팹을 인스턴스화
            GameObject EnemyInstance = Instantiate(enemy, enemyParent.transform);
            EnemyInstance.transform.localPosition = new Vector3(x * cellWidth, 0, y * cellWidth);
           
        }
    }



    private Vector2Int GetRandomPosition(HashSet<Vector2Int> usedPositions)
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, height);

        Vector2Int position = new Vector2Int(x, y);

        while (usedPositions.Contains(position))
        {
            x = Random.Range(0, width);
            y = Random.Range(0, height);
            position = new Vector2Int(x, y);
        }

        return position;
    }

    private void MakeMaze(Cell startCell)
    {
        Cell[] neighbors = GetNeighborCells(startCell);
        if (neighbors.Length > 0)
        {
            Cell nextCell = neighbors[Random.Range(0, neighbors.Length)];
            ConnectCells(startCell, nextCell);
            cellHistoryList.Add(nextCell);
            MakeMaze(nextCell);
        }
        else
        {
            if (cellHistoryList.Count > 0)
            {
                Cell lastCell = cellHistoryList[cellHistoryList.Count - 1];
                cellHistoryList.Remove(lastCell);
                MakeMaze(lastCell);
            }
        }
    }

    private Cell[] GetNeighborCells(Cell cell)
    {
        List<Cell> retCellList = new List<Cell>();
        Vector2Int index = cell.index;

        //forward
        if (index.y + 1 < height)
        {
            Cell neighbor = cellMap[index.x, index.y + 1];
            if (neighbor.CheckAllWall())
            {
                retCellList.Add(neighbor);
            }
        }

        //back
        if (index.y - 1 >= 0)
        {
            Cell neighbor = cellMap[index.x, index.y - 1];
            if (neighbor.CheckAllWall())
            {
                retCellList.Add(neighbor);
            }
        }

        //left
        if (index.x - 1 >= 0)
        {
            Cell neighbor = cellMap[index.x - 1, index.y];
            if (neighbor.CheckAllWall())
            {
                retCellList.Add(neighbor);
            }
        }

        // right
        if (index.x + 1 < width)
        {
            Cell neighbor = cellMap[index.x + 1, index.y];
            if (neighbor.CheckAllWall())
            {
                retCellList.Add(neighbor);
            }
        }

        return retCellList.ToArray();
    }

    private void ConnectCells(Cell c0, Cell c1)
    {
        Vector2Int dir = c0.index - c1.index;

        //forward
        if (dir.y <= -1)
        {
            c0.isForwardWall = false;
            c1.isBackWall = false;
        }

        //back
        else if (dir.y >= 1)
        {
            c0.isBackWall = false;
            c1.isForwardWall = false;
        }

        //left
        else if (dir.x >= 1)
        {
            c0.isLeftWall = false;
            c1.isRightWall = false;
        }

        //right
        else if (dir.x <= -1)
        {
            c0.isRightWall = false;
            c1.isLeftWall = false;
        }

        c0.ShowWalls();
        c1.ShowWalls();
    }
}
