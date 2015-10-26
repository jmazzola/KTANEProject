using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;
using Assets.Scripts.Components.VennWire;
using TMPro;

namespace KTANEProject
{
    public class Hax : MonoBehaviour
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

            foreach (PortWidget port in gameplayState.Bomb.WidgetManager.GetBehaviors<PortWidget>())
            {
                if (port.IsPortPresent(PortWidget.PortType.Parallel))
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

        // Print out all the modules and their information to help identify which one is which
        string GetModules()
        {

            string modules = string.Empty;
            for (int i = 0; i < gameplayState.Bomb.BombComponents.Count; i++)
            {
                BombComponent currentComp = gameplayState.Bomb.BombComponents[i];
                string type = currentComp.ComponentType.ToString();

                if (type == "Timer" || type == "Empty")
                    continue;

                modules += i + " - " + type + " - ";

                switch (type)
                {
                    case "Wires":
                        {
                            // Wires - wirecount Wires - [ list them ]
                            WireSetComponent wires = (WireSetComponent)currentComp;
                            int wireCount = wires.WireCount;
                            modules += wireCount.ToString() + " Wires: ";

                            for (int wire = 0; wire < wireCount; wire++)
                            {
                                modules += wires.GetColorOfWireIndex(wire).ToString() + ",";

                                if (wire == wireCount - 1)
                                    modules += wires.GetColorOfWireIndex(wire).ToString();
                            }

                            break;
                        }

                    case "BigButton":
                        {
                            // BigButton - color says instruction
                            ButtonComponent button = (ButtonComponent)currentComp;
                            modules += button.ButtonColor.ToString() + " says " + button.ButtonInstruction.ToString();
                            break;
                        }

                    case "Venn":
                        {
                            // Venn - wirecount Wires
                            VennWireComponent venn = (VennWireComponent)currentComp;
                            modules += venn.Wires.Length.ToString() + " Wires";
                            break;
                        }

                    case "Keypad":
                        {
                            // Keypad - [keys]
                            KeypadComponent keypad = (KeypadComponent)currentComp;
                            for (int key = 0; key < keypad.buttons.Length; key++)
                            {
                                modules += keypad.buttons[key].GetText() + ",";

                                if (key == keypad.buttons.Length)
                                    modules += keypad.buttons[key].GetText();
                            }
                            break;
                        }

                    case "Simon":
                        {
                            // Simon - [color flashing]
                            // 0 - yellow | 1 - blue | 2 - green | 3 - red
                            // There literally is no way to tell what button is flashing
                            // going to have to include a boolean to see what gets called in the modified .dll

                            //SimonComponent simon = (SimonComponent)currentComp;

                            //for (int key = 0; key < simon.buttons.Length; key++)
                            //{
                            //    // something here.
                            //        modules += key + " flashing.";
                            //}

                            break;
                        }

                    case "WhosOnFirst":
                        {
                            // WhosOnFirst - [display]
                            WhosOnFirstComponent whos = (WhosOnFirstComponent)currentComp;
                            modules += whos.DisplayText.text;
                            break;
                        }

                    case "Memory":
                        {
                            // Memory - [display]
                            MemoryComponent mem = (MemoryComponent)currentComp;
                            modules += mem.DisplayText.text;
                            break;
                        }

                    case "WireSequence":
                        {
                            // WireSequence - [1 ABC, 2ABC, 3ABC]
                            // No way of grabbing wire pages since it's private
                            // Need to make it public in modified .dll
                            break;
                        }

                    case "Maze":
                        {
                            // Maze - currentpos | goalpos
                            // No way of grabbing maze cells since it's private
                            // Need to make it public in modified .dll
                            break;
                        }

                    case "Password":
                        {
                            // Password - [letters]
                            PasswordComponent pw = (PasswordComponent)currentComp;

                            for (int spin = 0; spin < pw.Spinners.Count(); spin++)
                            {
                                modules += pw.Spinners[spin].Display.text + " ";
                            }

                            break;
                        }

                }


                modules += "\n";
            }

            return modules;
        }

        // Print out the time remaining as well strike count
        string GetTimerStrikeInfo()
        {
            if (gameplayState.Bomb.HasDetonated)
                return "Bomb has exploded.";
            else if (gameplayState.Bomb.IsSolved())
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
            if (Input.GetKeyDown(KeyCode.Insert))
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