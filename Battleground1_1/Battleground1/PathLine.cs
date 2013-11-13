using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Battleground
{
    public class PathLine : BattlegroundBaseObject
    {
        //locals
        private PathGeometry _pathGeometryData = new PathGeometry();
        private PathFigureCollection _pathFigureCollection = new PathFigureCollection();
        private PathFigure _pathFigure = new PathFigure();
        private PathSegmentCollection _pathSegmentCollection = new PathSegmentCollection();
        private double _opacity = 1;
        private double _strokeTickness = 3;
        //private SolidColorBrush _pathLineColorBrush;

        
        public PathLine()
        {

        }

        public PathLine(Point p)
        {
            CreateNewPath(p);
        }

        #region PathGeometry

        public PathGeometry PathGeometryData
        {
            get { return _pathGeometryData; }
            set
            {
                _pathGeometryData = value;
                OnPropertyChanged("PathGeometryData");
            }
        }

        //Creates new path with p at start and endpoint
        public void CreateNewPath(Point p)
        {
            _pathFigure.StartPoint = p;
            _pathSegmentCollection.Add(new LineSegment(p,true));
            _pathFigure.Segments = _pathSegmentCollection;
            _pathFigureCollection.Add(_pathFigure);
            PathGeometryData.Figures = _pathFigureCollection;
        }
         
        public void AddNewPointToSeries(Point p)
        {
            _pathSegmentCollection.Add(new LineSegment(p,true));
            OnPropertyChanged("PathGeometryData");
            //Console.WriteLine("addnewpathlinepoint");
        }

        public void ChangeFirstPoint(Point p)
        {
            _pathFigure.StartPoint = p;
            OnPropertyChanged("PathGeometryData");
        }

        public void ChangeLastPoint(Point p) 
        {
            ((LineSegment)_pathSegmentCollection[_pathSegmentCollection.Count - 1]).Point = p;
            OnPropertyChanged("PathGeometryData");
            //Console.WriteLine("changelastpathlinepoint to: " + ((LineSegment)_pathSegmentCollection[_pathSegmentCollection.Count - 1]).Point.ToString());
        }

        public void MoveObject(double deltaX, double deltaY)
        {
            //Console.WriteLine("------------------------------------------------------------");
            foreach (LineSegment l in _pathSegmentCollection.ToList())
            {
                Point newPoint = new Point(l.Point.X + deltaX, l.Point.Y+deltaY);
                //Console.WriteLine("MOVING OBJECT FROM:  " + l.Point.ToString() + "  TO:  " + newPoint.ToString());
                l.Point = newPoint;
            }

            //change first Point at last cause of OnPropertyChanged Event...
            Point newStartPoint = new Point(_pathFigure.StartPoint.X + deltaX, _pathFigure.StartPoint.Y+deltaY);
            //Console.WriteLine("MOVING Start from OBJECT:  " + _pathFigure.StartPoint.ToString() + "  TO:  " + newStartPoint.ToString());
            ChangeFirstPoint(newStartPoint);

            //OnPropertyChanged("PathGeometryData");
        }

        #endregion

        #region color and stuff

        public Color ShadowColor { get; set; }
        public Color SelectionColor { get; set; }

        public double StrokeStrokeTickness
        {
            get { return _strokeTickness; }
            set
            {
                _strokeTickness = value;
                OnPropertyChanged("StrokeStrokeTickness");
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

        /*public SolidColorBrush PathLineColorBrush
        {
            get { return _pathLineColorBrush; }
            set
            {
                _pathLineColorBrush = value;
                OnPropertyChanged("PathLineColorBrush");
            }
        }
        */
        #endregion
    }
}
