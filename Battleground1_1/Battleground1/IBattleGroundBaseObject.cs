using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Battleground
{
    interface IBattleGroundBaseObject : INotifyPropertyChanged
    {
        bool IsNew { get; set; }
        bool IsHighlighted { get; set; }
        bool IsMoving { get; set; }
        double Z { get; set; }
    }
}
