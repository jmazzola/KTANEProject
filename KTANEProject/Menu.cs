using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

namespace KTANEProject
{
    public class Menu : MonoBehaviour
    {
        GameplayState gameplayState;

        // Is the menu open?
        bool bMenuOpen = false;

        // Does our bomb include these modules?
        bool bIncludeWires, bIncludeWhosOnFirst, bIncludeSimon, bIncludeVennWire, bIncludeMemory, bIncludeButton, bIncludeMaze, bIncludeMorse, bIncludeWireSeq, bIncludePW;

        // Does our bomb include these needy modules?
        bool bIncludeKnob, bIncludeVent, bIncludeCapacitor;

        // Do we let the game generate our own bomb?
        bool bCustomBomb;

        // If we allow custom bombs, what is our seed?
        int nCustomSeed;

        // Print out the serial information - number, odd/even in number, has/hasnt a vowel in number
        string GetSerialInfo()
        {
            SerialNumber serialNumber = gameplayState.Bomb.WidgetManager.GetBehavior<SerialNumber>();

            string info = "Serial #" + serialNumber.GetSerialString();

            info += " | ";

            if (serialNumber.IsLastDigitEven())
                info += "Last digit is even";
            else
                info += "Last digit is odd";

            info += " | ";

            if (serialNumber.ContainsVowel())
                info += "Has a vowel.";
            else
                info += "Doesn't have a vowel.";

            return info;
        }

        // Print the number of batteries
        string GetNumOfBatteries()
        {
            int numberOfBatteries = 0;
            foreach (BatteryWidget battery in gameplayState.Bomb.WidgetManager.GetBehaviors<BatteryWidget>())
            {
                numberOfBatteries += battery.GetNumberOfBatteries();
            }

            if (numberOfBatteries > 1)
                return numberOfBatteries.ToString() + " Batteries.";
            else
                return numberOfBatteries.ToString() + " Battery";
        }

        // Print out what indicators are lit
        string GetLitIndicators()
        {
            string lit = string.Empty;
            foreach (IndicatorWidget ind in gameplayState.Bomb.WidgetManager.GetBehaviors<IndicatorWidget>())
            {
                if (ind.On)
                    lit += ind.Label + " ";
            }

            if (lit == string.Empty)
                return "No lit indicators.";

            return lit;
        }

        // Print out the ports
        string GetPresentPorts()
        {
            string ports = string.Empty;

            foreach(PortWidget port in gameplayState.Bomb.WidgetManager.GetBehaviors<PortWidget>())
            {
                if(port.IsPortPresent(PortWidget.PortType.Parallel))
                    ports += "Parallel ";

                if (port.IsPortPresent(PortWidget.PortType.DVI))
                    ports += "DVI ";

                if (port.IsPortPresent(PortWidget.PortType.PS2))
                    ports += "PS2 ";

                if (port.IsPortPresent(PortWidget.PortType.RJ45))
                    ports += "RJ45 ";

                if (port.IsPortPresent(PortWidget.PortType.Serial))
                    ports += "Serial ";

                if (port.IsPortPresent(PortWidget.PortType.StereoRCA))
                    ports += "RCA ";
            }

            if (ports == string.Empty)
                return "No ports.";

            return ports;
        }

        // Print out all the modules
        string GetModules()
        {
            
            string modules = string.Empty;
            for (int i = 0; i < gameplayState.Bomb.BombComponents.Count; i++)
            {
                string type = gameplayState.Bomb.BombComponents[i].ComponentType.ToString();

                if (type == "Timer" || type == "Empty")
                    continue;

                modules += i + " - " + type + "\n";
            }

            return modules;
        }

        // Print out the time remaining as well strike count
        string GetTimerStrikeInfo()
        {
            if (gameplayState.Bomb.HasDetonated)
                return "Bomb has exploded.";
            else if(gameplayState.Bomb.IsSolved())
                return "Bomb has been defused.";

            string info = string.Empty;

            info += "Time Remaining: " + TimerComponent.GetFormattedTime(gameplayState.Bomb.GetTimer().TimeRemaining, true) + " | ";

            int strikesLeft = gameplayState.Bomb.NumStrikesToLose;

            info += gameplayState.Bomb.NumStrikes.ToString() + " / ";

            if (strikesLeft == 1)
                info += strikesLeft.ToString() + " Strike";
            else
                info += strikesLeft.ToString() + " Strikes";

            return info;
        }

        void Start()
        {
            gameplayState = null;
        }

        void Update()
        {

            gameplayState = SceneManager.Instance.GameplayState;

            ProcessInput();

            if (gameplayState == null)
                return;
        }

        void ProcessInput()
        {
            // Put in menu support and stuff
            if(Input.GetKeyDown(KeyCode.Insert))
            {
                bMenuOpen = !bMenuOpen;
            }
        }

        void OnGUI()
        {
            // Set the GUI Style font and font size and color
            GUIStyle style = GUI.skin.GetStyle("label");
            style.fontSize = 16;
            style.font = (Font)Resources.Load("DINPro-Bold");
            GUI.color = Color.red;

            GUILayout.BeginArea(new Rect(0, 0, 1000, 1000));
            {
                // Trademark
                GUILayout.Label("KTANEProject v1.0 by Justin Mazzola");
                GUILayout.Label("All rights reserved to Steel Crate Games and the game Keep Talking and Nobody Explodes.");

                // Paranoid check for a null GameplayState
                if (!gameplayState)
                {
                    GUILayout.EndArea();
                    return;
                }

                if (bMenuOpen)
                {
                    // Spew out all information about bomb
                    GUILayout.BeginVertical();
                    {
                        GUI.color = Color.white;
                        GUILayout.Label(GetTimerStrikeInfo());
                        GUI.color = Color.magenta;
                        GUILayout.Label(GetSerialInfo());
                        GUI.color = Color.cyan;
                        GUILayout.Label(GetNumOfBatteries());
                        GUI.color = Color.yellow;
                        GUILayout.Label(GetLitIndicators());
                        GUI.color = new Color(1.0f, 0.7f, 0.9f);
                        GUILayout.Label(GetPresentPorts());
                        GUI.color = Color.green;
                        GUILayout.Label(GetModules());
                    }
                    GUILayout.EndVertical();
                }
                else
                {
                    GUI.color = Color.white;
                    GUILayout.Label("Press INSERT to toggle [Bomb Information]");
                }

            }
            GUILayout.EndArea();
        }
    }
}