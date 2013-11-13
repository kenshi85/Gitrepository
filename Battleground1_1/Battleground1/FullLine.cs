using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace Battleground
{
    public class FullLine:BattlegroundBaseObject
    {

        //locals
        private double _x1, _y1, _x2, _y2;
        private double _opacity = 1;
        private double _thickness = 3;

        #region Coordinates
        public double X1
        {
            get { return _x1; }
            set
            {
                _x1 = value;
                OnPropertyChanged("X1");
            }
        }

        public double Y1
        {
            get { return _y1; }
            set
            {
                _y1 = value;
                OnPropertyChanged("Y1");
            }
        }

        public double X2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                OnPropertyChanged("X2");
            }
        }

        public double Y2
        {
            get { return _y2; }
            set
            {
                _y2 = value;
                OnPropertyChanged("Y2");
            }
        }
        #endregion

        #region color and stuff

        public Color LineColor { get; set; }
        public Color ShadowColor { get; set; }
        public Color SelectionColor { get; set; }

        public double Thickness
        {
            get { return _thickness; }
            set
            {
                _thickness = value;
                OnPropertyChanged("Thickness");
            }
        }

        //Transparency? //Base Object
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        #endregion
    }
}
