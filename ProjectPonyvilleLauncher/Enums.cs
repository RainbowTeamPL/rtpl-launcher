using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPonyvilleLauncher.Enums
{
    public class Enums
    {
        public enum Game
        {
            Unknown = -1,
            ProjectPonyville = 0
        }

        public enum GameState
        {
            Unknown = -1,
            NotInstalled = 1,
            NotUpdated = 2,
            ReadyToPlay = 3
        }

        public enum UpdateState
        {
            Unknown = -1,
            Idle = 1,
            Downloading = 2,
            Unzipping = 3,
            Patching = 4,
            ReadyToPlay = 5,
            InstallingPrerequisites = 6
        }
    }
}