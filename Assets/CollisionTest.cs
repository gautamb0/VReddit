using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CollisionTest : MonoBehaviour
{

    public UnityEvent onPress;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision");
    }

    void OnTriggerEnter(Collider col)
    {

        Debug.Log("TRIGGER " + col);
        onPress.Invoke();
     
    }
}
