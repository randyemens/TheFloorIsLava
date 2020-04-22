using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    float coolDown = 0f;
    float rotation = 0;
    public GameObject player;
    private GameController controllerScript;
    // Start is called before the first frame update
    void Start()
    {
        GameObject GameController = GameObject.Find("GameController");
        controllerScript = (GameController)GameController.GetComponent(typeof(GameController));
    }

    public void StartPause(bool rotateDir)
    {
        // how many seconds to pause the game
        StartCoroutine(RotateMap(.25f, rotateDir));
    }
    public IEnumerator RotateMap(float pauseTime, bool rotateDir)
    {
        float startRotation = this.transform.rotation.eulerAngles.z;
        float endRotation = startRotation + 90.0f;
        if (rotateDir) endRotation = startRotation - 90.0f;
        if (player != null) player.transform.parent = this.gameObject.transform;
        coolDown = .1f;
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            rotation = (Time.realtimeSinceStartup - startTime) * 4f;
            this.transform.eulerAngles = new Vector3(0, 0, endRotation - (endRotation - startRotation) * (1 - rotation));
            yield return 0;
        }
        this.transform.eulerAngles = new Vector3(0, 0, endRotation);
        if (player != null)
        {
            player.transform.parent = null;
            if (rotateDir) player.transform.eulerAngles = new Vector3(0, 0f, 0);
            else player.transform.eulerAngles = new Vector3(0, 180.0f, 0);
        }
        if (controllerScript.paused) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerScript.paused) return;
        if (coolDown > 0f) coolDown -= Time.deltaTime;
        else
        {
            if (Input.GetKeyDown("a"))
            {
                player = GameObject.FindWithTag("Player");
                if (player == null) return;
                PlayerMovement stopJump = (PlayerMovement)player.transform.GetComponent(typeof(PlayerMovement));
                StartPause(false);
                stopJump.StopJump();
            }
            else if (Input.GetKeyDown("d"))
            {
                player = GameObject.FindWithTag("Player");
                if (player == null) return;
                PlayerMovement stopJump = (PlayerMovement)player.transform.GetComponent(typeof(PlayerMovement));
                StartPause(true);
                stopJump.StopJump();
            }
        }
    }
}
