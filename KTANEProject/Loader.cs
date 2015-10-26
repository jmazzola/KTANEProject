using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KTANEProject
{
    public class Loader
    {
        public static GameObject gameObject;

        public static void Load()
        {
            // Create a new gameobject
            gameObject = new GameObject();
            // Add our script to it
            gameObject.AddComponent<Hax>();
            // Never delete this until we close the game
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }
    }
}
