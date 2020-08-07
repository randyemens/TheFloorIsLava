/***********************************************************************************
 * TFIS: Map Rotation
 * Created by Randel Emens
 * Description: Handles rotation of map on A or D key press
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    float coolDown = 0f;
    float rotation = 0;
    public GameObject player;
    private GameController controllerScript;

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

    //Separate routine to keep game paused for a length of time while rotating map
    public IEnumerator RotateMap(float pauseTime, bool rotateDir)
    {
        //Set rotation direction
        float startRotation = this.transform.rotation.eulerAngles.z;
        float endRotation = startRotation + 90.0f;
        if (rotateDir)
            endRotation = startRotation - 90.0f;
        if (player != null)
            player.transform.parent = this.gameObject.transform;
        coolDown = .1f;

        //Keep game paused
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            //rotate map
            rotation = (Time.realtimeSinceStartup - startTime) * 4f;
            this.transform.eulerAngles = new Vector3(0, 0, endRotation - (endRotation - startRotation) * (1 - rotation));
            yield return 0;
        }

        //Finalize rotation
        this.transform.eulerAngles = new Vector3(0, 0, endRotation);

        //Set and finalize player rotation to be proper facing up
        if (player != null)
        {
            player.transform.parent = null;
            if (rotateDir) player.transform.eulerAngles = new Vector3(0, 0f, 0);
            else player.transform.eulerAngles = new Vector3(0, 180.0f, 0);
        }

        if (controllerScript.paused)
            Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerScript.paused) return;
        if (coolDown > 0f)
            coolDown -= Time.deltaTime;
        else
        {
            //Handle counterclockwise rotation request
            if (Input.GetKeyDown("a"))
            {
                player = GameObject.FindWithTag("Player");
                if (player == null) return;
                PlayerMovement stopJump = (PlayerMovement)player.transform.GetComponent(typeof(PlayerMovement));
                StartPause(false);
                stopJump.StopJump();
            }
            //Handle clockwise rotation request
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
