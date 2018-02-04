using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceEngineersScripts
{
    class Program
    {
        static void Main(string[] args)
        {
            var script = new Parachute();
            script.Main(args.FirstOrDefault(),Sandbox.ModAPI.Ingame.UpdateType.None);
        }
    }
}
