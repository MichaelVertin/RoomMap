using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour
{
    private HallwayClosed hallwayClosed;
    private HallwayOpened hallwayOpened;
    public RoomExit roomExit;
    [SerializeField] public Vector2 direction;

    private void Awake()
    {
        roomExit = GetComponentInChildren<RoomExit>(true);
        hallwayClosed = GetComponentInChildren<HallwayClosed>(true);
        hallwayOpened = GetComponentInChildren<HallwayOpened>(true);
        Close();
    }

    public void Close()
    {
        hallwayClosed.gameObject.SetActive(true);
        hallwayOpened.gameObject.SetActive(false);
    }

    public void Open()
    {
        hallwayClosed.gameObject.SetActive(false);
        hallwayOpened.gameObject.SetActive(true);
    }
}
