/***********************************************************************************
 * TFIS: Lava Despawner Animation
 * Created by Randel Emens
 * Description: Handles instantiation of lava despawn
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLavaDespawn : MonoBehaviour
{
    public GameObject ballPrefab;
    float animateTimer = .5f;
    int animateCount = 0;
    float ballVelocity = 2f;
    
    void Update()
    {
        animateTimer += Time.deltaTime;
        if (animateTimer > .1f)
        {
            animateTimer = 0f;
            int diagonal = animateCount % 2;
            if (animateCount < 2)
            {
                GameObject currBall = Instantiate(ballPrefab, this.transform);
                currBall.GetComponent<Rigidbody2D>().velocity = 
                    new Vector2(ballVelocity * diagonal, ballVelocity);
                currBall = Instantiate(ballPrefab, this.transform);
                currBall.GetComponent<Rigidbody2D>().velocity = 
                    new Vector2(ballVelocity * -diagonal, -ballVelocity);
                currBall = Instantiate(ballPrefab, this.transform);
                currBall.GetComponent<Rigidbody2D>().velocity = 
                    new Vector2(ballVelocity, ballVelocity * -diagonal);
                currBall = Instantiate(ballPrefab, this.transform);
                currBall.GetComponent<Rigidbody2D>().velocity = 
                    new Vector2(-ballVelocity, ballVelocity * diagonal);
            }
            animateCount++;
        }

        //On 5th iteration of lava animation instantiation, end animation creation
        if (animateCount >= 5) Destroy(this.gameObject);
    }
}
