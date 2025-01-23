using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Platformars
{
	public class LevelGeneratorWindow : EditorWindow
	{
		#region consts
		const string menu = "Tools/Platformars/Level Generator  Window";
		const string labelMenu = "Level Generator Input";
		const string assetFolder = "Assets/Platformars";
		const string defaultSave = "Assets/Platformars/Generated/Levels";
		const string bannerIconAddress = assetFolder + "Textures/bannericon.png";
		#endregion

		#region fields
		private Vector2 _scrollPos;
		#endregion

		#region properties
		//public LevelGenerator levelGenerator = new LevelGenerator;
		public Texture bannerIcon;

		public static int levels;
		public static string levelIdentifier;
		public static string saveFolder;

		public float difficultyRate;
		public AnimationCurve difficultyCurve = new AnimationCurve();
		public TileBase[] environments;
		public GameObject avatar;
		public GameObject[] obstacles;
		public GameObject thresholdEasy;
		public GameObject thresholdHard;

		//public LevelGenerator levelGenerator = new LevelGenerator();
		
		#endregion

		#region methods
		[MenuItem(menu, false, 15)]
		public static void ShowWindow()
		{
			
			GetWindow<LevelGeneratorWindow>("Level Generator");
		}

		private void OnGUI()
		{
			//GUI.DrawTexture(new Rect(50,100,500,500), bannerIcon, ScaleMode.ScaleToFit, true, 100.0F);
			GUILayout.Box(bannerIcon, GUILayout.MaxHeight(bannerIcon.width), GUILayout.Height(Screen.width), GUILayout.Width(Screen.width));
			EditorGUILayout.HelpBox("This Early Access tool proposed for the final project and does not represent the final quality!", MessageType.Info);
			GUILayout.Label(labelMenu);

			EditorGUILayout.BeginVertical();
			//GUILayout.FlexibleSpace();
			// _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(this.position.width), GUILayout.Height(this.position.height - bannerIcon.height - 70));
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			levels = EditorGUILayout.IntField(new GUIContent("Levels", "The number of levels to be generated. Must more than 0."), levels);
			if(levels > 0)
			{
				levelIdentifier = EditorGUILayout.TextField(new GUIContent("Level Identifier", "Identifier for generated levels. For example 'Desert', then the levels will be named as 'Desert Level 1', 'Desert Level 2' and so on."), levelIdentifier);
				saveFolder = EditorGUILayout.TextField(new GUIContent("Save Folder", "Destination folder to save generated levels."), saveFolder);

				if (GUILayout.Button("Select Folder to Save Map"))
				{
					saveFolder = EditorUtility.OpenFolderPanel("Select Prefabs Folder", "", "Prefabs");
					saveFolder = saveFolder.Replace(Application.dataPath, "Assets");

					if(saveFolder == null)
					{
						Debug.Log("Invalid Save Folder! Save Folder Changed to " + defaultSave);
						saveFolder = defaultSave;
					}
					else
						Debug.Log("Save Folder Changed to " + saveFolder);
				}

				avatar = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Player Avatar", "Playable Character GameObject"), avatar, typeof(GameObject), true);

				//SerializedProperty tEasyProperty = go.FindProperty("thresholdEasy");
				//EditorGUILayout.PropertyField(tEasyProperty, false);

				thresholdEasy = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Threshold Easy", "Sample of the easiest level"), thresholdEasy, typeof(GameObject), true);
				//thresholdEasy = (GameObject)tEasyProperty;

				//SerializedProperty tHardProperty = go.FindProperty("thresholdHard");
				//EditorGUILayout.PropertyField(tHardProperty, false);

				thresholdHard = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Threshold Hard","Sample of the hardest level"), thresholdHard, typeof(GameObject), true);

				if (levels == 1)
				{
					
					difficultyRate = EditorGUILayout.Slider(new GUIContent("Difficulty Rate", "Difficulty rate between thresholds."), difficultyRate, 0.1f, 1.0f);
				}
				else
				{
					difficultyCurve = EditorGUILayout.CurveField(new GUIContent("Difficulty Curve","A Curve that defines difficulty rate on each level."), difficultyCurve);
				}

				//target refering to selected gameobject
				ScriptableObject target = this;
				SerializedObject go = new SerializedObject(target);

				GUILayout.Box("List of tile textures for level environments.");
				SerializedProperty environmentProperty = go.FindProperty("environments");
				EditorGUILayout.PropertyField(environmentProperty, true); // True means show children

				GUILayout.Box("List of obstacles to be placed on levels");
				SerializedProperty obstacleProperty = go.FindProperty("obstacles");
				EditorGUILayout.PropertyField(obstacleProperty, true); // True means show children



				go.ApplyModifiedProperties(); // Remember to apply modified properties

				if (GUILayout.Button("Generate Levels"))
				{
					GenerateLevels(levels);
				}
				
			}
			else
			{
				GUILayout.Box("The number of levels must be more than 0!");
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();


		}

		/// <summary>
		/// Call Generate Level from Level Genrator Class as many as the number of levels
		/// </summary>
		/// <param name="levels">Number of levels to be generated</param>
		public void GenerateLevels(int levels)
		{
			//levelGenerator = new LevelGenerator();
			//TODO: Implement PCG with genetic algorithm to generate levels
			Debug.Log(difficultyCurve.Evaluate(1 / (float)levels));
			Debug.Log(difficultyCurve.length);
			if(levels > 0)
			{
				if (levelIdentifier == null)
					Debug.LogWarning("Level Identifier is left out blank! Generated level will be named as 'Level [Level Number]'.");
				if(saveFolder != null)
				{
					if(levels == 1)
					{
						var levelDifficulty = difficultyRate;
						Debug.Log("Difficulty At" + difficultyRate);
						LevelGenerator.GenerateLevel(levelIdentifier, 1, obstacles, environments, thresholdEasy, thresholdHard, avatar, levelDifficulty, saveFolder);
					}
					else
					{
						for (int i = 1; i <= levels; i++)
						{
							EditorUtility.DisplayProgressBar("Simple Progress Bar", "Doing some work...", i / levels);
							var levelDifficulty = difficultyCurve.Evaluate((1 / (float)levels) * i);
							Debug.Log("Difficulty At" + difficultyCurve.Evaluate(1 / (float)levels) * i);
							LevelGenerator.GenerateLevel(levelIdentifier, i, obstacles, environments, thresholdEasy, thresholdHard, avatar, levelDifficulty, saveFolder);
						}
					}
					Debug.Log("Levels are saved in " + saveFolder);
				}
				else
				{
					Debug.LogWarning("Invalid or blank folder destination. The generated level will be saved on " + defaultSave);
					if(levels == 1)
					{
						var levelDifficulty = difficultyRate;
						Debug.Log("Difficulty At" + difficultyRate);
						LevelGenerator.GenerateLevel(levelIdentifier, 1, obstacles, environments, thresholdEasy, thresholdHard, avatar, levelDifficulty, defaultSave);
					}
					else
					{
						for (int i = 1; i <= levels; i++)
						{
							EditorUtility.DisplayProgressBar("Simple Progress Bar", "Doing some work...", i / levels);
							var levelDifficulty = difficultyCurve.Evaluate((1 / (float)levels) * i);
							Debug.Log("Difficulty At" + difficultyCurve.Evaluate(1 / (float)levels) * i);
							LevelGenerator.GenerateLevel(levelIdentifier, i, obstacles, environments, thresholdEasy, thresholdHard, avatar, levelDifficulty, defaultSave);
						}
					}
					Debug.Log("Levels are saved in " + defaultSave);
				}
				
			}
			else
			{
				Debug.LogError("Please enter a valid number of levels!");
			}
			EditorUtility.ClearProgressBar();

		}
		#endregion
	}
}

