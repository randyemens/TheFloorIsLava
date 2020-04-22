using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    GameObject GameController;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        GameController gameScript = (GameController)GameController.GetComponent(typeof(GameController));
        scoreText.text = gameScript.GetScore().ToString() + " -";
    }
}
