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

        for (int i = 0; i < totalRooms - 1; i++)
        {
            if (availableRoomLocations.Count == 0) break;

            int randRoomIndex = Random.Range(0, availableRoomLocations.Count);
            var (randX, randY) = availableRoomLocations[randRoomIndex];

            // create a room there
            CreateRoom((randX, randY), roomPrefab);

            Room newRoom = rooms[(randX, randY)];

            // -----------------------------------------
            // CONNECT NEW ROOM TO ALL ADJACENT ROOMS
            // -----------------------------------------

            // These are the 4 cardinal directions
            Vector2[] dirs = new Vector2[]
            {
            new Vector2( 1,  0), // east
            new Vector2(-1,  0), // west
            new Vector2( 0,  1), // north
            new Vector2( 0, -1)  // south
            };

            foreach (var dir in dirs)
            {
                int nx = randX + (int)dir.x;
                int ny = randY + (int)dir.y;

                // no adjacent room? skip
                if (!rooms.ContainsKey((nx, ny)))
                    continue;

                Room adjacentRoom = rooms[(nx, ny)];

                // -----------------------------------------
                // Find exits facing each other
                // -----------------------------------------

                RoomExit exitNew = FindExitFacing(newRoom, dir);
                RoomExit exitAdjacent = FindExitFacing(adjacentRoom, -dir);

                if (exitNew == null || exitAdjacent == null)
                {
                    Debug.LogWarning($"Could not find exits for direction {dir}");
                    continue;
                }

                // Already connected? Skip.
                if (exitNew.roomTransition != null || exitAdjacent.roomTransition != null)
                    continue;

                // Create transition
                RoomTransition transition = new RoomTransition(exitNew, exitAdjacent);
                transition.Enable();
            }
        }
    }

    private void CreateRoom((int x, int y) coordinate, Room roomToInstantiate)
    {
        if (rooms.ContainsKey(coordinate))
            Debug.LogError("Attempted To Create Room in Occupied Space");

        // create object
        Room newRoom = Instantiate(roomToInstantiate);
        newRoom.transform.position = new Vector3(coordinate.x * 50, 0, coordinate.y * 50);

        // store in rooms
        rooms[coordinate] = newRoom;

        // remove position from available rooms
        availableRoomLocations.Remove(coordinate);

        // add new available positions (north, east, west, south)
        AddIfAvailable((coordinate.x + 1, coordinate.y)); // East
        AddIfAvailable((coordinate.x - 1, coordinate.y)); // West
        AddIfAvailable((coordinate.x, coordinate.y + 1)); // North
        AddIfAvailable((coordinate.x, coordinate.y - 1)); // South
    }
    private RoomExit FindExitFacing(Room room, Vector2 dir)
    {
        // Get all hallways in this room
        Hallway[] hallways = room.GetComponentsInChildren<Hallway>(true);

        foreach (var hallway in hallways)
        {
            if (hallway.direction == dir)   // match direction vector
                return hallway.roomExit;
        }
        Debug.LogError("Failed To Find Exit");
        return null; // none found
    }

    private void AddIfAvailable((int x, int y) coord)
    {
        if (rooms.ContainsKey(coord)) return;                 // Already occupied  
        if (availableRoomLocations.Contains(coord)) return;   // Already added  

        availableRoomLocations.Add(coord);
    }
}