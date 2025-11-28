using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition
{
    private RoomExit e1, e2;
    public RoomTransition(RoomExit exit1, RoomExit exit2)
    {
        if(exit1 == null || exit2 == null)
        {
            Debug.LogError("Null Exit");
        }
        e1 = exit1;
        e2 = exit2;
    }

    public void Enable()
    {
        if (e1.roomTransition != null || e2.roomTransition != null)
        {
            Debug.LogError("Transition Already Exists!");
        }
        e1.hallway.Open();
        e2.hallway.Open();
        e1.roomTransition = this;
        e2.roomTransition = this;
    }

    public void Disable()
    {
        if (e1.roomTransition == null || e2.roomTransition == null)
        {
            Debug.LogError("Transition Not Enabled!");
        }
        e1.hallway.Close();
        e2.hallway.Close();
        e1.roomTransition = null;
        e2.roomTransition = null;
    }

    public RoomExit Travel(RoomExit departureExit)
    {
        if(departureExit == e1)
        {
            return e2;
        }
        if(departureExit == e2)
        {
            return e1;
        }
        return null;
    }
}
