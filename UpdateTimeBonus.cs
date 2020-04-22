using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTimeBonus : MonoBehaviour
{
    GameObject GameController;
    public Text bonusText;
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        GameController gameScript = (GameController)GameController.GetComponent(typeof(GameController));
        if (gameScript.GetTimeBonus() == -1) bonusText.text = "";
        else bonusText.text = "TB - " + gameScript.GetTimeBonus().ToString();
    }
}
