using System;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable
{
    public class MiniHackDetailsViewModel : ViewModelBase
    {
        public MiniHack Hack { get; set;}
        public MiniHackDetailsViewModel(MiniHack hack)
        {
            Hack = hack;
        }

        public void FinishHack ()
        {
            Hack.IsCompleted = true;
            Settings.Current.FinishHack (Hack.Id);
        }
    }
}

