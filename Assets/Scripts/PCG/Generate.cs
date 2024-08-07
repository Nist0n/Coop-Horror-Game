using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
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

        private int openDoors;
        
        // while (openDoors > 0)
        
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
        }

        private GameObject CreateRoom()
        {
            if (!currentGameObject)
            {
                List<GameObject> startRooms = roomPrefabs.FindAll(x => x.GetComponent<Room>().doors.Count >= 1);
                GameObject room = startRooms[Random.Range(0, startRooms.Count)];
                return Instantiate(room, startPos, Quaternion.identity);
            }
            
            Room currentRoom = currentGameObject.GetComponent<Room>();
            Door randomDoor = currentRoom.doors[Random.Range(0, currentRoom.doors.Count)];

            Room chosenRoom;
            int chosenDoor;
            float offsetX = 0f;
            float offsetZ = 0f;
            
            switch (randomDoor.doorPosition)
            {
                case DoorPosition.North:
                    chosenRoom = southRoomPrefabs[Random.Range(0, southRoomPrefabs.Count)].GetComponent<Room>();
                    chosenDoor = chosenRoom.doors.FindIndex(x => x.doorPosition == DoorPosition.South);
                    
                    offsetX = -chosenRoom.floorDimensions.localScale.x * 5;
                    break;
                case DoorPosition.South:
                    chosenRoom = northRoomPrefabs[Random.Range(0, northRoomPrefabs.Count)].GetComponent<Room>();
                    chosenDoor = chosenRoom.doors.FindIndex(x => x.doorPosition == DoorPosition.North);
                    
                    offsetX = chosenRoom.floorDimensions.localScale.x * 5;
                    break;
                case DoorPosition.East:
                    chosenRoom = westRoomPrefabs[Random.Range(0, westRoomPrefabs.Count)].GetComponent<Room>();
                    chosenDoor = chosenRoom.doors.FindIndex(x => x.doorPosition == DoorPosition.West);

                    offsetZ = chosenRoom.floorDimensions.localScale.z * 5;
                    break;
                default:
                    chosenRoom = eastRoomPrefabs[Random.Range(0, eastRoomPrefabs.Count)].GetComponent<Room>();
                    chosenDoor = chosenRoom.doors.FindIndex(x => x.doorPosition == DoorPosition.East);
                    
                    offsetZ = -chosenRoom.floorDimensions.localScale.z * 5;
                    break;
            }
            
            Vector3 randomDoorPos = randomDoor.transform.position;
            Vector3 newPos = new Vector3(randomDoorPos.x + offsetX, randomDoorPos.y, randomDoorPos.z + offsetZ);

            if (maxRooms - rooms.Count == 1)
            {
                chosenRoom = roomPrefabs.Find(x => x.GetComponent<Room>().doors.Count == 0).GetComponent<Room>();
                Quaternion rotation = Quaternion.identity;
                switch (randomDoor.doorPosition)
                {
                    case DoorPosition.North:
                        offsetX = -chosenRoom.floorDimensions.localScale.x * 5;
                        rotation = Quaternion.Euler(0, -180, 0);
                        break;
                    case DoorPosition.South:
                        offsetX = chosenRoom.floorDimensions.localScale.x * 5;
                        break;
                    case DoorPosition.East:
                        offsetZ = chosenRoom.floorDimensions.localScale.z * 5;
                        rotation = Quaternion.Euler(0, -90, 0);
                        break;
                    default:
                        offsetZ = -chosenRoom.floorDimensions.localScale.z * 5;
                        rotation = Quaternion.Euler(0, 90, 0);
                        break;
                }

                newPos = new Vector3(randomDoorPos.x + offsetX, randomDoorPos.y, randomDoorPos.z + offsetZ);
                GameObject lastRoom = Instantiate(chosenRoom.gameObject, newPos, rotation);
                return lastRoom;
            }
            
            Room newRoom = Instantiate(chosenRoom.gameObject, newPos, Quaternion.identity).GetComponent<Room>();


            newRoom.doors.RemoveAt(chosenDoor);
            
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
