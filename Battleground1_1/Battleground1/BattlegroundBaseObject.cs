using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Battleground
{
    public abstract class BattlegroundBaseObject:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isNew;
        private bool _isHighlighted = false;
        private bool _isMoving = false;
        private double _z = 0;
        private SolidColorBrush _objectColor;
         

        //is selected? 
        public bool IsHighlighted
        {
            get
            {
                return _isHighlighted;
            }
            set
            {
                _isHighlighted = value;
                OnPropertyChanged("IsHighlighted");
            }
        }

        //is New? 
        public bool IsNew
        {
            get { return _isNew; }
            set
            {
                _isNew = value;
                OnPropertyChanged("IsNew");
            }
        }

        public bool IsMoving
        {
            get { return _isMoving; }
            set
            {
                _isMoving = value;
                OnPropertyChanged("IsMoving");
            }
        }

        //z height?
        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }

        //color?
        public SolidColorBrush ObjectColor
        {
            get { return _objectColor; }
            set
            {
                _objectColor = value;
                OnPropertyChanged("ObjectColor");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
