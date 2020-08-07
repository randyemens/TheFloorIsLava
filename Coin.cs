/***********************************************************************************
 * TFIS: Coin
 * Created by Randel Emens
 * Description: Handles functionality of the coin
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public BoxCollider2D playerCheck;
    public LayerMask Player;
    public GameObject collectedPrefab;
    public GameObject scorePrefab;

    void Start()
    {
        //Start animation at a random frame
        float generate = Random.Range(0.0f, 1.0f);
        Animator anim = this.GetComponent<Animator>();
        anim.Play("Coin", 0, generate);
    }


    void Update()
    {
        //Handle player collection
        if (Physics2D.IsTouchingLayers(playerCheck, Player))
        {
            //Lets the map spawner know location of player
            Spawn updateLocation = (Spawn)this.transform.parent.GetComponent(typeof(Spawn));
            updateLocation.UpdateLocation(this);

            //Adds coin to game controller to handle count and score of coins
            GameObject GameController = GameObject.Find("GameController");
            GameController controllerScript = (GameController)GameController.GetComponent(typeof(GameController));
            controllerScript.AddCoin();

            //Animates collection of coin
            GameObject addScore = Instantiate(scorePrefab, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
            addScore.GetComponent<AddScore>().SetAnimation(0);

            //Remove coin
            Destroy(this.gameObject);
        }
    }
}
