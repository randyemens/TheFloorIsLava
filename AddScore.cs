/***********************************************************************************
 * TFIS: Score Handler for Player
 * Created by Randel Emens
 * Description: Handles animation for added scores above player
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{

    /*
     * NEEDS REFACTORING: Animation types need to be added as public array so 
     * that adding new types of scoring may be easier
     */

    int animationType = 0;
    float timer = 1.5f;
    private GameObject player;
    public float disFromPlayer = .625f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + disFromPlayer, 0f);
        player = GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
        timer -= Time.deltaTime * 3;

        //TYPE 1: If player collects a coin
        if (animationType == 1 && player != null)
            transform.position = 
                new Vector3(player.transform.position.x, 
                    player.transform.position.y + disFromPlayer, 
                    0f);

        if (timer < 0) Destroy(this.gameObject);
    }

    //Set type of score card whenever score is incremented
    public void SetAnimation(int animType)
    {
        animationType = animType;
        GetComponent<Animator>().SetInteger("Type", animType);
        if (animType > 0)
            timer = 3f;
        if (animType == 0)
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 1f);
    }
}
