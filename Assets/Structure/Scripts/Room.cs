using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Hallway[] hallways;

    private void Awake()
    {
        hallways = GetComponentsInChildren<Hallway>();
    }
}
