using System.Collections.Generic;
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
        
        private void Awake()
        {
            startPos = transform.position;
        }

        private void Start()
        {
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
                GameObject room = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
                return Instantiate(room, startPos, Quaternion.identity);
            }

            Room currentRoom = currentGameObject.GetComponent<Room>();
            Door randomDoor = currentRoom.doors[Random.Range(0, currentRoom.doors.Count)];
            
            foreach (var room in roomPrefabs)
            {
                Room roomComponent = room.GetComponent<Room>();
                foreach (var door in roomComponent.doors)
                {
                    if ((door.doorPosition == DoorPosition.North && randomDoor.doorPosition == DoorPosition.South) ||
                        (door.doorPosition == DoorPosition.South && randomDoor.doorPosition == DoorPosition.North) ||
                        (door.doorPosition == DoorPosition.East && randomDoor.doorPosition == DoorPosition.West) || 
                        (door.doorPosition == DoorPosition.West && randomDoor.doorPosition == DoorPosition.East))
                    {
                        roomComponent.doors.Remove(randomDoor);
                        return Instantiate(room, randomDoor.transform.position, Quaternion.identity);
                    }
                }
            }
            
            return gameObject;
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
