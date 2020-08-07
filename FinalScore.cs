/***********************************************************************************
 * TFIS: Final Score Display
 * Created by Randel Emens
 * Description: Displays final score
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//NEEDS REFACTORING: Make floating values public variables for easy changing in Unity UI
public class FinalScore : MonoBehaviour
{
    public Text scoreText;

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
