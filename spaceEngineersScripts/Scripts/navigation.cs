
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using VRage.Game;
using VRage.Library;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRageMath;

namespace spaceEngineersScripts
{
    class Navigation : MyGridProgram
    {



        //--------------------------------------------------------------------//    
        //         Navigation Calculation Tools (12.5.17)      //    
        //-------------------------------------------------------------------//     

        //Ensure the display you want to use contains "RESULT"

        //Main Script      



        private string rawInput = "::::";

        public void Main(string args, UpdateType updateSource)
        {
            if ((updateSource & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) == 0)
            {
                this.rawInput = args;
            }
            Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update10;




            double speed = SpeedCalc();

            //Current Ship Position
            var myXYZ = Me.GetPosition();
            double x = myXYZ.X;
            double y = myXYZ.Y;
            double z = myXYZ.Z;

            TargetInfo nameXYZ = FindCoords(this.rawInput);

            // Target Name
 

            string TargetName = nameXYZ.Destination;




            // Target Position

            double X = nameXYZ.X;

            double Y = nameXYZ.Y;
            
            double Z = nameXYZ.Z;

            //check for valid GPS


            double Distance = Math.Sqrt(Math.Pow((x - X), 2) + Math.Pow((y - Y),2) + Math.Pow((z - Z),2)); // returns distance in meters
           
            double ETA = ETACalc(Distance, speed); // measured in seconds

            string result = TimeSpanConvert(ETA);

            DisplayResult(nameXYZ.Destination, result, TargetName);
                       

            

        }

        private string TimeSpanConvert(double ETA)
        {
            if (ETA > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(ETA);
                string days = time.ToString(@"g");


                return (                string.Format(
                    "{0}{1}{2}{3} Seconds",
                    // Only show Days if > 0
                    time.Days > 0
                        ? string.Format("{0} Days | ", time.Days)
                        : null,
                    // Only show Hours if > 0
                    // ternary operator
                    // condition ? value if true : value if false
                    time.Hours > 0
                        ? string.Format("{0} Hours | ", time.Hours)
                        : null, 
                    time.Minutes > 0
                        ? string.Format("{0} Minutes | ", time.Minutes)
                        : null,
                    time.Seconds
                )
            );      
                    
                    


            }
            else
            {
                return "No ETA Possible: Ship Stationary.";
            }
        }


        private double ETACalc(double Distance,double speed)
        {
            if (speed > 0.0099999) {
                double seconds = Distance / speed;
                return seconds;
            }
            else
            {
                return 0;
            }

        }


        private class TargetInfo
        {
            public string Destination { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        TargetInfo FindCoords(string argument)
        {


            TargetInfo info = new TargetInfo();
            string[] splitString = (argument).Split(':');

            double result = 0;

            if (splitString.Length > 4)
            {

                if (!double.TryParse(splitString[2], out result))
                {
                    info.Destination = "null";
                    return info;
                }

            }


            if (splitString.Length > 4)
            {
                if (!double.TryParse(splitString[3], out result))
                {
                    info.Destination = "null";
                    return info;
                }
            }

            if (splitString.Length > 4)
            {

                if (!double.TryParse(splitString[3], out result))
                {
                    info.Destination = "null";
                    return info;
                }
            }

            if (splitString.Length > 4)
            {

                if (double.TryParse(splitString[3], out result))
                {


                    info.Destination = Convert.ToString(splitString[1]);
                    info.X = Convert.ToDouble(splitString[2]);
                    info.Y = Convert.ToDouble(splitString[3]);
                    info.Z = Convert.ToDouble(splitString[4]);
                }
                return info;
            }
            else
            {
                info.Destination = "null";
                return info;
                     
            }
        }












    private void DisplayResult(string check, string result, string destination)
        {
            if (check.Equals("null"))

            {
                string output = "Please enter a valid GPS coordinate." + "\n  GPS:EXAMPLE:X:Y:Z:";
                List<IMyTextPanel> panels = new List<IMyTextPanel>();

                GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(panels, x => x.CustomName.Contains("RESULT"));

                foreach (var display in panels)
                {

                    display.ShowPublicTextOnScreen();
                    display.WritePublicText(output);

                }
                Echo(output);
            }
            else
            {

                string output = " You will arrive to " + destination + " in approx:\n" + result;


                List<IMyTextPanel> pan = new List<IMyTextPanel>();

                GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(pan, x => x.CustomName.Contains("RESULT"));

                foreach (var display in pan)
                {

                    display.ShowPublicTextOnScreen();
                    display.WritePublicText(output);
                }
                Echo(output);
            }
            
        }

    private double SpeedCalc()
    {

            List<IMyShipController> shipControllers = new List<IMyShipController>();
            GridTerminalSystem.GetBlocksOfType<IMyShipController>(shipControllers);
            foreach (var item in shipControllers)
            {
                return item.GetShipSpeed();

            }
            return 0.00;

     }



        //f(double.TryParse("this is a string", out result)) { do something with it }
        //GPS:Alligatro #2:51722.23:-186167.45:942.28:
    }
}