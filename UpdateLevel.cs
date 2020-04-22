using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLevel : MonoBehaviour
{
    GameObject GameController;
    public Text levelText;
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        GameController gameScript = (GameController)GameController.GetComponent(typeof(GameController));
        levelText.text = "lvl " + gameScript.GetLevel().ToString();
    }
}
