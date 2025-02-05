using Edgar.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnPositionPostProcess : DungeonGeneratorPostProcessingComponentGrid2D
{

    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        SetSpawnPosition(level);
    }

    private void SetSpawnPosition(DungeonGeneratorLevelGrid2D level)
    {
        var entranceRoomInstance = level.RoomInstances.FirstOrDefault(x => (x.Room.ToString()) == "Spawn");

        if (entranceRoomInstance == null)
        {
            throw new InvalidOperationException("Could not find Spawn room");
        }

        var roomTemplateInstance = entranceRoomInstance.RoomTemplateInstance;

        // Find the spawn position marker
        var spawnPosition = roomTemplateInstance.transform.Find("SpawnPosition");

        // Move the player to the spawn position
        var player = GameObject.Find("Player");
        player.transform.position = spawnPosition.position;
        /////////////////////// HERE
        ///need to rotate player when spawn
        ///a
        ///a
        ///a
        ///a
        ///a
        ///
    }
}