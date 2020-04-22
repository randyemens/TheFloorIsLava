using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDrop : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    public CircleCollider2D collideCheck;
    public LayerMask Despawn;
    public GameObject DespawnPrefab;
    private float oscillator = 0.0f;
    private int oscDir = -1;
    private float xPos;
    private Transform Child;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        float setYRotation = -((this.gameObject.transform.rotation.eulerAngles.z / 90) - 1) % 2;
        float setXRotation = ((this.gameObject.transform.rotation.eulerAngles.z / 90) - 2) % 2;
        xPos = transform.position.x;
        Child = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Child.position = transform.position + transform.right * oscillator;
        oscillator += Time.deltaTime * oscDir * 0.5f;
        if (oscillator > .05f) oscDir = -1;
        if (oscillator < -.05f) oscDir = 1;
        if (Physics2D.IsTouchingLayers(collideCheck, Despawn) && (Mathf.Abs(transform.position.x - transform.parent.position.x) > .625 || Mathf.Abs(transform.position.y - transform.parent.position.y) > .625))
        {
            GameObject SpawnDrop = Instantiate(DespawnPrefab, new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation);
            SpawnDrop.transform.parent = this.gameObject.transform.parent;
            Destroy(this.gameObject);
        }
        float setYRotation = -((int)(this.gameObject.transform.rotation.eulerAngles.z / 90) - 1) % 2;
        float setXRotation = ((int)(this.gameObject.transform.rotation.eulerAngles.z / 90) - 2) % 2;
        m_Rigidbody2D.velocity = new Vector2(setXRotation * 2.5f, setYRotation * 2.5f);
    }
}
