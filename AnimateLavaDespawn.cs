using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLavaDespawn : MonoBehaviour
{
    public GameObject ballPrefab;
    float animateTimer = .5f;
    int animateCount = 0;
    float ballVelocity = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
                currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ballVelocity * diagonal, ballVelocity);
                currBall = Instantiate(ballPrefab, this.transform);
                currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ballVelocity * -diagonal, -ballVelocity);
                currBall = Instantiate(ballPrefab, this.transform);
                currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ballVelocity, ballVelocity * -diagonal);
                currBall = Instantiate(ballPrefab, this.transform);
                currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(-ballVelocity, ballVelocity * diagonal);
            }
            animateCount++;
        }
        if (animateCount >= 5) Destroy(this.gameObject);
    }
}
