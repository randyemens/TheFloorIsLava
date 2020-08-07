/***********************************************************************************
 * TFIS: Death Ball
 * Created by Randel Emens
 * Description: Handles movement of ball that represents player death
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBall : MonoBehaviour
{
    float animateTimer = 1f;
    
    void Update()
    {
        animateTimer -= Time.deltaTime * 1.5f;
        float xDir = this.gameObject.GetComponent<Rigidbody2D>().velocity.x;
        float yDir = this.gameObject.GetComponent<Rigidbody2D>().velocity.y;
        if (animateTimer < 0f || Input.GetKeyDown("s")) Destroy(this.gameObject);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, animateTimer);
    }
}
