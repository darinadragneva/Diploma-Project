using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddLoan.xaml
    /// </summary>
    public partial class AddLoan : Window
    {
        private readonly ModelContext context = new ModelContext();
        public int currentUser;
        private MenuWindowForCashier window1;
        private MenuWindow window2;
        private MenuWindowForChairman window3;
        private MenuWindowForAdmin window4;

        public ObservableCollection<Client> clientsCollection { get; set; }
        //public ObservableCollection<Loan> loansCollection { get; set; }

        public class Client
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public string PhoneNumber { get; set; }
            public string? Address { get; set; }
            public double CurrentCapital { get; set; }
            public double AutoPay { get; set; }
            public int DivisionId { get; set; }

            public string DivisionName { get; set; } // New property for displaying the DivisionName in the DataGrid
        }

        public AddLoan(MenuWindowForCashier window)
        {
            InitializeComponent();
            this.window1 = window;
        }

        public AddLoan(MenuWindow window)
        {
            InitializeComponent();
            this.window2 = window;
        }

        public AddLoan(MenuWindowForChairman window)
        {
            InitializeComponent();
            this.window3 = window;
        }

        public AddLoan(MenuWindowForAdmin window)
        {
            InitializeComponent();
            this.window4 = window;
        }

        private bool AllLoansHaveStatus(int clientId)
        {
            var maxLoanNumber = context.Loans.Where(loan => loan.ClientId == clientId).Max(loan => (int?)loan.LoanNumber);

            if (maxLoanNumber.HasValue)
            {
                var loanWithMaxNumber = context.Loans.FirstOrDefault(loan => loan.ClientId == clientId && loan.LoanNumber == maxLoanNumber.Value);

                if (loanWithMaxNumber != null && loanWithMaxNumber.StatusId == 2)
                {
                    return true;
                }
            }

            return false;
        }

        void calculate()
        {
            double loanSum, realLoan, loanInterest, currentCapital, newInterest, newInterest1 = 0;
            int loanMonths = 0;
            try
            {
                currentCapital = double.Parse(textBox1.Text);
                loanSum = double.Parse(textBox3.Text);
                loanInterest = (double.Parse(textBox4.Text))/1200;
                loanMonths = int.Parse(textBox5.Text);
                if (loanSum < currentCapital * 3.1)
                {
                    //realLoan = (loanSum - currentCapital / ((1 + loanInterest) ^ loanMonths)) / ((1 - ((1 + loanInterest) ^ (-loanMonths))) / loanInterest);
                    newInterest1 = loanSum - currentCapital;
                    newInterest = (newInterest1 - 0 / Math.Pow(1 + loanInterest, loanMonths)) / ((1 - Math.Pow(1 + loanInterest, -loanMonths)) / loanInterest);
                    newInterest = newInterest * loanMonths - newInterest1;
                    double newInterest2 = Math.Round(newInterest * 10) / 10;
                    if (newInterest2 < 0)
                    {
                        newInterest2 = 0;
                    }
                    textBox6.Text = newInterest2.ToString("N2");
                    realLoan = double.Parse(textBox6.Text);
                    textBox7.Text = (loanSum - realLoan).ToString("N2");
                }
                else
                {
                    MessageBox.Show($"Клиентът няма право да тегли толкова голям заем!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch { }
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text != "" &&  textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
                calculate();

            double number = Convert.ToDouble(textBox3.Text);
            if (number > 10000)
            {
                textBox3.Foreground = Brushes.Red;
            }
            else
            {
                textBox3.Foreground = Brushes.Black;
            }
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
                calculate();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);
            string currentClientFullName = label8.Content.ToString();

            string[] names = currentClientFullName.Split(' ');
            if (names.Length >= 3)
            {
                string currentClientFirstName = names[0];
                string currentClientMiddleName = names[1];
                string currentClientLastName = names[2];

                var selectedClient = context.Clients.FirstOrDefault(client => client.FirstName == currentClientFirstName && client.MiddleName == currentClientMiddleName && client.LastName == currentClientLastName);
                if (selectedClient != null)
                {
                    var selectedClientLoan = context.Loans.FirstOrDefault(loan => loan.ClientId == selectedClient.Id);
                    if ((selectedClientLoan != null && AllLoansHaveStatus(selectedClient.Id)) || (selectedClientLoan == null))
                    {
                        if ((string.IsNullOrWhiteSpace(dateTimePicker1.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox5.Text)))
                        {
                            MessageBox.Show("Всички полета трябва да са попълнени!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else if (textBox3.Foreground != Brushes.Red)
                        {
                            var maxLoanNumber = 0;
                            if (selectedClientLoan == null) 
                            { 
                                maxLoanNumber = 1; 
                            }
                            else
                            {
                                //maxLoanNumber = selectedClientLoan.LoanNumber + 1;
                                maxLoanNumber = context.Loans.Where(loan => loan.ClientId == selectedClientLoan.ClientId).Max(loan => loan.LoanNumber) + 1;
                            }
                            var newLoan = new Loans
                            {
                                LoanNumber = maxLoanNumber,
                                LoanDate = Convert.ToDateTime(dateTimePicker1.Text),
                                LoanSum = Convert.ToDouble(textBox3.Text),
                                CurrentInterest = Convert.ToDouble(textBox6.Text),
                                RealLoan = Convert.ToDouble(textBox7.Text),
                                LoanRemainder = Convert.ToDouble(textBox3.Text),
                                LoanMonths = Convert.ToInt32(textBox5.Text),
                                ClientId = selectedClient.Id,
                                StatusId = 1
                            };
                            context.Loans.Add(newLoan);
                            context.SaveChanges();
                            MessageBoxResult result = MessageBox.Show($"Заемът е добавен успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (existingUser.PositionId == 1)
                            {
                                window1.LoadDataInDataGrid3new();
                                this.Close();
                            }
                            else if (existingUser.PositionId == 2)
                            {
                                window2.LoadDataInDataGrid3new();
                                this.Close();
                            }
                            else if (existingUser.PositionId == 3)
                            {
                                window3.LoadDataInDataGrid3new();
                                this.Close();
                            }
                            else if (existingUser.PositionId == 4)
                            {
                                window4.LoadDataInDataGrid3new();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Клиентът няма право да тегли заем по-голям от 10,000!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Клиентът вече има активен заем и няма право да тегли друг!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }


    }
}
