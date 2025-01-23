using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformars
{
    /// <summary>
    /// You can add another enemy type here
    /// </summary>
    [Serializable]
    public enum EnemyType
    {
        EnemyGround,
        EnemyFlying
        
    }
     
    public class Enemy : MonoBehaviour
    {
        public EnemyType enemyType;
    }

}
