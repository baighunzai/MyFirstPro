using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController:MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnplayerDied;
    public static event PlayerDelegate OnplayerScored;

    public float tapforce = 10;
    public float tiltSmooth = 5;
    public Vector3 starpos;

    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;
     
    void Start()
    {  
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapforce, ForceMode2D.Force);

        }
      
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
       if(col.gameObject.tag == "Score Zone")
        {
            OnplayerScored();// event sent to game manager

            //register a score event
            //play a sound
        }
        if (col.gameObject.tag == "DeathZone")
        {
            rigidbody.simulated = false;
            //register a dead event
            //play a sound
            OnplayerDied();//event sent to game manager
        }

    }
}
    

