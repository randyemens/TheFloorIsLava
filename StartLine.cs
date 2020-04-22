using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    float opacityTimer = 0f;
    int direction = 1;
    public bool showStart = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        opacityTimer += Time.deltaTime * direction / 2;
        if (opacityTimer > .25f) direction = -1;
        if (opacityTimer < -.25f) direction = 1;
        if (showStart) this.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f + opacityTimer);
        else this.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }
}
