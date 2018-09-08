using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
//using VRageMath;
using VRage.Game;
using VRage.Library;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;


namespace spaceEngineersScripts
{
    class Parachute : MyGridProgram
    {



        //--------------------------------------------------------------------//    
        //         Parachute Calculator Script v1(12.3.17)         //    
        //-------------------------------------------------------------------//     


        //This script will display the terminal velocity of the craft the programable block is attached to in m/s
        //This is how fast your craft will be going before it hits the ground.

        //Instructions//    

        //Name the display you want to show the terminal speed on as "Velocity Result" without the ""    
        //Use the following arguments: "1" or "Earth" for Earth (default), "2" or "Mars" for Mars, "3" or "Alien" 
        //for Alien, "4" or "Moons" for Europa or Titan
        //Note: if a value other than 1, 2, 3, 4, Earth, Mars, Alien, or Moons is input, the program will reset to "1"   

        //-------------------------------------------------------------------------------------------------------- 
        // !!!WARNING!!! The Earth-like moon has no atmosphere and you will not stop with a parachute 
        //--------------------------------------------------------------------------------------------------------- 


        //The craft MUST HAVE A CONTROL BLOCK such as a cockpit, seat, or remote control at this time due to API restrictions.
        //Note: This block does not have to be completed and can be removed after calculations are complete, however it must
        // be owned by you.

        //---------------------------------------------------------------------------------------------------------//    
        //!!!WARNING!!! If a control block is not present a value of "0" will display.          //    
        // This DOES NOT mean you will hit the ground at 0 m/s.                                      //    
        //---------------------------------------------------------------------------------------------------------//    

        //Note: If a parachute is not present, the speed will display as "infinite"  

        //Configurable constant
        private const float ATM = 0.85f; // Atmosphere

        //(avoid using 1.0 (high accuracy mode). 0.85 gives a more conservative result (you will be 
        // travelling slower than stated). 
        // 1.0 and higher can lead to dangerous results and is assuming "ideal" conditions

        //Constants       
        private const int RADMULT = 8; //- radius multiplier, always       
        private const float REEFLEVEL = 0.6f; // reefing level, always       
        private const int CD = 1; //- drag coefficient, always       


        //Main Script      

        public void Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update10;
        }

        private string rawInput = "";

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) == 0)
            {
                this.rawInput = argument;
            }

            var gravity = SelectGravity(this.rawInput);

            var gridSize = Me.CubeGrid.GridSize;

            var qty = CountParachutes();

            var mass = CalcMass();

            var parachuteDiameter = ParachuteDiameterCalc(gridSize);

            var area = AreaCalc(parachuteDiameter);

            var result = TerminalVelocitycalc(mass, gravity, area, qty);

            DisplayResult(result);
        }

        private void DisplayResult(double result)
        {
            var output = $"Your terminal velocity with\n parachutes deployed will be approx:\n{result.ToString()} m/s";
            Echo(output);

            var panels = new List<IMyTextPanel>();

            GridTerminalSystem.GetBlocksOfType(panels, x => x.CustomName.Contains("Velocity Result"));

            foreach (var display in panels)
            {
                display.ShowPublicTextOnScreen();
                display.WritePublicText(output);
            }
        }

        private double SelectGravity(string selection)
        {
            switch (selection.ToUpper())
            {
                case "4": //moons    
                case "MOONS":
                    return 2.45;
                case "3": // alien    
                case "ALIEN":
                    return 10.8;
                case "2": //mars    
                case "MARS":
                    return 8.83;
                case "1": //earth 
                case "EARTH":
                default:
                    return 9.81;
            }
        }

        private int CountParachutes()
        {
            var parachutes = new List<IMyParachute>();
            GridTerminalSystem.GetBlocksOfType(parachutes);
            return parachutes.Count;
        }

        /// <remarks>Returns 0.0 instead of NaN when no <seealso cref="IMyShipController"/> is available</remarks>
        private double CalcMass()
        {
            var mass = double.NaN;
            var shipControllers = new List<IMyShipController>();
            GridTerminalSystem.GetBlocksOfType(shipControllers);
            if (shipControllers.Any())
            {
                mass = shipControllers.First().CalculateShipMass().TotalMass;
            }
            else
            {
                mass = 0.0;
            }

            return mass;
        }

        private double ParachuteDiameterCalc(double gridSize)
        {
            return (Math.Log((10 * (ATM - REEFLEVEL)) - 0.99) + 5) * RADMULT * gridSize;
        }

        private double AreaCalc(double parachuteDiameter)
        {
            return (Math.PI * (Math.Pow(parachuteDiameter / 2.0, 2.0)));
        }

        private double TerminalVelocitycalc(double mass, double gravity, double area, int qty)
        {
            return Math.Round(Math.Sqrt((mass * gravity) / (area * CD * qty * ATM * 1.225 * 2.5)), 2);
        }
    }
}
