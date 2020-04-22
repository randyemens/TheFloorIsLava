using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    public GameObject lavaPrefab;
    public GameObject spawnerPrefab;
    public int spawnCount = 19;
    private float xStartPos;
    Vector3 setUpperBounds;
    Vector3 setBottomBounds;
    Vector3 setLeftBounds;
    Vector3 setRightBounds;
    public GameObject[] bottomArray;
    public GameObject[] leftArray;
    public GameObject[] rightArray;
    public GameObject[] upperArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFloor()
    {
        xStartPos = spawnCount * .625f / 2 - .3125f;
        upperArray = new GameObject[spawnCount];
        bottomArray = new GameObject[spawnCount];
        leftArray = new GameObject[spawnCount];
        rightArray = new GameObject[spawnCount];
        setUpperBounds = new Vector3(0, (xStartPos + .625f), 0);
        setBottomBounds = new Vector3(0, -(xStartPos + .625f), 0);
        setLeftBounds = new Vector3(-(xStartPos + .625f), 0, 0);
        setRightBounds = new Vector3((xStartPos + .625f), 0, 0);
        for (int i = 0; i < spawnCount; i++)
        {
            float generate = Random.Range(0.0f, 1.0f);
            bottomArray[i] = Instantiate(lavaPrefab, new Vector3(transform.position.x + ((float)i * .625f - xStartPos), transform.position.y, 0), this.transform.rotation);
            bottomArray[i].transform.position += setBottomBounds;
            bottomArray[i].transform.parent = this.gameObject.transform;
            Animator anim = bottomArray[i].GetComponent<Animator>();
            anim.Play("Lava", 0, generate);

            generate = Random.Range(0.0f, 1.0f);
            upperArray[i] = Instantiate(lavaPrefab, new Vector3(transform.position.x + ((float)i * .625f - xStartPos), transform.position.y, 0), this.transform.rotation);
            upperArray[i].transform.position += setUpperBounds;
            upperArray[i].transform.Rotate(0.0f, 0.0f, 180f);
            upperArray[i].transform.parent = this.gameObject.transform;
            anim = upperArray[i].GetComponent<Animator>();
            anim.Play("Lava", 0, generate);

            generate = Random.Range(0.0f, 1.0f);
            leftArray[i] = Instantiate(lavaPrefab, new Vector3(transform.position.x, transform.position.y + ((float)i * .625f - xStartPos), 0), this.transform.rotation);
            leftArray[i].transform.position += setLeftBounds;
            leftArray[i].transform.Rotate(0.0f, 0.0f, -90f);
            leftArray[i].transform.parent = this.gameObject.transform;
            anim = leftArray[i].GetComponent<Animator>();
            anim.Play("Lava", 0, generate);

            generate = Random.Range(0.0f, 1.0f);
            rightArray[i] = Instantiate(lavaPrefab, new Vector3(transform.position.x, transform.position.y + ((float)i * .625f - xStartPos), 0), this.transform.rotation);
            rightArray[i].transform.position += setRightBounds;
            rightArray[i].transform.Rotate(0.0f, 0.0f, 90f);
            rightArray[i].transform.parent = this.gameObject.transform;
            anim = rightArray[i].GetComponent<Animator>();
            anim.Play("Lava", 0, generate);
        }
        GameObject corner = Instantiate(spawnerPrefab, transform.position + setUpperBounds + setLeftBounds + new Vector3(-.1f, .1f, 0f), this.transform.rotation);
        corner.transform.parent = this.gameObject.transform;
        corner = Instantiate(spawnerPrefab, transform.position + setUpperBounds + setRightBounds + new Vector3(.1f, .1f, 0f), this.transform.rotation);
        corner.transform.parent = this.gameObject.transform;
        corner = Instantiate(spawnerPrefab, transform.position + setBottomBounds + setLeftBounds + new Vector3(-.1f, -.1f, 0f), this.transform.rotation);
        corner.transform.parent = this.gameObject.transform;
        corner = Instantiate(spawnerPrefab, transform.position + setBottomBounds + setRightBounds + new Vector3(.1f, -.1f, 0f), this.transform.rotation);
        corner.transform.parent = this.gameObject.transform;
    }
}
