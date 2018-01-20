using UnityEngine;
using System.Collections;

public class Camera_Follow : MonoBehaviour {

    public GameObject player;       //Public variable to store a reference to the player game object

    public float speed = 6.0f;
    
    void Update () {
        float interpolation = speed * Time.deltaTime;
        	
        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, player.transform.position.y, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x, interpolation);
        
        this.transform.position = position;
    }
}