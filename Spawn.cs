using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject lavaPrefab;
    public GameObject wallPrefab;
    public GameObject coinPrefab;
    public GameObject playerPrefab;
    public int spawnCount = 19;
    private float StartPos;
    public GameObject[,] spawnArray;
    private List<MapTile> spawnList;
    private List<MapTile> coinLocations = new List<MapTile>();
    private int lavaCount = 10;
    private int boxCount = 0;
    private float tileRatio = 0f;
    private MapTile startLocation;
    private int startQuadrant = 0;

    private struct MapTile
    {
        public int row; public int col;
        public MapTile(int addRow, int addCol) { row = addRow; col = addCol; }
    }

    private void Start()
    {
        SetSpawnList();
    }

    //Set List for spiral traversal of map array
    void SetSpawnList()
    {
        spawnList = new List<MapTile>();
        int m = spawnCount; int n = spawnCount;
        int i, k = 0, l = 0;
        while (k < m && l < n)
        {
            //Console.Write(a[k, i] + " ");
            for (i = l; i < n; ++i) spawnList.Add(new MapTile(k, i));
            k++;
            //Console.Write(a[i, n - 1] + " ");
            for (i = k; i < m; ++i) spawnList.Add(new MapTile(i, n - 1));
            n--;
            if (k < m)
            {
                //Console.Write(a[m - 1, i] + " ");
                for (i = n - 1; i >= l; --i) spawnList.Add(new MapTile(m - 1, i));
                m--;
            }
            if (l < n)
            {
                //Console.Write(a[i, l] + " ");
                for (i = m - 1; i >= k; --i) spawnList.Add(new MapTile(i, l));
                l++;
            }
        }
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        startQuadrant = 0;
        startLocation = new MapTile(spawnCount / 2, spawnCount / 2);
        Transform Map = this.transform.parent;
        Map.transform.rotation = Quaternion.Euler(0, 0, 0);
        SpawnNewMap(1);
        Instantiate(playerPrefab, new Vector3(transform.position.x + ((float)startLocation.row * .625f - StartPos), transform.position.y + ((float)startLocation.col * .625f - StartPos), 0), this.transform.rotation);
    }

    public void SpawnNewMap(int level)
    {
        lavaCount = 10 + level * 3;
        if (lavaCount > 100) lavaCount = 100;
        Transform Map = this.transform.parent;
        int currRotation = (int)Map.transform.rotation.eulerAngles.z;
        this.transform.parent = null;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        boxCount = 0;
        ClearMap();
        StartPos = spawnCount * .625f / 2 - .3125f;
        int underRow = startLocation.row;
        int underCol = startLocation.col;
        underRow += ((currRotation / 90) - 2) % 2 * 2;
        underCol += ((currRotation / 90) - 1) % 2 * 2;
        spawnArray[underRow, underCol] = Instantiate(boxPrefab, new Vector3(transform.position.x + ((float)(underRow) * .625f - StartPos), transform.position.y + ((float)(underCol) * .625f - StartPos), 0), this.transform.rotation);
        spawnArray[underRow, underCol].transform.parent = this.gameObject.transform;
        SpawnBlocks(level);
        this.transform.rotation = Map.rotation;
        this.transform.parent = Map;
    }

    public void ClearMap()
    {
        if (spawnArray != null)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                for (int j = 0; j < spawnCount; j++)
                {
                    if (spawnArray[i, j] != null)
                    {
                        Destroy(spawnArray[i, j]);
                        spawnArray[i, j] = null;
                    }
                }
            }
        }
        else
        {
            spawnArray = new GameObject[spawnCount, spawnCount];
        }
        coinLocations.Clear();
    }

    void SpawnCoins()
    {
        coinLocations.Clear();
        int middle = spawnCount / 2;
        SpawnCoin(2, middle - 3, 2, middle - 3);
        SpawnCoin(2, middle - 3, middle + 3, spawnCount - 3);
        SpawnCoin(middle + 3, spawnCount - 2, middle + 3, spawnCount - 3);
        SpawnCoin(middle + 3, spawnCount - 2, 2, middle - 3);
    }

    void SpawnCoin(int xStart, int xEnd, int yStart, int yEnd)
    {
        if (coinLocations.Count == startQuadrant)
        {
            coinLocations.Add(new MapTile(-1, -1));
            return;
        }
        int x = Random.Range(xStart, xEnd);
        int y = Random.Range(yStart, yEnd);
        if (spawnArray[x, y] != null)
        {
            Destroy(spawnArray[x, y]);
            spawnArray[x, y] = null;
        }
        spawnArray[x, y] = Instantiate(coinPrefab, new Vector3(transform.position.x + ((float)x * .625f - StartPos), transform.position.y + ((float)y * .625f - StartPos), 0), this.transform.rotation);
        spawnArray[x, y].transform.parent = this.gameObject.transform;
        coinLocations.Add(new MapTile(x, y));
    }

    void SpawnBlocks(int level)
    {
        int createPlatform = 0;
        for (int i = 0; i < spawnList.Count; i++)
        {
            createPlatform = SpawnBlock(spawnList[i].row, spawnList[i].col, createPlatform, level);
        }
        SpawnCoins();
        SpawnLava();
        for (int i = 0; i < coinLocations.Count; i++)
        {
            if (i == startQuadrant) continue;
            FindPath(startLocation, new MapTile(coinLocations[i].row, coinLocations[i].col));
        }
    }

    int SpawnBlock(int i, int j, int createPlatform, int level)
    {
        tileRatio = .15f - (float)level / 750f;
        if (!isValidIndex(i, j)) return 0;
        if (createPlatform == 0)
        {
            float start = Random.Range(0.0f, 1.0f);
            if (start < tileRatio)
            {
                createPlatform = Random.Range(2, 5);
            }
            return createPlatform;
        }
        else
        {
            spawnArray[i, j] = Instantiate(boxPrefab, new Vector3(transform.position.x + ((float)i * .625f - StartPos), transform.position.y + ((float)j * .625f - StartPos), 0), this.transform.rotation);
            spawnArray[i, j].transform.parent = this.gameObject.transform;
            boxCount++;
            return createPlatform - 1;
        }
    }

    void SpawnLava()
    {
        int i = 0;
        List<MapTile> addedTiles = new List<MapTile>();
        while (i < lavaCount)
        {
            int tileIndex = Random.Range(0, spawnList.Count);
            int currIndex = tileIndex;
            int row, col, changeDir = 0;
            bool validIndex = true;
            int lavaLength = Random.Range(1, 4);
            if (lavaLength + i >= lavaCount) lavaLength = lavaCount - i;
            int lengthIndex = 0;
            while (lengthIndex < lavaLength)
            {
                int loopCount = 0;
                do
                {
                    validIndex = true;
                    if (currIndex >= spawnList.Count) currIndex = 0;
                    row = spawnList[currIndex].row;
                    col = spawnList[currIndex].col;
                    loopCount++;
                    currIndex++;
                    if (spawnArray[row, col] != null)
                    {
                        validIndex = false;
                        continue;
                    }
                    changeDir = CheckLava(row, col);
                    if (changeDir == -1) validIndex = false;
                    if (loopCount >= spawnList.Count)
                    {
                        return;
                    }
                } while (!validIndex);
                Destroy(spawnArray[row, col]);
                spawnArray[row, col] = null;
                spawnArray[row, col] = Instantiate(lavaPrefab, new Vector3(transform.position.x + ((float)row * .625f - StartPos), transform.position.y + ((float)col * .625f - StartPos), 0), this.transform.rotation);
                spawnArray[row, col].transform.parent = this.gameObject.transform;
                addedTiles.Add(new MapTile(row, col));
                i++;
                lengthIndex++;
            }
        }
        for (int rotateIndex = 0; rotateIndex < addedTiles.Count; rotateIndex++)
        {
            int row = addedTiles[rotateIndex].row; int col = addedTiles[rotateIndex].col;
            int rotation = CheckLava(row, col);
            if (rotation == -1) {
                Destroy(spawnArray[row, col]);
                spawnArray[row, col] = null;
            }
            else
            {
                spawnArray[row, col].transform.rotation = Quaternion.Euler(0, 0, rotation);
                spawnArray[row, col].transform.parent = this.gameObject.transform;
            }
        }
    }
    int CheckLava(int row, int col)
    {
        if (!isValidIndex(row, col)) return -1;
        int left = 1; int right = 1; int up = 1; int down = 1; int max = 1;
        int finalDirection = -1;
        while (col + up < spawnCount && spawnArray[row, col + up] == null) up++;
        if (up > max)
        {
            max = up;
            finalDirection = 0;
        }
        while (col - down >= 0 && spawnArray[row, col - down] == null) down++;
        if (down > max)
        {
            max = down;
            finalDirection = 180;
        }
        while (row + right < spawnCount && spawnArray[row + right, col] == null) right++;
        if (right > max)
        {
            max = right;
            finalDirection = -90;
        }
        while (row - left >= 0 && spawnArray[row - left, col] == null) left++;
        if (left > max) finalDirection = 90;
        return finalDirection;
    }

    bool isValidIndex(int row, int col)
    {
        int middle = spawnCount / 2;
        if (row >= middle - 1 && row <= middle + 1 && col >= middle - 1 && col <= middle + 1) return false;
        if (Mathf.Abs(row - startLocation.row) <= 2 && Mathf.Abs(col - startLocation.col) <= 2) return false;
        if (row == 0 || row == spawnCount - 1 || col == 0 || col == spawnCount - 1) return false;
        return true;
    }

    void FindPath(MapTile start, MapTile destination)
    {
        Queue<MapTile> BFS = new Queue<MapTile>();
        BFS.Enqueue(start);
        MapTile currLocation = new MapTile(-1, -1);
        HashSet<MapTile> visited = new HashSet<MapTile>();
        visited.Add(startLocation);
        MapTile closestTile = start;
        while (BFS.Count > 0)
        {
            currLocation = BFS.Dequeue();
            if (currLocation.row == destination.row && currLocation.col == destination.col)
            {
                return;
            }
            if (Mathf.Abs(destination.row - currLocation.row) + Mathf.Abs(destination.col - currLocation.col) <=
                Mathf.Abs(destination.row - closestTile.row) + Mathf.Abs(destination.col - closestTile.col))
                closestTile = currLocation;
            MapTile nextLocation = new MapTile(currLocation.row + 1, currLocation.col);
            if (currLocation.row + 1 < spawnCount - 1 && !visited.Contains(nextLocation) && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
            nextLocation = new MapTile(currLocation.row - 1, currLocation.col);
            if (currLocation.row - 1 > 1 && !visited.Contains(nextLocation) && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
            nextLocation = new MapTile(currLocation.row, currLocation.col + 1);
            if (currLocation.col + 1 < spawnCount - 1 && !visited.Contains(nextLocation) && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
            nextLocation = new MapTile(currLocation.row, currLocation.col - 1);
            if (currLocation.col - 1 > 1 && !visited.Contains(nextLocation) && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
        }
        CreatePath(closestTile, destination);
    }

    void CreatePath(MapTile start, MapTile destination)
    {
        int currRow = start.row;
        int currCol = start.col;
        while (currRow != destination.row || currCol != destination.col)
        {
            if (currRow != destination.row)
            {
                if (spawnArray[currRow, currCol] != null && spawnArray[currRow, currCol].gameObject.tag != "Coin")
                {
                    Destroy(spawnArray[currRow, currCol]);
                    spawnArray[currRow, currCol] = null;
                }
                if (currRow < destination.row) currRow++;
                else currRow--;
            }
            if (currCol != destination.col)
            {
                if (spawnArray[currRow, currCol] != null && spawnArray[currRow, currCol].gameObject.tag != "Coin")
                {
                    Destroy(spawnArray[currRow, currCol]);
                    spawnArray[currRow, currCol] = null;
                }
                if (currCol < destination.col) currCol++;
                else currCol--;
            }
        }
    }

    public void UpdateLocation(Coin currCoin)
    {
        for (int i = 0; i < coinLocations.Count; i++)
        {
            if (coinLocations[i].row == -1) continue;
            if (spawnArray[coinLocations[i].row, coinLocations[i].col] == currCoin.gameObject)
            {
                startQuadrant = i;
                this.startLocation = new MapTile(coinLocations[i].row, coinLocations[i].col);
            }
        }
    }
}