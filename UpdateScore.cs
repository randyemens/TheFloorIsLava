/***********************************************************************************
 * TFIS: Score Text
 * Created by Randel Emens
 * Description: Handles Score UI Display
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    GameObject GameController;
    public Text scoreText;

    void Start()
    {
        GameController = GameObject.Find("GameController");
    }


    void Update()
    {
        GameController gameScript = (GameController)GameController.GetComponent(typeof(GameController));
        scoreText.text = gameScript.GetScore().ToString() + " -";
    }
}
