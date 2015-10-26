using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;

namespace KTANEProject
{
    public class Menu : MonoBehaviour
    {
        
        public string customSeed = "Custom seed";

        void Start()
        {
             
        }

        void Update()
        {

            ProcessInput();
        }

        void ProcessInput()
        {

        }

        void OnGUI()
        {
            GUI.color = Color.magenta;

                             //x  y
            GUI.Label(new Rect(0, 0, 300, 40), "KTANEProject v1.0 - By Justin 'Chocolate' Mazzola");

            // Set Seed: [textbox]
            GUI.Label(new Rect(0, 50, 100, 40), "Set Seed: ");
            customSeed = GUI.TextField(new Rect(110, 50, 100, 20), customSeed);

        }
    }
}