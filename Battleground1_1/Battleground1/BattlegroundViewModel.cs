using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Battleground
{
    class BattlegroundViewModel:INotifyPropertyChanged
    {
        #region Collection of Lines and Objects

        //Lines
        private bool _line1 = false;
        private double _currentMousePositionX, _currentMousePositionY;
        private PathGeometry _tilePathData = new PathGeometry();
        private Color _selectedColor = Colors.Black;
        //private double _currentScrollViewerMousePositionX, _currentScrollViewerMousePositionY;

        ObservableCollection<BattlegroundBaseObject> _battlegroundObjects;
        public ObservableCollection<BattlegroundBaseObject> BattlegroundObjects
        {
            get { return _battlegroundObjects ?? (_battlegroundObjects = new ObservableCollection<BattlegroundBaseObject>()); }
        } 

        #endregion

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged("SelectedColor");
            }
        }

        public PathGeometry TilePathData
        {
            get { return _tilePathData; }
            set
            {
                _tilePathData = value;
                OnPropertyChanged("TilePathData");
            }
        }

        public double CurrentMousePositionX 
        {
            get { return _currentMousePositionX; }
            set 
            { 
                _currentMousePositionX = value;
                OnPropertyChanged("CurrentMousePositionX");
            }
        }

        public double CurrentMousePositionY
        {
            get { return _currentMousePositionY; }
            set
            {
                _currentMousePositionY = value;
                OnPropertyChanged("CurrentMousePositionY");
            }
        }

        //get / set selected Object
        private BattlegroundBaseObject _selectedObject;
        public BattlegroundBaseObject SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (!IsMoving)
                {
                    BattlegroundObjects.ToList().ForEach(x => x.IsHighlighted = false);

                    _selectedObject = value;
                    OnPropertyChanged("SelectedObject");
                }
            }
        }

        //get / set delete Command possible
        private BattlegroundCommand _deleteCommand;
        public BattlegroundCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new BattlegroundCommand(Delete)); }
        }

        private void Delete()
        {
            if (SelectedObject is BattlegroundBaseObject)
                BattlegroundObjects.Remove(SelectedObject);
        }

        #region different lines 
        public bool CreateLine
        {
            get { return _line1; }
            set
            {
                _line1 = value;
                //if (_line1) CreatingNewLine = true;
            }
        }

        #endregion

        private bool _creatingNewLine;
        public bool CreatingNewLine
        {
            get { return _creatingNewLine; }
            set
            { 
                _creatingNewLine = value;
                if (!_creatingNewLine) SelectedObject = null;
            }
        }
        
        public PathLine CreateNewPathLine(double x1, double y1)
        {
            var pathline = new PathLine(new Point(x1, y1))
                {
                    ObjectColor = new SolidColorBrush(SelectedColor)
                };
            SelectedObject = pathline;
            BattlegroundObjects.Add(pathline);
            //RemoveNewObjects();
            return pathline;
        }

        public void FinishCurrentPathLine()
        {
            SelectedObject.IsNew = false;
            SelectedObject = null;
            RemoveNewObjects();
        }


        //Move Objects
        private bool _isMoving;
        public bool IsMoving
        {
            get { return _isMoving; }
            set { _isMoving = value; }
        }

        public void MoveObject(double xOld, double yOld, double xNew, double yNew)
        {
            if (SelectedObject != null)
            {
                if (SelectedObject is PathLine)
                {
                    ((PathLine)SelectedObject).MoveObject(xNew-xOld,yNew-yOld);
                }
            }
        }

        private double _currentStrokeThickness;
        public double CurrentStrokeTickness
        {
            get { return _currentStrokeThickness; }
            set
            {
                _currentStrokeThickness = value;
                OnPropertyChanged("CurrentStrokeTickness");
            }
        }

        public void MoveWhileDrawing(double x2, double y2, bool sketchDrawing)
        {
            if (SelectedObject != null)
            {   
                if (SelectedObject is PathLine)
                {
                    if (sketchDrawing)
                    {
                        ((PathLine)SelectedObject).AddNewPointToSeries(new Point(x2, y2));
                    }
                    else
                    {
                        ((PathLine) SelectedObject).ChangeLastPoint(new Point(x2, y2));
                    }
                }
            }
        }

        public void RemoveNewObjects()
        {
            BattlegroundObjects.Where(x => x.IsNew).ToList().ForEach(x => BattlegroundObjects.Remove(x));
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        #endregion

            /*public PathLine CreateDummyPathForTestPurpose(double seed)
            {

                PathFigure hexPathFigure1 = new PathFigure();
                hexPathFigure1.StartPoint = new Point(0, 0);
                PathSegmentCollection hexPathSegementCollection1 = new PathSegmentCollection();
                hexPathSegementCollection1.Add(new LineSegment(new Point(100, 100 + seed), true));
                hexPathSegementCollection1.Add(new LineSegment(new Point(200 - seed, 100), true));
                hexPathSegementCollection1.Add(new LineSegment(new Point(300 + seed, 150), true));
                hexPathSegementCollection1.Add(new LineSegment(new Point(200, 400), true));

                PathFigure hexPathFigure2 = new PathFigure();
                hexPathFigure2.StartPoint = new Point(250, 450);
                PathSegmentCollection hexPathSegementCollection2 = new PathSegmentCollection();
                hexPathSegementCollection2.Add(new LineSegment(new Point(600 - seed, 800 + seed), true));
                hexPathSegementCollection2.Add(new LineSegment(new Point(800 + seed, 1200 - seed), true));
                //hexPathSegementCollection2.Add(new LineSegment(new Point(0,p9.Y), true));

                hexPathFigure1.Segments = hexPathSegementCollection1;
                hexPathFigure2.Segments = hexPathSegementCollection2;

                PathFigureCollection hexFigureCollection = new PathFigureCollection();
                hexFigureCollection.Add(hexPathFigure1);
                hexFigureCollection.Add(hexPathFigure2);

                PathGeometry hexPathGeometry = new PathGeometry();
                hexPathGeometry.Figures = hexFigureCollection;

                var pathLine = new PathLine();
                pathLine.PathGeometryData = hexPathGeometry;

                return pathLine;
            }*/
    }
}
