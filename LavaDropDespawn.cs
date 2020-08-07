using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDropDespawn : MonoBehaviour
{
    float despawnTimer = 0f;

    void Update()
    {
        despawnTimer += Time.deltaTime * 4;
    }

    void Despawn()
    {
        Destroy(this.gameObject);
    }
}
