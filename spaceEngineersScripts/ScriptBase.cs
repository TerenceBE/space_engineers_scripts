using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI.Ingame;

namespace spaceEngineersScripts
{
    public abstract class ScriptBase : IMyGridProgram
    {
        public Sandbox.ModAPI.Ingame.IMyGridTerminalSystem GridTerminalSystem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Sandbox.ModAPI.Ingame.IMyProgrammableBlock Me { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan ElapsedTime {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public string Storage {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public IMyGridProgramRuntimeInfo Runtime {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Action<string> Echo
        {
            get => throw new NotImplementedException();
            set {
                Console.WriteLine(value);
            }
        }


        public bool HasMainMethod => throw new NotImplementedException();

        public bool HasSaveMethod => throw new NotImplementedException();

        public void Main(string argument)
        {
            this.Main(argument, UpdateType.None);
        }

        public abstract void Main(string argument, UpdateType updateSource);

        public virtual void Save()
        {
            throw new NotImplementedException();
        }
    }
}
