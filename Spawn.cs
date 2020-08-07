/***********************************************************************************
 * TFIS: Level Map Spawner
 * Created by Randel Emens
 * Description: Handles all creation of randomly generated maps for each new level
 **********************************************************************************/

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

    //number of tiles across map
    public int spawnCount = 19;

    //length of each tile in unity units
    public float tileLength = .625f;
    private float StartPos;

    //array for hold onto and change map tiles
    public GameObject[,] spawnArray;

    //indices of spawnarray in a list for easy access
    private List<MapTile> spawnList;

    //indices of coin locations in the spawnarray
    private List<MapTile> coinLocations = new List<MapTile>();

    //start player location for each new level
    private MapTile startLocation;
    private int startQuadrant = 0;

    /**************************************************
     * Simple struct to index tiles for the map array
     *************************************************/
    private struct MapTile
    {
        public int row; public int col;
        public MapTile(int addRow, int addCol) { row = addRow; col = addCol; }
    }

    private void Start()
    {
        SetSpawnList();
    }

    /**************************************************
     * Set spawnList for spiral traversal of map array
     * Needed so that box prefabs are evenly 
     * distributed around each side of the map
     *************************************************/
    void SetSpawnList()
    {
        spawnList = new List<MapTile>();
        int m = spawnCount; int n = spawnCount;
        int i, k = 0, l = 0;
        while (k < m && l < n)
        {
            //Tile Index: k, i
            for (i = l; i < n; ++i) spawnList.Add(new MapTile(k, i));
            k++;
            //Tile Index: i, n - 1
            for (i = k; i < m; ++i) spawnList.Add(new MapTile(i, n - 1));
            n--;
            if (k < m)
            {
                //Tile Index: m - 1, i
                for (i = n - 1; i >= l; --i) spawnList.Add(new MapTile(m - 1, i));
                m--;
            }
            if (l < n)
            {
                //Tile Index: i, 1
                for (i = m - 1; i >= k; --i) spawnList.Add(new MapTile(i, l));
                l++;
            }
        }
    }

    // This is called when player chooses to start new game with S key press
    public void StartGame()
    {
        startQuadrant = 0;
        //Always start at middle tile
        startLocation = new MapTile(spawnCount / 2, spawnCount / 2);
        //Make sure map is always set to no rotation at start
        Transform Map = this.transform.parent;
        Map.transform.rotation = Quaternion.Euler(0, 0, 0);
        //Create a new map for level 1
        SpawnNewMap(1);
        //Instantiate player for start of game
        Instantiate(playerPrefab, 
            new Vector3(transform.position.x + ((float)startLocation.row * tileLength - StartPos), 
                transform.position.y + ((float)startLocation.col * tileLength - StartPos), 0), 
                this.transform.rotation);
    }

    /**************************************************************
     * Handle all reseting and recreating of map blocks
     *************************************************************/

    public void SpawnNewMap(int level)
    {
        //Reset rotation to properly handle proper placement of blocks
        Transform Map = this.transform.parent;
        int currRotation = (int)Map.transform.rotation.eulerAngles.z;
        this.transform.parent = null;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);

        ClearMap();
        
        //Place a block directly under player for convenience
        int underRow = startLocation.row;
        int underCol = startLocation.col;
        underRow += ((currRotation / 90) - 2) % 2 * 2;
        underCol += ((currRotation / 90) - 1) % 2 * 2;
        StartPos = spawnCount * tileLength / 2 - tileLength / 2;

        spawnArray[underRow, underCol] = 
            Instantiate(boxPrefab, 
                new Vector3(transform.position.x + ((float)(underRow) * tileLength - StartPos), 
                transform.position.y + ((float)(underCol) * tileLength - StartPos), 0), 
                this.transform.rotation);

        spawnArray[underRow, underCol].transform.parent = this.gameObject.transform;
        SpawnAllBlocks(level);

        //Return transform to previous transform
        this.transform.rotation = Map.rotation;
        this.transform.parent = Map;
    }


    /****************************************************************
     * Destroy all map tile game objects and set spawnarray to null.
     * If spawn array has not been instantiated, instantiate.
     ***************************************************************/
    void ClearMap()
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
            spawnArray = new GameObject[spawnCount, spawnCount];
        coinLocations.Clear();
    }


    /****************************************************************
     * Spawns all required block types on map in order:
     *      1. Platforms
     *      2. Coins
     *      3. Lava Blocks
     * Calls FindPath to verify paths exist to each coin
     ***************************************************************/

    void SpawnAllBlocks(int level)
    {
        SpawnBlocks(level);
        SpawnCoins();

        //Derive number of internal map lava blocks using level and instantiate
        int lavaCount = 10 + level * 3;
        if (lavaCount > 100) lavaCount = 100;
        SpawnLava(lavaCount);

        //Check for valid paths to each coin from player start location and create one if none exist
        for (int i = 0; i < coinLocations.Count; i++)
        {
            if (i == startQuadrant)
                continue;
            FindPath(startLocation, new MapTile(coinLocations[i].row, coinLocations[i].col));
        }
    }


    /*********************************************************
     * Randomly generate all platform blocks in the map
     * using spiral array
     ********************************************************/
    void SpawnBlocks(int level)
    {
        float tileRatio = .15f - (float)level / 750f;
        int createPlatform = 0;
        for (int i = 0; i < spawnList.Count; i++)
        {
            int row = spawnList[i].row;
            int col = spawnList[i].col;

            if (!isValidIndex(row, col))
                continue;

            //Creates rows of platforms of length 2 to 5
            if (createPlatform == 0)
            {
                float start = Random.Range(0.0f, 1.0f);
                if (start < tileRatio)
                    createPlatform = Random.Range(2, 5);
            }
            else
            {
                spawnArray[row, col] = 
                    Instantiate(boxPrefab, 
                    new Vector3(transform.position.x + ((float)row * tileLength - StartPos), 
                        transform.position.y + ((float)col * tileLength - StartPos), 0), 
                        this.transform.rotation);

                spawnArray[row, col].transform.parent = this.gameObject.transform;
                createPlatform--;
            }
        }
    }


    /**********************************************************
     * Reset coin locations and instantiate coins in each
     * quadrant except the one the player is in.
     *********************************************************/

    void SpawnCoins()
    {
        coinLocations.Clear();
        int middle = spawnCount / 2;
        SpawnCoin(2, middle - 3, 2, middle - 3);
        SpawnCoin(2, middle - 3, middle + 3, spawnCount - 3);
        SpawnCoin(middle + 3, spawnCount - 2, middle + 3, spawnCount - 3);
        SpawnCoin(middle + 3, spawnCount - 2, 2, middle - 3);
    }


    /**********************************************************
     * Randomly spawn a coin in the given space constraints
     *********************************************************/

    void SpawnCoin(int xStart, int xEnd, int yStart, int yEnd)
    {
        //Avoid spawning coin in the player's current quadrant
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
        spawnArray[x, y] = 
            Instantiate(coinPrefab, 
                new Vector3(transform.position.x + ((float)x * tileLength - StartPos), 
                    transform.position.y + ((float)y * tileLength - StartPos), 0), 
                    this.transform.rotation);

        spawnArray[x, y].transform.parent = this.gameObject.transform;
        coinLocations.Add(new MapTile(x, y));
    }


    /********************************************************
     * Spawn lava blocks throughout map.
     *******************************************************/
    void SpawnLava(int lavaCount)
    {
        int i = 0;
        List<MapTile> addedTiles = new List<MapTile>();

        //Instantiate lava blocks
        while (i < lavaCount)
        {
            //Choose a random tile index
            int currIndex = Random.Range(0, spawnList.Count);

            //Choose how many lava blocks to instantiate in a row from 1 to 4
            int lavaLength = Random.Range(1, 4);

            int row, col, changeDir = 0;
            bool validIndex = true;
            if (lavaLength + i >= lavaCount)
                lavaLength = lavaCount - i;
            for (int j = 0; j < lavaLength; j++)
            {
                //Iterate through all indices until reaching valid tile
                int loopCount = 0;
                do
                {
                    validIndex = true;

                    //Set next index
                    if (currIndex >= spawnList.Count)
                        currIndex = 0;
                    row = spawnList[currIndex].row;
                    col = spawnList[currIndex].col;
                    currIndex++;

                    //Check if current indexed tile is valid
                    if (spawnArray[row, col] != null)
                        validIndex = false;
                    else
                    {
                        //Get the best direction for lava block to face. If no direction is possible, skip index
                        changeDir = CheckLava(row, col);
                        if (changeDir == -1)
                            validIndex = false;
                    }

                    loopCount++;
                    //If every lava block is invalid for whatever reason, return and stop spawning lava
                    if (loopCount >= spawnList.Count)
                        return;

                } while (!validIndex);

                //Make sure that tile that's needed is null just in case
                Destroy(spawnArray[row, col]);
                spawnArray[row, col] = null;

                //Instantiate lava block at valid index
                spawnArray[row, col] = 
                    Instantiate(lavaPrefab, 
                        new Vector3(transform.position.x + ((float)row * tileLength - StartPos), 
                        transform.position.y + ((float)col * tileLength - StartPos), 0), 
                        this.transform.rotation);

                spawnArray[row, col].transform.parent = this.gameObject.transform;
                addedTiles.Add(new MapTile(row, col));
                i++;
            }
        }

        //Rotate each lava block to face in the proper optimal direction
        //Remove if no optimal direction
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


    /***********************************************************************
     * Lava blocks spawn and fire projectiles in a single direction. This
     * function finds the optimal direction (farthest it can shoot) to face.
     * If every direction is blocked, the index is not valid. Also checks
     * general validity (not too close to player or on outside of map). 
     **********************************************************************/
    int CheckLava(int row, int col)
    {
        if (!isValidIndex(row, col))
            return -1;
        int left = 1; int right = 1; int up = 1; int down = 1; int max = 1;

        //Final Direction is returned as -1 if no direction is valid
        int finalDirection = -1;

        //Checks how far lava block can shoot upwards. If it can't, leaves final direction invalid
        while (col + up < spawnCount && spawnArray[row, col + up] == null) up++;
        if (up > max)
        {
            max = up;
            finalDirection = 0;
        }
        //Checks how far lava block can shoot downwards. If it can't, leaves final direction invalid
        while (col - down >= 0 && spawnArray[row, col - down] == null) down++;
        if (down > max)
        {
            max = down;
            finalDirection = 180;
        }
        //Checks how far lava block can shoot right. If it can't, leaves final direction invalid
        while (row + right < spawnCount && spawnArray[row + right, col] == null) right++;
        if (right > max)
        {
            max = right;
            finalDirection = -90;
        }
        //Checks how far lava block can shoot left. If it can't, leaves final direction invalid
        while (row - left >= 0 && spawnArray[row - left, col] == null) left++;
        if (left > max) finalDirection = 90;

        //returns direction that can shoot the furthest
        return finalDirection;
    }


    /**********************************************************************
     * Outside rows/columns, middle block of map array, and direct player
     * surroundings are not valid block spawn locations to allow player 
     * space to react on a new map spawn.
     *********************************************************************/
    bool isValidIndex(int row, int col)
    {
        int middle = spawnCount / 2;
        if (row >= middle - 1 && row <= middle + 1 && col >= middle - 1 && col <= middle + 1)
            return false;
        if (Mathf.Abs(row - startLocation.row) <= 2 && Mathf.Abs(col - startLocation.col) <= 2)
            return false;
        if (row == 0 || row == spawnCount - 1 || col == 0 || col == spawnCount - 1)
            return false;
        return true;
    }


    /***********************************************************************
     * Breadth First Search for checking if there is a valid path from
     * a start point (player location) to a destination (coin location).
     * If not, create one from closest valid point to destination with
     * call to CreatePath function
     **********************************************************************/
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
                return;
            if (Mathf.Abs(destination.row - currLocation.row) + Mathf.Abs(destination.col - currLocation.col) <=
                Mathf.Abs(destination.row - closestTile.row) + Mathf.Abs(destination.col - closestTile.col))
                closestTile = currLocation;
            MapTile nextLocation = new MapTile(currLocation.row + 1, currLocation.col);

            //Add values to queue
            if (currLocation.row + 1 < spawnCount - 1 
                && !visited.Contains(nextLocation) 
                && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
            nextLocation = new MapTile(currLocation.row - 1, currLocation.col);
            if (currLocation.row - 1 > 1 
                && !visited.Contains(nextLocation) 
                && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
            nextLocation = new MapTile(currLocation.row, currLocation.col + 1);
            if (currLocation.col + 1 < spawnCount - 1 
                && !visited.Contains(nextLocation) 
                && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
            nextLocation = new MapTile(currLocation.row, currLocation.col - 1);
            if (currLocation.col - 1 > 1 
                && !visited.Contains(nextLocation) 
                && spawnArray[nextLocation.row, nextLocation.col] == null)
            {
                visited.Add(nextLocation);
                BFS.Enqueue(nextLocation);
            }
        }
        CreatePath(closestTile, destination);
    }


    /********************************************************************
     * Creates most straightforward path from start point to destination
     * by destroying blocks along the way. Any coins along path are 
     * disregarded as they can be passed through.
     ******************************************************************/

    /********************************************************************************
     * NEEDS REFACTORING: For future updates, there may be other game 
     * objects that need to be disregarded. Updated function should be:
     
        CreatePath(MapTile start, MapTile destination, List<String> IgnoredObjects)
    
     * List<String> IgnoredObjects references any objects with tags that aren't
     * obstacles in a given path (e.g. coins). Refactoring allows for easier time
     * adding pass-throughable objects.
     *******************************************************************************/
    void CreatePath(MapTile start, MapTile destination)
    {
        int currRow = start.row;
        int currCol = start.col;
        while (currRow != destination.row || currCol != destination.col)
        {
            if (currRow != destination.row)
            {
                if (spawnArray[currRow, currCol] != null 
                    && spawnArray[currRow, currCol].gameObject.tag != "Coin")
                {
                    Destroy(spawnArray[currRow, currCol]);
                    spawnArray[currRow, currCol] = null;
                }
                if (currRow < destination.row)
                    currRow++;
                else
                    currRow--;
            }
            if (currCol != destination.col)
            {
                if (spawnArray[currRow, currCol] != null 
                    && spawnArray[currRow, currCol].gameObject.tag != "Coin")
                {
                    Destroy(spawnArray[currRow, currCol]);
                    spawnArray[currRow, currCol] = null;
                }
                if (currCol < destination.col)
                    currCol++;
                else
                    currCol--;
            }
        }
    }


    /********************************************************************
     * Updates player's location to properly index the start location
     * for the breadth first search on a new map
     *******************************************************************/
    public void UpdateLocation(Coin currCoin)
    {
        for (int i = 0; i < coinLocations.Count; i++)
        {
            if (coinLocations[i].row == -1)
                continue;
            if (spawnArray[coinLocations[i].row, coinLocations[i].col] == currCoin.gameObject)
            {
                //player's current quadrant
                startQuadrant = i;
                //player's current tile index
                this.startLocation = new MapTile(coinLocations[i].row, coinLocations[i].col);
            }
        }
    }
}