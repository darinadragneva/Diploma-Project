using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection;
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
using static WpfApp1.MenuWindow;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        private readonly ModelContext context = new ModelContext();
        private MenuWindowForCashier window1;
        private MenuWindow window2;
        private MenuWindowForChairman window3;
        private MenuWindowForAdmin window4;
        public int currentUser;

        public AddClient(MenuWindowForCashier window)
        {
            InitializeComponent();
            this.window1 = window;
        }

        public AddClient(MenuWindow window)
        {
            InitializeComponent();
            this.window2 = window;
        }

        public AddClient(MenuWindowForChairman window)
        {
            InitializeComponent();
            this.window3 = window;
        }

        public AddClient(MenuWindowForAdmin window)
        {
            InitializeComponent();
            this.window4 = window;
        }

        public void viewPositionInComboBox()
        {
            foreach (var item in context.Divisions.ToList())
            {
                comboBox1.Items.Add(item.DivisionName);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewPositionInComboBox();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);
            string ClientFirstName = textBox1.Text;
            string ClientMiddleName = textBox2.Text;
            string ClientLastName = textBox3.Text;
            string ClientDivisionId = comboBox1.Text;
            string ClientGender = textBox4.Text;
            double ClientCurrentCapital = Convert.ToDouble(textBox5.Text);
            double ClientAutoPay = Convert.ToDouble(textBox6.Text);
            string ClientPhoneNumber = textBox7.Text;
            string ClientAddress = textBox8.Text;

            var newDivision = context.Divisions.FirstOrDefault(division => division.DivisionName == comboBox1.Text);
            var newClient = new Clients
            {
                FirstName = ClientFirstName,
                MiddleName = ClientMiddleName,
                LastName = ClientLastName,
                Gender = ClientGender,
                PhoneNumber = ClientPhoneNumber,   
                Address = ClientAddress,
                CurrentCapital = ClientCurrentCapital,
                AutoPay = ClientAutoPay,
                DivisionId = newDivision.Id
            };

            context.Clients.Add(newClient);
            context.SaveChanges();
            MessageBoxResult result = MessageBox.Show($"Клиентът е добавен успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            if (existingUser.PositionId == 1)
            {
                window1.LoadDataInDataGrid1();
                this.Close();
            }
            else if (existingUser.PositionId == 2)
            {
                window2.LoadDataInDataGrid1();
                this.Close();
            }
            else if (existingUser.PositionId == 3)
            {
                window3.LoadDataInDataGrid1();
                this.Close();
            }
            else if (existingUser.PositionId == 4)
            {
                window4.LoadDataInDataGrid1();
                this.Close();
            }
        }

    }
}
