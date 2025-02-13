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
        RoomInstanceGrid2D entranceRoomInstance = level.RoomInstances.FirstOrDefault(x => (x.Room.ToString()) == "Spawn");

        if (entranceRoomInstance == null)
        {
            throw new InvalidOperationException("Could not find Spawn room");
        }

        GameObject roomTemplateInstance = entranceRoomInstance.RoomTemplateInstance;

        // Find the spawn position marker
        Transform spawnPosition = roomTemplateInstance.transform.Find("SpawnPosition");

        // Move the player to the spawn position
        GameObject player = GameObject.Find("Player");
        player.transform.position = spawnPosition.position;

        Transform spriteChild = player.transform.Find("PlayerSprite"); // Assure-toi que le nom est correct

        // Appliquer la rotation au sprite
        if (spriteChild != null)
        {
            spriteChild.transform.rotation = spawnPosition.rotation; // Ajuste l'angle selon ton besoin
        }
    }
}