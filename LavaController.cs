using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    private List<GameObject> lavaBlocks;
    private float spawnTimer;
    private float timerReset = 1f;
    private bool fire;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = timerReset;
        SetFloor();
    }

    // Update is called once per frame
    void Update()
    {
        if (fire) spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            lavaBlocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Lava"));
            spawnTimer = timerReset;
            if (lavaBlocks.Count > 0)
            {
                int randomFire = Random.Range(0, lavaBlocks.Count);
                LavaCharge lavaScript = (LavaCharge)lavaBlocks[randomFire].transform.GetChild(0).GetComponent(typeof(LavaCharge));
                lavaScript.Fire();
            }
        } 
    }

    public void SetFrequency(int level)
    {
        if (level == 0) fire = false;
        else fire = true;
        timerReset = 1 / Mathf.Sqrt((float)level);
    }

    private void SetFloor()
    {
        LavaFloor setFloor = (LavaFloor)this.transform.Find("Lava_Floor").GetComponent(typeof(LavaFloor));
        setFloor.SetFloor();
    }
}
