using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    float timer = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime * 3;
        //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, timer);
        //transform.localScale = new Vector3((3.5f - timer) / 2, (3.5f - timer) / 2, 1f);
        //transform.Rotate(0f, 0f, 10f);
        if (timer < 0) Destroy(this.gameObject);
    }
}
