/***********************************************************************************
 * REMOVE - NO LONGER NEEDED
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public LayerMask Despawn;
    public BoxCollider2D lavaCheck;
    private Rigidbody2D m_Rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float setYRotation = ((this.gameObject.transform.rotation.eulerAngles.z / 90) - 1) % 2;
        float setXRotation = -((this.gameObject.transform.rotation.eulerAngles.z / 90) - 2) % 2;
        m_Rigidbody2D.velocity = new Vector2(setXRotation, setYRotation);
        if (Physics2D.IsTouchingLayers(lavaCheck, Despawn))
        {
            Destroy(this.gameObject);
        }
    }
}
