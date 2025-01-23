using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformars
{
    /// <summary>
    /// You can add another obstacle type here
    /// </summary>
    [Serializable]
    public enum ObstacleType
    {
        Destroyable,
        Hazard
    }

    public class Obstacle : MonoBehaviour
    {
        public ObstacleType obstacleType;
    }
}

