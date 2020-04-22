using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDropDespawn : MonoBehaviour
{
    float despawnTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        despawnTimer += Time.deltaTime * 4;
        //this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - despawnTimer);
    }

    void Despawn()
    {
        Destroy(this.gameObject);
    }
}
