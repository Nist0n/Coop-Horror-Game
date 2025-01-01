using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PCG
{
    public class Generate : MonoBehaviour
    {
        [SerializeField] private List<GameObject> rooms;
        [SerializeField] private List<GameObject> roomPrefabs;

        [SerializeField] private GameObject currentGameObject;

        [SerializeField] private int maxRooms;
        
        private Vector3 startPos;

        // [SerializeField] private List<GameObject> northRoomPrefabs;
        // [SerializeField] private List<GameObject> southRoomPrefabs;
        // [SerializeField] private List<GameObject> eastRoomPrefabs;
        // [SerializeField] private List<GameObject> westRoomPrefabs;

        [SerializeField] private GameObject openDoorBlockNS;
        [SerializeField] private GameObject openDoorBlockWE;

        [SerializeField] private Random.State seed;
        
        private void Awake()
        {
            startPos = transform.position;
        }

        private void Start()
        {
            seed = Random.state;
            // foreach (var roomPrefab in roomPrefabs)
            // {
            //     foreach (var door in roomPrefab.GetComponent<Room>().doors)
            //     {
            //         switch (door.doorPosition)
            //         {
            //             case DoorPosition.North:
            //                 northRoomPrefabs.Add(roomPrefab);
            //                 break;
            //             case DoorPosition.South:
            //                 southRoomPrefabs.Add(roomPrefab);
            //                 break;
            //             case DoorPosition.East:
            //                 eastRoomPrefabs.Add(roomPrefab);
            //                 break;
            //             case DoorPosition.West:
            //                 westRoomPrefabs.Add(roomPrefab);
            //                 break;
            //             default: // For universal (maybe change later)
            //                 northRoomPrefabs.Add(roomPrefab);
            //                 southRoomPrefabs.Add(roomPrefab);
            //                 westRoomPrefabs.Add(roomPrefab);
            //                 eastRoomPrefabs.Add(roomPrefab);
            //                 break;
            //         }
            //     }       
            // }
            
            for (int i = 0; i < maxRooms; i++)
            {
                currentGameObject = CreateRoom();
                rooms.Add(currentGameObject);
            }

            foreach (var roomGameObject in rooms)
            {
                Room room = roomGameObject.GetComponent<Room>();
                foreach (var door in room.doors)
                {
                    // Just for demo purposes (while doors are not yet real)
                    Vector3 correctedPos =
                        new Vector3(door.transform.position.x, 1, door.transform.position.z);
                    switch (door.doorPosition)
                    {
                        case DoorPosition.North:
                        case DoorPosition.South:
                            Instantiate(openDoorBlockNS, correctedPos, Quaternion.identity);
                            break;
                        default:
                            Instantiate(openDoorBlockWE, correctedPos, Quaternion.identity);
                            break;
                    }
                }
            }
        }

        private GameObject CreateRoom()
        {
            if (!currentGameObject) // The first room
            {
                List<GameObject> startRooms = roomPrefabs.FindAll(x => x.GetComponent<Room>().doors.Count > 1);
                GameObject room = startRooms[Random.Range(0, startRooms.Count)];
                return Instantiate(room, startPos, Quaternion.identity);
            }

            bool spaceTaken = false;
            
            Room currentRoom = currentGameObject.GetComponent<Room>();
            while (!currentRoom.doors.Any()) // If there are no doors other than the one we've come through (dead end)
            {
                int currentIndex = rooms.FindIndex(x => x == currentGameObject);
                currentGameObject = rooms[currentIndex - 1];
                Debug.Log(currentGameObject);
                currentRoom = currentGameObject.GetComponent<Room>();
            }
            
            Door randomDoor;
            
            Room chosenRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)].GetComponent<Room>();

            while (currentRoom.isHallway && chosenRoomPrefab.isHallway)
            {
                chosenRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)].GetComponent<Room>();
            }
            
            float offsetX = 0f;
            float offsetZ = 0f;
            
            Quaternion newRotation = Quaternion.identity;
            
            do
            {
                randomDoor = currentRoom.doors[Random.Range(0, currentRoom.doors.Count)];
                
                // We only need to rotate for East and West as by default prefabs face South and North is just 180 degrees from South
                switch (randomDoor.doorPosition)
                {
                    case DoorPosition.East:
                        newRotation = Quaternion.Euler(0, -90, 0);
                        break;
                    case DoorPosition.West:
                        newRotation = Quaternion.Euler(0, 90, 0);
                        break;
                }

                // Check if space is empty. Radius is a random chosen value for now
                // Produces an infinite cycle in some cases
                // if (Physics.CheckSphere(newPos, 3))
                // {
                //     spaceTaken = true;
                // }
                
            } while (spaceTaken);

            currentRoom.doors.Remove(randomDoor); // So that doors don't get closed afterward
            
            // if (maxRooms - rooms.Count == 1) // The last room doesn't have to be a dead end and
            //                                  // honestly dead ends should be introduced differently
            // {
            //     chosenRoomPrefab = roomPrefabs.Find(x => x.GetComponent<Room>().doors.Count == 0).GetComponent<Room>();
            //     Quaternion rotation = Quaternion.identity;
            //     switch (randomDoor.doorPosition)
            //     {
            //         case DoorPosition.North:
            //             offsetX = -chosenRoomPrefab.floorDimensions.localScale.x * 5;
            //             rotation = Quaternion.Euler(0, -180, 0);
            //             break;
            //         case DoorPosition.South:
            //             offsetX = chosenRoomPrefab.floorDimensions.localScale.x * 5;
            //             break;
            //         case DoorPosition.East:
            //             offsetZ = chosenRoomPrefab.floorDimensions.localScale.z * 5;
            //             rotation = Quaternion.Euler(0, -90, 0);
            //             break;
            //         default:
            //             offsetZ = -chosenRoomPrefab.floorDimensions.localScale.z * 5;
            //             rotation = Quaternion.Euler(0, 90, 0);
            //             break;
            //     }
            //
            //     newPos = new Vector3(randomDoorPos.x + offsetX, randomDoorPos.y, randomDoorPos.z + offsetZ);
            //     GameObject lastRoom = Instantiate(chosenRoomPrefab.gameObject, newPos, rotation);
            //     return lastRoom;
            // }
            
            Room createdRoom = Instantiate(chosenRoomPrefab.gameObject, Vector3.zero, newRotation).GetComponent<Room>();

            Vector3 truePos = randomDoor.transform.position + createdRoom.transform.position;
            Vector3 normalizedPos = truePos.normalized;
            Debug.Log(normalizedPos);
            if (normalizedPos == Vector3.left) // North
            {
                offsetX = -chosenRoomPrefab.floorDimensions.localScale.x * 5;
            }
            else if (normalizedPos == Vector3.right) // Sourh
            {
                offsetX = chosenRoomPrefab.floorDimensions.localScale.x * 5;
            }
            else if (normalizedPos == Vector3.forward) // East
            {
                offsetZ = chosenRoomPrefab.floorDimensions.localScale.z * 5;
            }
            else // West
            {
                offsetZ = -chosenRoomPrefab.floorDimensions.localScale.z * 5;
            }
            // switch (randomDoor.doorPosition)
            // {
            //     case DoorPosition.North:
            //         offsetX = -chosenRoomPrefab.floorDimensions.localScale.x * 5;
            //         break;
            //     case DoorPosition.South:
            //         
            //         break;
            //     case DoorPosition.East:
            //         
            //         break;
            //     case DoorPosition.West:
            //         
            //         break;
            // }
            
            // Change the position after rotation
            Vector3 randomDoorPos = randomDoor.transform.position;
            Vector3 newPos = new Vector3(randomDoorPos.x + offsetX, randomDoorPos.y, randomDoorPos.z + offsetZ);
            createdRoom.transform.position = newPos;
            
            List<Door> newRoomCorrespondingDoors = createdRoom.doors.FindAll(x => x.transform.position == randomDoor.transform.position);
            
            foreach (var door in newRoomCorrespondingDoors)
            {
                createdRoom.doors.Remove(door);
            }
            
            // createdRoom.transform.rotation = rotation;
            return createdRoom.gameObject;
        }
    }
}
