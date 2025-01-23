using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformars
{
    public class MapGenerator 
    {
        #region properties

        //General information
        public string mapName;
        public string saveFolder;

        //Map Properties
        public int height, width;
        public int[,] map;

        //Seed props when generating map
        public bool useRandomSeed;
        public string seed;

        [Range(0, 100)]
        public int randomFillPercent;

        //Base environment for floor
        public GameObject environment;
        public TileBase tileBase;
        public GridPalette palette;
        #endregion

        #region methods
        //Fill map with environment based

        public void GenerateMap()
        {
            map = new int[width, height];
            RandomFillMap();

            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
            }
            //GameObject generatedMap = DrawTile(map, tileBase);
            //PrefabUtility.SaveAsPrefabAsset(generatedMap, saveFolder + "/" + generatedMap.name + ".prefab");
        }

        void RandomFillMap()
        {
            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }

            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }
                }
            }
        }

        //Smoothen generated map
        void SmoothMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        map[x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        map[x, y] = 0;

                }
            }
        }

        //Get surrounding wall
        int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        /// <summary>
        /// Draw map object using Tilemaps component
        /// </summary>
        /// <param name="map">Map dimension</param>
        /// <param name="tileBase">Tile Base Object for environment</param>
        /// <param name="tilemap">Tilemap Object for rendering the tile</param>
        /// <returns></returns>
        public void DrawTile(int[,] map, TileBase tileBase, Tilemap tilemap)
        {
            Debug.Log("latest matrix length = " + map.GetLength(0) + ", height = " + map.GetLength(1));
            if (map != null)
            {

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x, y] == 1)
                        {
                            tilemap.SetTile(new Vector3Int(y, x, 0), tileBase);
                            
                        }
                        //Debug.Log(map[x, y] + " ");
                    }
                }
            }


        }
        #endregion
    }
}