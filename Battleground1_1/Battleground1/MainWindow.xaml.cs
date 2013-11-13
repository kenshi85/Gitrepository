using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Battleground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public List<FullLine> Lines { get; set; }

        private double _x1, _y1, _x2, _y2;
        private double _xMovingOld, _yMovingOld;
        private bool _zoomChanged = false;
        //private bool _isMoving = false;
        private bool _leftShiftPressed = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new BattlegroundViewModel();
            var vm = DataContext as BattlegroundViewModel;
            vm.TilePathData = BattlegroundUtilities.HexCellTile(100);
            ArenaGrid.Cursor = Cursors.Arrow;
        }

        private int _numberDrawnItems = 0;

        private double _visualisationWidth = 100;
        public double VisualisationWidth
        {
            get { return _visualisationWidth; }
            private set { _visualisationWidth = value; }
        }

        private double _visualisationHeight = 100;
        public double VisualisationHeight
        {
            get { return _visualisationHeight; }
            private set { _visualisationHeight = value; }
        }

        //Zoom Funktion
        private void ArenaGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) uiScaleSlider.Value += 0.1;
            else uiScaleSlider.Value -= 0.1;
            _zoomChanged = true;
        }

        /*private void LineOne_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)(sender as ToggleButton).IsChecked)
            {
                //LineTwo.IsChecked = false;
                //LineThree.IsChecked = false;
            }
        }*/

        private void ArenaGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as BattlegroundViewModel;
            if (vm != null)
            {   
                //just testpathline... can be deleted...
                    //var testpath2 = vm.CreateDummyPathForTestPurpose(99);
                    //testpath2.ObjectColor = new SolidColorBrush(Colors.Coral);
                    //vm.BattlegroundObjects.Add(testpath2);
                //_leftShiftPressed = (Keyboard.GetKeyStates(Key.LeftShift) == KeyStates.Down);
                //handling different possibilities based on Objects (like Line1 Button)
                if (vm.CreateLine )
                {
                        _x1 = e.GetPosition(ArenaGrid).X;
                        _y1 = e.GetPosition(ArenaGrid).Y;
                        vm.CreatingNewLine = true;
                        var line = vm.CreateNewPathLine(_x1, _y1);
                        //line.ObjectColor = new SolidColorBrush(Colors.DarkBlue);
                        //line.ChangeLastPoint(new Point(_x1 + 1000, _y1 + 10000));
                        line.IsNew = true;
                        e.Handled = true;
                }
            }
        }

        private void ArenaGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as BattlegroundViewModel;
            if (vm != null)
            {
                //handling different possibilities based on Objects (like Line1 Button)
                if (vm.CreatingNewLine)
                {
                            vm.FinishCurrentPathLine();
                            e.Handled = true;
                            vm.CreatingNewLine = false;
                }
            }
        }

        private void ArenaGrid_MouseMove(object sender, MouseEventArgs e)
        {
            var vm = DataContext as BattlegroundViewModel;
            if (vm != null)
            {
                //cursor pixelchange?
                if (Math.Round(e.GetPosition(ArenaGrid).X, 0) != vm.CurrentMousePositionX ||
                    Math.Round(e.GetPosition(ArenaGrid).Y, 0) != vm.CurrentMousePositionY)
                {
                    //_leftShiftPressed = (Keyboard.GetKeyStates(Key.LeftShift) == KeyStates.Down);

                    vm.CurrentMousePositionX = e.GetPosition(ArenaGrid).X;
                    vm.CurrentMousePositionY = e.GetPosition(ArenaGrid).Y;
                    var listboxItem = ((DependencyObject)e.OriginalSource).FindAnchestor<ListBoxItem>();
                    //handling different possibilities based on Objects (like Line or Hero..)
                    if (vm.CreatingNewLine)
                    {
                        _x2 = e.GetPosition(ArenaGrid).X;
                        _y2 = e.GetPosition(ArenaGrid).Y;
                        var leftShiftState2 = Keyboard.GetKeyStates(Key.LeftShift);
                        vm.MoveWhileDrawing(_x2, _y2, _leftShiftPressed);
                    }
                    else if(e.LeftButton == MouseButtonState.Pressed && vm.SelectedObject is BattlegroundBaseObject && vm.IsMoving) 
                    {
                        vm.MoveObject(_xMovingOld, _yMovingOld, vm.CurrentMousePositionX, vm.CurrentMousePositionY);
                    }
                    _xMovingOld = vm.CurrentMousePositionX;
                    _yMovingOld = vm.CurrentMousePositionY;
                }
            }
        }

        private void ArenaScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var vm = DataContext as BattlegroundViewModel;
            if (vm != null)
            {
                if (_zoomChanged)
                {
                    ArenaScrollViewer.ScrollToHorizontalOffset(vm.CurrentMousePositionX*ArenaScrollViewer.ScrollableWidth/10000);
                    ArenaScrollViewer.ScrollToVerticalOffset(vm.CurrentMousePositionY*ArenaScrollViewer.ScrollableHeight/10000);
                    _zoomChanged = false;
                    vm.CurrentMousePositionX = Mouse.GetPosition(ArenaGrid).X;
                    vm.CurrentMousePositionY = Mouse.GetPosition(ArenaGrid).Y;
                }
            }
        }

        private void SetIsMovingWithAnimation(Boolean _isMoving)
        {   
            var vm = DataContext as BattlegroundViewModel;
            if (vm != null)
            {
                ArenaGrid.Cursor = ArenaGrid.Cursor == Cursors.Arrow ? Cursors.Hand : Cursors.Arrow;
                vm.SelectedObject.IsMoving = _isMoving;
            }
        }

        private void ArenaGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
             var vm = DataContext as BattlegroundViewModel;
             if (vm != null)
             {
                 vm.IsMoving = false;
             }
        }

        private void ArenaGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as BattlegroundViewModel;
             if (vm != null)
             {
                 var listboxItem = ((DependencyObject)e.OriginalSource).FindAnchestor<ListBoxItem>();
                 if (listboxItem != null)
                 {
                     BattlegroundBaseObject o = ArenaGrid.ItemContainerGenerator.ItemFromContainer(listboxItem) as BattlegroundBaseObject;
                     vm.SelectedObject = o;
                     vm.IsMoving = true;
                     e.Handled = true;
                 }
             }
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _leftShiftPressed = (e.Key == Key.LeftShift);
            Console.WriteLine("Key: " + e.Key + "  || KeyState: "+ e.KeyStates + "  || _leftShiftPressed: "+ _leftShiftPressed);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.LeftShift) _leftShiftPressed = !_leftShiftPressed;
            Console.WriteLine("Key: " + e.Key + "  || KeyState: " + e.KeyStates + "  || _leftShiftPressed: " + _leftShiftPressed);
        }
    }
}
