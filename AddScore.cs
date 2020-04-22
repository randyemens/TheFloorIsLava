using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    int animationType = 0;
    float timer = 1.5f;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        transform.position = new Vector3(transform.position.x, transform.position.y + .625f, 0f);
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime * 3;
        if (animationType == 1 && player != null) transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .625f, 0f);
        //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, timer);
        if (timer < 0) Destroy(this.gameObject);
    }

    public void SetAnimation(int animType)
    {
        animationType = animType;
        GetComponent<Animator>().SetInteger("Type", animType);
        if (animType > 0) timer = 3f;
        if (animType == 0) GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 1f);
        //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
