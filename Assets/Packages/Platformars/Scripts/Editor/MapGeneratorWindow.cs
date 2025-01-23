using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Platformars.Runtime;

namespace Platformars
{
    public class MapGeneratorWindow : EditorWindow
    {
        #region consts
        const string menu = "Tools/Platformars/Map Generator Window";
        const string labelMenu = "Map Generator Alpha by Imanda & Annazar";
        #endregion

        #region properties
        //General information
        public static string mapName;
        public string saveFolder;

        //Define height and width
        public static int height, width;
        public static int[,] map;

        //Seed props when generating map
        public bool useRandomSeed;
        public string seed;

        //Base environment for floor
        public GameObject environment;
        public TileBase tileBase;
        public GridPalette palette;


        [Range(0, 100)]
        public int randomFillPercent;
        #endregion

        #region methods
        [MenuItem(menu, false, 15)]
        public static void ShowWindow()
        {
            GetWindow<MapGeneratorWindow>("Map Generator");
        }

        //Tab layout
        private void OnGUI()
        {
            //GUILayout.Label(labelMenu);

            GUILayout.BeginVertical();
            GUILayout.Label("General Information");

            mapName = EditorGUILayout.TextField("Map Name", mapName);
            saveFolder = EditorGUILayout.TextField("Save Folder", saveFolder);

            if (GUILayout.Button("Select Folder to Save Map"))
            {
                saveFolder = EditorUtility.OpenFolderPanel("Select Prefabs Folder", "", "Prefabs");
                saveFolder = saveFolder.Replace(Application.dataPath, "Assets");
                Debug.Log("Save Folder Changed to " + saveFolder);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Map Builder");

            width = EditorGUILayout.IntField("Width", width);
            height = EditorGUILayout.IntField("Height", height);
            useRandomSeed = EditorGUILayout.Toggle("Use Random Seed", useRandomSeed);
            randomFillPercent = EditorGUILayout.IntSlider("Random Fill", randomFillPercent, 0, 100);
            //environment = (GameObject)EditorGUILayout.ObjectField("Environment" ,environment, typeof(GameObject), true);
            tileBase = (TileBase)EditorGUILayout.ObjectField("TileBase", tileBase, typeof(TileBase), true);


            //Add button to GenerateMap
            if (GUILayout.Button("Generate Map"))
            {
                GenerateMap();
                Debug.Log("Generating map with " + randomFillPercent + "% filling.");
            }
            GUILayout.EndVertical();
        }

        //Generate map as GameObject
        void GenerateMap()
        {
            map = new int[width, height];
            RandomFillMap();

            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
            }
            DrawTile();
        }

        //Fill map with environment based
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

        //Instantiate environment GameObjects as Map's child
        void DrawMap()
        {
            GameObject prnt = Instantiate(new GameObject("Map"), Vector3.zero, Quaternion.identity) as GameObject;
            
            prnt.name = mapName + "Map";
            
            if (map != null)
            {
                
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if(map[x,y] == 1)
                        {
                            GameObject obj = Instantiate(environment, new Vector3(x,y,0), Quaternion.identity) as GameObject;
                            
                            obj.transform.SetParent(prnt.transform);

                        }
                    }
                }
            }
            Selection.activeGameObject = prnt;
            PrefabUtility.SaveAsPrefabAsset(prnt, saveFolder + "/" + prnt.name + ".prefab");
        }

        /// <summary>
		/// Draw Tile based on Generated Matrix
		/// </summary>
        void DrawTile()
        {
            GameObject grid = Instantiate(new GameObject("Map"), Vector3.zero, Quaternion.identity) as GameObject;
            grid.name = mapName + "Map";
            grid.AddComponent<Grid>();
            GameObject tilemap = Instantiate(new GameObject("Tilemap"), Vector3.zero, Quaternion.identity) as GameObject;
            tilemap.AddComponent<Tilemap>();
            tilemap.AddComponent<TilemapRenderer>();
            tilemap.transform.SetParent(grid.transform);

            if (map != null)
            {

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (map[x, y] == 1)
                        {
                            tilemap.GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), tileBase);

                        }
                    }
                }
            }
            if(tilemap.GetComponent<TilemapCollider2D>() == null)
                tilemap.AddComponent<TilemapCollider2D>();
            Selection.activeGameObject = grid;
            PrefabUtility.SaveAsPrefabAsset(grid, saveFolder + "/" + grid.name + ".prefab");
        }

       

        #endregion

    }
}

