﻿using System.Collections;
using System.Diagnostics;
using UnityEngine;
namespace Edgar.Unity
{
    /// <summary>
    /// Example of a simple game manager that uses the DungeonGeneratorRunner to generate levels.
    /// </summary>
    public class GenManager : GameManagerBase<GenManager>
    {

        public override void LoadNextLevel()
        {
            // Show loading screen
            ShowLoadingScreen("Next Level", "loading..");

            // Find the generator runner
            var generator = GameObject.Find("Generator").GetComponent<DungeonGeneratorGrid2D>();

            // Start the generator coroutine
            StartCoroutine(GeneratorCoroutine(generator));
        }

        /// <summary>
        /// Coroutine that generates the level.
        /// We need to yield return before the generator starts because we want to show the loading screen
        /// and it cannot happen in the same frame.
        /// It is also sometimes useful to yield return before we hide the loading screen to make sure that
        /// all the scripts that were possibly created during the process are properly initialized.
        /// </summary>
        private IEnumerator GeneratorCoroutine(DungeonGeneratorGrid2D generator)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            yield return null;

            generator.Generate();

            yield return null;

            stopwatch.Stop();

            SetLevelInfo($"Generated in {stopwatch.ElapsedMilliseconds / 1000d:F}s");
            HideLoadingScreen();
        }
    }
}