using Edgar.Unity;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnPostProcess : DungeonGeneratorPostProcessingComponentGrid2D
{
    [Range(0, 1)]
    public float EnemySpawnChance = 0.5f;

    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        HandleEnemies(level);
    }

    private void HandleEnemies(DungeonGeneratorLevelGrid2D level)
    {
        // Iterate through all the rooms
        foreach (var roomInstance in level.RoomInstances)
        {
            // Get the transform that holds all the enemies
            var enemiesHolder = roomInstance.RoomTemplateInstance.transform.Find("Enemies");

            // Skip this room if there are no enemies
            if (enemiesHolder == null || enemiesHolder.childCount == 0)
            {
                continue;
            }

            int totalEnemies = enemiesHolder.childCount;
            int minEnemiesToSpawn = Mathf.CeilToInt(totalEnemies / 2f); // Au moins la moitié
            int spawnedEnemies = 0;
            List<GameObject> potentialSpawns = new();

            // Étape 1 : Application du spawn aléatoire
            foreach (Transform enemyTransform in enemiesHolder)
            {
                var enemy = enemyTransform.gameObject;

                if (Random.NextDouble() < EnemySpawnChance)
                {
                    enemy.SetActive(true);
                    spawnedEnemies++;
                }
                else
                {
                    enemy.SetActive(false);
                    potentialSpawns.Add(enemy); // On garde en mémoire les désactivés
                }
            }

            // Étape 2 : Forcer l'activation si besoin
            while (spawnedEnemies < minEnemiesToSpawn && potentialSpawns.Count > 0)
            {
                int index = Random.Next(potentialSpawns.Count); // Sélection aléatoire
                potentialSpawns[index].SetActive(true);
                potentialSpawns.RemoveAt(index); // On évite de réactiver le même
                spawnedEnemies++;
            }
        }
    }
}