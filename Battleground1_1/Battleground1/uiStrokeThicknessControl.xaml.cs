using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battleground
{
    /// <summary>
    /// Interaction logic for uiStrokeThicknessControl.xaml
    /// </summary>
    public partial class uiStrokeThicknessControl : UserControl
    {
        public uiStrokeThicknessControl()
        {
            UiStrokeThickness = 3;
            InitializeComponent();
            UiStrokeThickness = 3;

        }



        private double _uiStrokeThickness;
        public double UiStrokeThickness
        {
            get { return _uiStrokeThickness; }
            set
            {
                _uiStrokeThickness = value;
                OnPropertyChanged("UiStrokeThickness");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void uiStrokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (uiStrokeThicknessTextBox == null) return;
            UiStrokeThickness = uiStrokeThicknessSlider.Value;
            uiStrokeThicknessTextBox.Text = UiStrokeThickness.ToString();
        }

       
        private void uiStrokeThicknessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double ui = UiStrokeThickness;
            if (double.TryParse(uiStrokeThicknessTextBox.Text, out ui))
            {
                uiStrokeThicknessSlider.Value = ui;
            }
        }
    }
}
