using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public BoxCollider2D playerCheck;
    public LayerMask Player;
    public GameObject collectedPrefab;
    public GameObject scorePrefab;
    // Start is called before the first frame update
    void Start()
    {
        float generate = Random.Range(0.0f, 1.0f);
        Animator anim = this.GetComponent<Animator>();
        anim.Play("Coin", 0, generate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.IsTouchingLayers(playerCheck, Player))
        {
            Spawn updateLocation = (Spawn)this.transform.parent.GetComponent(typeof(Spawn));
            updateLocation.UpdateLocation(this);
            GameObject GameController = GameObject.Find("GameController");
            GameController controllerScript = (GameController)GameController.GetComponent(typeof(GameController));
            controllerScript.AddCoin();
            //GameObject collected = Instantiate(collectedPrefab, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
            //collected.transform.parent = this.transform.parent.parent;
            GameObject addScore = Instantiate(scorePrefab, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
            addScore.GetComponent<AddScore>().SetAnimation(0);
            Destroy(this.gameObject);
        }
    }
}
