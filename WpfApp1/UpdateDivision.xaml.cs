using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for UpdateDivision.xaml
    /// </summary>
    public partial class UpdateDivision : Window
    {
        public UpdateDivision()
        {
            InitializeComponent();
        }

        public int currentDivision;

        private readonly ModelContext context = new ModelContext();
        
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string newDivisionName = textBox1.Text;
            double newDivisionInterest = Convert.ToDouble(textBox2.Text);
            Divisions existingDivision = context.Divisions.FirstOrDefault(division => division.Id == currentDivision);
            if (existingDivision != null)
            {
                if (!string.IsNullOrEmpty(newDivisionName))
                {
                    existingDivision.DivisionName = newDivisionName;
                }
                if (newDivisionInterest != existingDivision.Interest)
                {
                    existingDivision.Interest = newDivisionInterest;
                }
                context.SaveChanges();
                MessageBox.Show("Поделението е редактирано успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Поделението не е намерено!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}