using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
    public LayerMask Player;
    public BoxCollider2D collideCheck;
    private Rigidbody2D m_Rigidbody2D;
    private float lightTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        updateLight();
        float setYRotation = ((this.gameObject.transform.rotation.eulerAngles.z / 90) - 1) % 2;
        float setXRotation = -((this.gameObject.transform.rotation.eulerAngles.z / 90) - 2) % 2;
        /*m_Rigidbody2D.velocity = new Vector2(setXRotation, setYRotation);
        if (Physics2D.IsTouchingLayers(collideCheck, Despawn))
        {
            Destroy(this.gameObject);
        }*/
        if (Physics2D.IsTouchingLayers(collideCheck, Player))
            lightTime = .5f;
    }

    private void updateLight()
    {
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, lightTime * 2);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, lightTime + .5f);
        if (lightTime > 0f) lightTime -= Time.deltaTime * 2;
        else lightTime = 0f;
    }
}
