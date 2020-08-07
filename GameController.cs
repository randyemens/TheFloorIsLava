/***********************************************************************************
 * TFIS: Game Controller and Scoring
 * Created by Randel Emens
 * Description: Handles exterior game components (text displays/starting & pausing
 * game/scoring)
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject MapPrefab;
    public GameObject addScorePrefab;
    private GameObject tileMap;
    private GameObject Score;
    private GameObject Level;
    private GameObject LevelContainer;
    private GameObject ScoreIcon;
    private GameObject StartLine;
    private GameObject TitleCard;
    private GameObject GameOver;
    private FinalScore FinalScore;
    private GameObject Ctrls;
    private GameObject Pause;
    private int coinTotal = 0;
    private int levelCoins = 0;
    private int currLevel = 1;
    private int score = 0;
    public bool gameRunning = false;
    public bool paused;
    private int timeBonus;
    private float bonusTracker;
    
    //NEEDS REFACTORING: Separate display and controls so they are easier to manage

    void Start()
    {
        //Creates the map object for basic game setup
        tileMap = Instantiate(MapPrefab, new Vector3(0, 0, 0), this.transform.rotation);
        tileMap.transform.parent = this.gameObject.transform;

        //Sets up displays for text UI
        GameObject textCanvas = this.transform.Find("Canvas").gameObject;
        Score = textCanvas.transform.Find("Score").gameObject;
        Level = textCanvas.transform.Find("Level").gameObject;
        LevelContainer = this.transform.Find("LevelContainer").gameObject;
        ScoreIcon = this.transform.Find("ScoreIcon").gameObject;
        Score.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        Level.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        LevelContainer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        ScoreIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        StartLine = this.transform.Find("StartLine").gameObject;
        StartLine.GetComponent<StartLine>().showStart = true;
        TitleCard = this.transform.Find("TitleCard").gameObject;
        TitleCard.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        GameOver = this.transform.Find("GameOver").gameObject;
        GameOver.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        GameObject endCanvas = this.transform.Find("EndCanvas").Find("FinalScore").gameObject;
        FinalScore = (FinalScore)endCanvas.GetComponent(typeof(FinalScore));
        Ctrls = this.transform.Find("Ctrls").gameObject;
        Ctrls.GetComponent<SpriteRenderer>().color = new Color(.84f, .89f, .98f, .75f);
        Pause = this.transform.Find("Pause").gameObject;
        Pause.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Handle new game and pause button functionality
        if (Input.GetKeyDown("s") && !gameRunning)
            StartGame();
        if (Input.GetKeyDown("p") && gameRunning)
            PauseGame();
        if (!paused && gameRunning)
        {
            if (bonusTracker > 0) bonusTracker -= Time.deltaTime;
            else
            {
                bonusTracker = 1f;
                if (timeBonus > 0) timeBonus--;
            }
        }

    }

    //NEEDS REFACTORING: Move coin scoring to public variable so changing values is easier

    //Add coin to score
    public void AddCoin()
    {
        score += 10;
        coinTotal++;
        CheckLevel();
    }


    //If coin count hits 3, start next level and add time and score bonus
    private void CheckLevel()
    {
        levelCoins = coinTotal % 3;
        if (levelCoins == 0)
        {
            GameObject player = GameObject.FindWithTag("Player");

            //Display level up animation 
            if (player != null)
            {
                GameObject levelUp = Instantiate(addScorePrefab, player.transform.position, new Quaternion(0f, 0f, 0f, 0f));
                levelUp.GetComponent<AddScore>().SetAnimation(1);
            }

            //Update values
            score += 50 + timeBonus;
            timeBonus = 30;
            bonusTracker = 1f;
            currLevel++;

            //End game if level hits 50
            if (currLevel > 50) {
                if (player != null) Destroy(player);
                EndGame(true);
                return;
            }

            //Start a new map for next level
            Spawn updateMap = (Spawn)tileMap.transform.Find("Spawner").GetComponent(typeof(Spawn));
            updateMap.SpawnNewMap(currLevel);

            //Make projectiles faster
            LavaController updateLavaArray = (LavaController)tileMap.transform.Find("LavaController").GetComponent(typeof(LavaController));
            updateLavaArray.SetFrequency(currLevel);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetLevel()
    {
        return currLevel;
    }

    public int GetTimeBonus()
    {
        if (!gameRunning) return -1;
        return timeBonus;
    }

    private void StartGame()
    {
        //Remove lava drops so they don't hit players on a new game
        List<GameObject> lavaDrops = new List<GameObject>(GameObject.FindGameObjectsWithTag("LavaDrop"));
        for (int i = 0; i < lavaDrops.Count; i++)
        {
            if (lavaDrops[i] != null)
                Destroy(lavaDrops[i]);
        }

        //Reset game values
        score = 0;
        currLevel = 1;
        coinTotal = 0;
        timeBonus = 30;
        bonusTracker = 1f;

        //Spawn new map and start at level one
        Spawn updateMap = (Spawn)tileMap.transform.Find("Spawner").GetComponent(typeof(Spawn));
        updateMap.StartGame();

        //Reset projectile frequency
        LavaController updateLavaArray = (LavaController)tileMap.transform.Find("LavaController").GetComponent(typeof(LavaController));
        updateLavaArray.SetFrequency(currLevel);

        //Set game running value
        gameRunning = true;
        paused = false;

        //Display UI Changes for Starting Game
        //REFACTOR magic numbers
        Score.GetComponent<Text>().color = new Color(.84f, .89f, .98f, .75f);
        Level.GetComponent<Text>().color = new Color(.84f, .89f, .98f, 1f);
        LevelContainer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        ScoreIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        StartLine.GetComponent<StartLine>().showStart = false;
        TitleCard.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        GameOver.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        Ctrls.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .25f);
        FinalScore.HideScore();
    }

    public void EndGame(bool winCheck)
    {
        //Display UI Changes for Ending Game
        Score.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        Level.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        LevelContainer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        ScoreIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        StartLine.GetComponent<StartLine>().showStart = true;
        GameOver.GetComponent<Animator>().SetBool("Win", winCheck);
        GameOver.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        FinalScore.ShowScore(score);

        //Remove projectiles after game has ended
        LavaController updateLavaArray = (LavaController)tileMap.transform.Find("LavaController").GetComponent(typeof(LavaController));
        updateLavaArray.SetFrequency(0);

        //Set game running value
        gameRunning = false;
    }

    private void PauseGame()
    {
        paused = !paused;

        //Display UI Changes for Pausing Game
        if (paused)
        {
            Ctrls.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            Pause.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            Time.timeScale = 0f;
        }
        else
        {
            Ctrls.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .25f);
            Pause.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            Time.timeScale = 1f;
        }
    }
}
