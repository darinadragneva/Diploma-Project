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
    /// Interaction logic for AddDivision.xaml
    /// </summary>
    public partial class AddDivision : Window
    {
        public AddDivision()
        {
            InitializeComponent();
        }

        private readonly ModelContext context = new ModelContext();


        //public string DivisionName { get; set; }
        //public double DivisionInterest { get; set; }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string DivisionName = textBox1.Text;
            double DivisionInterest = Convert.ToDouble(textBox2.Text);

            var newDivision = new Divisions
            {
                DivisionName = DivisionName,
                Interest = DivisionInterest
            };
            
            context.Divisions.Add(newDivision);
            context.SaveChanges();
            MessageBoxResult result = MessageBox.Show($"Поделението е добавено успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
