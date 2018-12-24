using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Uhr
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            this.DataContext = vm;

            // Layout aufbauen 
            const int newLine = 11;
            StackPanel sp = null;
            for (int i = 0; i < Ziffernblatt.MaxDigits; i++)
            {
                if (i % newLine == 0)
                {
                    if (sp != null) StackParent.Children.Add(sp);
                    sp = new StackPanel() { Orientation = Orientation.Horizontal };
                }
                var digi = new ucDigit(); 
                // Binding Selection-Property
                var propName = $"{nameof(MainViewModel.Selected)}[{i}]";
                Bind(digi, ucDigit.IsSelectedProperty, propName);
                // Binding Text-Property
                propName = $"{nameof(MainViewModel.Text)}[{i}]";
                Bind(digi, ucDigit.TextProperty, propName);
                // Add Digit
                sp.Children.Add(digi);
            }
            if (sp != null) StackParent.Children.Add(sp);
        }

        private void Bind(DependencyObject target, DependencyProperty dp, string propName)
        {
            Binding binding = new Binding()
            {
                Path = new PropertyPath(propName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
            BindingOperations.SetBinding(target, dp, binding);
        }

    }
}
