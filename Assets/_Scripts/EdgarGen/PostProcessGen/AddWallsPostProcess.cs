using Edgar.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class AddWallsPostProcess : DungeonGeneratorPostProcessingComponentGrid2D
{
    public TileBase wallRuleTile;
    public int borderSize = 5;

    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        PutWalls(level);
    }

    private void PutWalls(DungeonGeneratorLevelGrid2D level)
    {
        // Find walls tilemap layer
        var tilemaps = level.GetSharedTilemaps();
        var walls = tilemaps.Single(x => x.name == "Walls");
        var background = tilemaps.Single(x => x.name == "Background");

        BoundsInt bounds = walls.cellBounds;
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();

        // Étape 1 : Récupérer toutes les positions des murs existants
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (walls.HasTile(pos))
                {
                    wallPositions.Add(pos);
                }
            }
        }

        // Étape 2 : Identifier les bords extérieurs des murs
        HashSet<Vector3Int> borderPositions = new HashSet<Vector3Int>();

        foreach (var pos in wallPositions)
        {
            Vector3Int[] directions = {
                new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0), // Droite, Gauche
                new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0)  // Haut, Bas
            };

            foreach (var dir in directions)
            {
                Vector3Int neighbor = pos + dir;
                if (!wallPositions.Contains(neighbor)) // C'est un bord extérieur
                {
                    borderPositions.Add(pos);
                    break;
                }
            }
        }

        // Étape 3 : Étendre les murs vers l'extérieur de 5 cases
        HashSet<Vector3Int> expansionPositions = new HashSet<Vector3Int>();

        foreach (var borderPos in borderPositions)
        {
            for (int dx = -borderSize; dx <= borderSize; dx++)
            {
                for (int dy = -borderSize; dy <= borderSize; dy++)
                {
                    Vector3Int newPos = new Vector3Int(borderPos.x + dx, borderPos.y + dy, 0);

                    // Vérifier si la case est vide et n'est pas une partie du background
                    if (!wallPositions.Contains(newPos) && !background.HasTile(newPos))
                    {
                        expansionPositions.Add(newPos);
                    }
                }
            }
        }

        // Étape 4 : Ajouter les nouvelles tuiles de murs
        foreach (var pos in expansionPositions)
        {
            walls.SetTile(pos, wallRuleTile);
        }
    }
}