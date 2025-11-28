using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Room roomPrefab;
    [SerializeField] int totalRooms = 20;

    Dictionary<(int x, int y), Room> rooms = new Dictionary<(int, int), Room>();
    List<(int x, int y)> availableRoomLocations = new List<(int x, int y)>();


    private void Start()
    {
        // create first room at 0,0
        CreateRoom((0, 0), roomPrefab);

        // add totalRooms
        for (int i = 0; i < totalRooms - 1; i++)
        {
            // skip if no available locations
            if (availableRoomLocations.Count == 0) break;

            // find random room
            int randRoomIndex = Random.Range(0, availableRoomLocations.Count);
            var (randX, randY) = availableRoomLocations[randRoomIndex];

            // create a room there
            Room newRoom = CreateRoom((randX, randY), roomPrefab);

            // Basic Direction Vectors
            Vector2[] dirs = new Vector2[]
            {
                new Vector2( 1,  0), // east
                new Vector2(-1,  0), // west
                new Vector2( 0,  1), // north
                new Vector2( 0, -1)  // south
            };

            foreach (var dir in dirs)
            {
                // adjust by the direction
                int nx = randX + (int)dir.x;
                int ny = randY + (int)dir.y;

                // skip if the room doesn't exist
                if (!rooms.ContainsKey((nx, ny)))
                    continue;
                Room adjacentRoom = rooms[(nx, ny)];

                // find the new room and adjacent room's exits
                RoomExit exitNew = FindExitFacing(newRoom, dir);
                RoomExit exitAdjacent = FindExitFacing(adjacentRoom, -dir);

                // create and enable the transition
                RoomTransition transition = new RoomTransition(exitNew, exitAdjacent);
                transition.Enable();
            }
        }
    }

    private Room CreateRoom((int x, int y) coordinate, Room roomToInstantiate)
    {
        // validate the room is valid
        if (rooms.ContainsKey(coordinate))
            Debug.LogError("Attempted To Create Room in Occupied Space");

        // create the room object
        Room newRoom = Instantiate(roomToInstantiate);

        // convert coordinate to real-world position
        newRoom.transform.position = new Vector3(coordinate.x * 50, 0, coordinate.y * 50);


        // update rooms
        rooms[coordinate] = newRoom;
        availableRoomLocations.Remove(coordinate);

        // add new available positions (north, east, west, south)
        AddIfAvailable((coordinate.x + 1, coordinate.y)); // East
        AddIfAvailable((coordinate.x - 1, coordinate.y)); // West
        AddIfAvailable((coordinate.x, coordinate.y + 1)); // North
        AddIfAvailable((coordinate.x, coordinate.y - 1)); // South

        return newRoom;
    }

    // finds exit in the room that faces the specified direction
    private RoomExit FindExitFacing(Room room, Vector2 dir)
    {
        Hallway[] hallways = room.GetComponentsInChildren<Hallway>(true);

        // search for matching direction
        foreach (var hallway in hallways)
        {
            if (hallway.direction == dir)
                return hallway.roomExit;
        }
        Debug.LogError("Failed To Find Exit");
        return null;
    }

    // adds the room if there are no conflicts
    private void AddIfAvailable((int x, int y) coord)
    {
        if (rooms.ContainsKey(coord)) return;                 // Already occupied  
        if (availableRoomLocations.Contains(coord)) return;   // Already added  

        availableRoomLocations.Add(coord);
    }
}