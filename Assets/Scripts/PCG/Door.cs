using System;
using UnityEngine;

namespace PCG
{
    [Serializable]
    public class Door
    {   
        public Transform transform;
        public DoorPosition doorPosition;
    }
    
    public enum DoorPosition
    {
        North,
        South,
        East,
        West
    }
}