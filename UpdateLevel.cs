/***********************************************************************************
 * TFIS: Level Text
 * Created by Randel Emens
 * Description: Handles Level UI Display
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLevel : MonoBehaviour
{
    GameObject GameController;
    public Text levelText;

    void Start()
    {
        GameController = GameObject.Find("GameController");
    }


    void Update()
    {
        GameController gameScript = (GameController)GameController.GetComponent(typeof(GameController));
        levelText.text = "lvl " + gameScript.GetLevel().ToString();
    }
}
