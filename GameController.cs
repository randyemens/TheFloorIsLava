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

    // Start is called before the first frame update
    void Start()
    {
        tileMap = Instantiate(MapPrefab, new Vector3(0, 0, 0), this.transform.rotation);
        tileMap.transform.parent = this.gameObject.transform;
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
        if (Input.GetKeyDown("s") && !gameRunning) StartGame();
        if (Input.GetKeyDown("p") && gameRunning) PauseGame();
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

    public void AddCoin()
    {
        score += 10;
        coinTotal++;
        CheckLevel();
    }

    private void CheckLevel()
    {
        levelCoins = coinTotal % 3;
        if (levelCoins == 0)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                GameObject levelUp = Instantiate(addScorePrefab, player.transform.position, new Quaternion(0f, 0f, 0f, 0f));
                levelUp.GetComponent<AddScore>().SetAnimation(1);
            }
            score += 50 + timeBonus;
            timeBonus = 30;
            bonusTracker = 1f;
            currLevel++;
            if (currLevel > 50) {
                if (player != null) Destroy(player);
                EndGame(true);
                return;
            }
            Spawn updateMap = (Spawn)tileMap.transform.Find("Spawner").GetComponent(typeof(Spawn));
            updateMap.SpawnNewMap(currLevel);
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
        List<GameObject> lavaDrops = new List<GameObject>(GameObject.FindGameObjectsWithTag("LavaDrop"));
        for (int i = 0; i < lavaDrops.Count; i++)
        {
            if (lavaDrops[i] != null) Destroy(lavaDrops[i]);
        }
        score = 0;
        currLevel = 1;
        coinTotal = 0;
        timeBonus = 30;
        bonusTracker = 1f;
        Spawn updateMap = (Spawn)tileMap.transform.Find("Spawner").GetComponent(typeof(Spawn));
        updateMap.StartGame();
        LavaController updateLavaArray = (LavaController)tileMap.transform.Find("LavaController").GetComponent(typeof(LavaController));
        updateLavaArray.SetFrequency(currLevel);
        gameRunning = true;
        paused = false;
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
        Score.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        Level.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        LevelContainer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        ScoreIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        StartLine.GetComponent<StartLine>().showStart = true;
        Spawn updateMap = (Spawn)tileMap.transform.Find("Spawner").GetComponent(typeof(Spawn));
        LavaController updateLavaArray = (LavaController)tileMap.transform.Find("LavaController").GetComponent(typeof(LavaController));
        updateLavaArray.SetFrequency(0);
        gameRunning = false;
        GameOver.GetComponent<Animator>().SetBool("Win", winCheck);
        GameOver.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        FinalScore.ShowScore(score);
    }

    private void PauseGame()
    {
        paused = !paused;
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
