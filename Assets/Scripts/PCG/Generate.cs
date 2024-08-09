using System.Collections.Generic;
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

        [SerializeField] private List<GameObject> northRoomPrefabs;
        [SerializeField] private List<GameObject> southRoomPrefabs;
        [SerializeField] private List<GameObject> eastRoomPrefabs;
        [SerializeField] private List<GameObject> westRoomPrefabs;

        [SerializeField] private GameObject openDoorBlockNS;
        [SerializeField] private GameObject openDoorBlockWE;
        
        
        private void Awake()
        {
            startPos = transform.position;
        }

        private void Start()
        {
            foreach (var roomPrefab in roomPrefabs)
            {
                foreach (var door in roomPrefab.GetComponent<Room>().doors)
                {
                    switch (door.doorPosition)
                    {
                        case DoorPosition.North:
                            northRoomPrefabs.Add(roomPrefab);
                            break;
                        case DoorPosition.South:
                            southRoomPrefabs.Add(roomPrefab);
                            break;
                        case DoorPosition.East:
                            eastRoomPrefabs.Add(roomPrefab);
                            break;
                        case DoorPosition.West:
                            westRoomPrefabs.Add(roomPrefab);
                            break;
                    }
                }       
            }
            
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
                        new Vector3(door.transform.position.x, 1.1f, door.transform.position.z);
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
                List<GameObject> startRooms = roomPrefabs.FindAll(x => x.GetComponent<Room>().doors.Count >= 1);
                GameObject room = startRooms[Random.Range(0, startRooms.Count)];
                return Instantiate(room, startPos, Quaternion.identity);
            }

            bool spaceTaken = false;
            
            Room currentRoom = currentGameObject.GetComponent<Room>();
            Door randomDoor;
            
            Room chosenRoomPrefab;
            int prefabDoor;
            float offsetX = 0f;
            float offsetZ = 0f;
            
            Vector3 randomDoorPos;
            Vector3 newPos;
            
            do
            {
                randomDoor = currentRoom.doors[Random.Range(0, currentRoom.doors.Count)];
                switch (randomDoor.doorPosition)
                {
                    case DoorPosition.North:
                        chosenRoomPrefab = southRoomPrefabs[Random.Range(0, southRoomPrefabs.Count)].GetComponent<Room>();
                        prefabDoor = chosenRoomPrefab.doors.FindIndex(x => x.doorPosition == DoorPosition.South);
                        offsetX = -chosenRoomPrefab.floorDimensions.localScale.x * 5;
                        break;
                    case DoorPosition.South:
                        chosenRoomPrefab = northRoomPrefabs[Random.Range(0, northRoomPrefabs.Count)].GetComponent<Room>();
                        prefabDoor = chosenRoomPrefab.doors.FindIndex(x => x.doorPosition == DoorPosition.North);
                        offsetX = chosenRoomPrefab.floorDimensions.localScale.x * 5;
                        break;
                    case DoorPosition.East:
                        chosenRoomPrefab = westRoomPrefabs[Random.Range(0, westRoomPrefabs.Count)].GetComponent<Room>();
                        prefabDoor = chosenRoomPrefab.doors.FindIndex(x => x.doorPosition == DoorPosition.West);
                        offsetZ = chosenRoomPrefab.floorDimensions.localScale.z * 5;
                        break;
                    default:
                        chosenRoomPrefab = eastRoomPrefabs[Random.Range(0, eastRoomPrefabs.Count)].GetComponent<Room>();
                        prefabDoor = chosenRoomPrefab.doors.FindIndex(x => x.doorPosition == DoorPosition.East);
                        offsetZ = -chosenRoomPrefab.floorDimensions.localScale.z * 5;
                        break;
                }

                randomDoorPos = randomDoor.transform.position;
                newPos = new Vector3(randomDoorPos.x + offsetX, randomDoorPos.y, randomDoorPos.z + offsetZ);

                // Check if space is empty
                if (Physics.CheckSphere(newPos, 3.0f))
                {
                    spaceTaken = true;
                }
            } while (spaceTaken);

            currentRoom.doors.Remove(randomDoor); // So that doors don't get closed afterwards
            
            if (maxRooms - rooms.Count == 1) // The last room doesn't have to be a dead end and
                                             // honestly dead ends should be introduced differently
            {
                chosenRoomPrefab = roomPrefabs.Find(x => x.GetComponent<Room>().doors.Count == 0).GetComponent<Room>();
                Quaternion rotation = Quaternion.identity;
                switch (randomDoor.doorPosition)
                {
                    case DoorPosition.North:
                        offsetX = -chosenRoomPrefab.floorDimensions.localScale.x * 5;
                        rotation = Quaternion.Euler(0, -180, 0);
                        break;
                    case DoorPosition.South:
                        offsetX = chosenRoomPrefab.floorDimensions.localScale.x * 5;
                        break;
                    case DoorPosition.East:
                        offsetZ = chosenRoomPrefab.floorDimensions.localScale.z * 5;
                        rotation = Quaternion.Euler(0, -90, 0);
                        break;
                    default:
                        offsetZ = -chosenRoomPrefab.floorDimensions.localScale.z * 5;
                        rotation = Quaternion.Euler(0, 90, 0);
                        break;
                }

                newPos = new Vector3(randomDoorPos.x + offsetX, randomDoorPos.y, randomDoorPos.z + offsetZ);
                GameObject lastRoom = Instantiate(chosenRoomPrefab.gameObject, newPos, rotation);
                return lastRoom;
            }
            
            Room newRoom = Instantiate(chosenRoomPrefab.gameObject, newPos, Quaternion.identity).GetComponent<Room>();

            List<int> newRoomCorrespondingDoors = new List<int>();

            for (int i = 0; i < chosenRoomPrefab.doors.Count; i++)
            {
                if (chosenRoomPrefab.doors[i].transform.position.Equals(randomDoor.transform.position))
                {
                    newRoomCorrespondingDoors.Add(i);
                }
            }
            
            // foreach (var door in newRoomCorrespondingDoors)
            // {
            //     newRoom.doors.Remove(door);
            // }
            // newRoom.doors.RemoveAt(chosenDoor);
            return newRoom.gameObject;
        }
    }

    public enum DoorPosition
    {
        North,
        South,
        East,
        West
    }
}
