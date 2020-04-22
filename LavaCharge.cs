using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCharge : MonoBehaviour
{
    private bool fire = false;
    private float charge = 1.0f;
    private float cooldown = 0f;
    public GameObject LavaDrop;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
            charge -= Time.deltaTime / 2;
        if (charge < 0.0f)
        {
            GameObject SpawnDrop = Instantiate(LavaDrop, new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation);
            SpawnDrop.transform.parent = this.gameObject.transform;
            fire = false;
            charge = 1.0f;
            cooldown = 0.5f;
        }
        if (cooldown > 0) cooldown -= Time.deltaTime * 2;
        else cooldown = 0f;
        this.transform.Find("LavaShot").gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, cooldown * 2);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - charge);
    }

    public void Fire()
    {
        fire = true;
    }
}
