/***********************************************************************************
 * TFIS: Coin Collection Removal
 * Created by Randel Emens
 * Description: Destroys game object after certain amount of time
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    float timer = 1.5f;
    void Update()
    {
        timer -= Time.deltaTime * 3;
        if (timer < 0) Destroy(this.gameObject);
    }
}
