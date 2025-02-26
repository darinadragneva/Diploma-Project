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
using static WpfApp1.MenuWindow;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Log_in.xaml
    /// </summary>
    public partial class Log_in : Window
    {
        public Log_in()
        {
            InitializeComponent();
        }

        private readonly ModelContext context = new ModelContext();

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox1.Password.ToString();
            var user = context.Users.FirstOrDefault(user => user.Username == textBox1.Text);
            if (user != null)
            {
                string storedHashedPassword = user.Pass;
                var sensitivedata = new SensitiveDataEncryption();
                byte[] storedSalt = Convert.FromBase64String(user.Salt);
                bool passwordIsCorrect = sensitivedata.VerifySensitiveData(password, storedHashedPassword, storedSalt);
                if (passwordIsCorrect)
                {
                    if (user.PositionId == 1) //касиер 
                    {
                        string userFirstName = user.FirstName;
                        string userLastName = user.LastName;
                        string combinedNames = $"{userFirstName} {userLastName}";
                        var positionName = context.Positions.FirstOrDefault(position => position.Id == user.PositionId)?.PositionName;
                        MenuWindowForCashier window = new MenuWindowForCashier();
                        window.currentUser = user.Id;
                        window.menuItem3.Header = combinedNames;
                        window.label3.Content = positionName;
                        window.SetSelectedPositionName(positionName);
                        this.Close();
                        window.ShowDialog();
                    }
                    else if (user.PositionId == 2) //счетоводиттел 
                    {
                        string userFirstName = user.FirstName;
                        string userLastName = user.LastName;
                        string combinedNames = $"{userFirstName} {userLastName}";
                        var positionName = context.Positions.FirstOrDefault(position => position.Id == user.PositionId)?.PositionName;
                        MenuWindow window = new MenuWindow();
                        window.currentUser = user.Id;
                        window.menuItem3.Header = combinedNames;
                        window.label3.Content = positionName;
                        window.SetSelectedPositionName(positionName);
                        this.Close();
                        window.ShowDialog();
                    }
                    else if (user.PositionId == 3) //председател - преглед
                    {
                        string userFirstName = user.FirstName;
                        string userLastName = user.LastName;
                        string combinedNames = $"{userFirstName} {userLastName}";
                        var positionName = context.Positions.FirstOrDefault(position => position.Id == user.PositionId)?.PositionName;
                        MenuWindowForChairman window = new MenuWindowForChairman();
                        window.currentUser = user.Id;
                        window.menuItem3.Header = combinedNames;
                        window.label3.Content = positionName;
                        window.SetSelectedPositionName(positionName);
                        this.Close();
                        window.ShowDialog();
                    }
                    else if (user.PositionId == 4) //admin - всичко + триене на всеки заем и вноска без да трябва да е с максимален номер
                    {
                        string userFirstName = user.FirstName;
                        string userLastName = user.LastName;
                        string combinedNames = $"{userFirstName} {userLastName}";
                        var positionName = context.Positions.FirstOrDefault(position => position.Id == user.PositionId)?.PositionName;
                        MenuWindowForAdmin window = new MenuWindowForAdmin();
                        window.currentUser = user.Id;
                        window.menuItem3.Header = combinedNames;
                        window.label3.Content = positionName;
                        window.SetSelectedPositionName(positionName);
                        this.Close();
                        window.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Грешно потребителско име или парола!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Грешно потребителско име или парола!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.ShowDialog();
        }
    }
}