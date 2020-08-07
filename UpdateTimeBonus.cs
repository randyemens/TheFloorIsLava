/***********************************************************************************
 * TFIS: Time Bonus Text
 * Created by Randel Emens
 * Description: Handles Time Bonus UI Display
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTimeBonus : MonoBehaviour
{
    GameObject GameController;
    public Text bonusText;

    void Start()
    {
        GameController = GameObject.Find("GameController");
    }


    void Update()
    {
        GameController gameScript = (GameController)GameController.GetComponent(typeof(GameController));
        if (gameScript.GetTimeBonus() == -1) bonusText.text = "";
        else bonusText.text = "TB - " + gameScript.GetTimeBonus().ToString();
    }
}
