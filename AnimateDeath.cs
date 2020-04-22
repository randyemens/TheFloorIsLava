using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDeath : MonoBehaviour
{
    public GameObject ballPrefab;
    float animateTimer = .5f;
    int animateCount = 0;
    float ballVelocity = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animateTimer += Time.deltaTime;
        if (animateTimer > .15f)
        {
            animateTimer = 0f;
            animateCount++;
            int diagonal = animateCount % 2;
            GameObject currBall = Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0f, 0f, 0f, 0f));
            currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ballVelocity * diagonal, ballVelocity);
            currBall = Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0f, 0f, 0f, 0f));
            currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ballVelocity * -diagonal, -ballVelocity);
            currBall = Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0f, 0f, 0f, 0f));
            currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ballVelocity, ballVelocity * -diagonal);
            currBall = Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0f, 0f, 0f, 0f));
            currBall.GetComponent<Rigidbody2D>().velocity = new Vector2(-ballVelocity, ballVelocity * diagonal);
        }
        if (animateCount >= 3 || Input.GetKeyDown("s")) Destroy(this.gameObject);
    }
}
