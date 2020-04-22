using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowScore(int score)
    {
        scoreText.text = score.ToString();
        this.gameObject.GetComponent<Text>().color = new Color(.84f, .89f, .98f, .7f);
    }

    public void HideScore()
    {
        this.gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
    }
}
