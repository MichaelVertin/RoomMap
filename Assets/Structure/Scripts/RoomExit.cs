using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public RoomTransition roomTransition = null;
    public Hallway hallway = null;

    private void Awake()
    {
        hallway = GetComponentInParent<Hallway>();
        if(hallway == null)
        {
            Debug.LogError("Could Not Find hallway");
        }
    }
}
