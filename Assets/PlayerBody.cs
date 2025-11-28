using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        RoomExit departureExit = other.GetComponent<RoomExit>();
        if(departureExit != null)
        {
            RoomExit destinationExit = departureExit.roomTransition.Travel(departureExit);
            Hallway destinationHallway = destinationExit.hallway;

            Vector2 direction = destinationHallway.direction;

            this.transform.position = destinationExit.transform.position + new Vector3(direction.x, 0, direction.y) * -3f;
        }
    }
}
