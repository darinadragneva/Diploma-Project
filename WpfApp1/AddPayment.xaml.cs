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
    /// Interaction logic for AddPayment.xaml
    /// </summary>
    public partial class AddPayment : Window
    {
        private readonly ModelContext context = new ModelContext();
        private MenuWindowForCashier window1;
        private MenuWindow window2;
        private MenuWindowForChairman window3;
        private MenuWindowForAdmin window4;
        public int currentUser;

        public AddPayment(MenuWindowForCashier window)
        {
            InitializeComponent();
            this.window1 = window;
        }

        public AddPayment(MenuWindow window)
        {
            InitializeComponent();
            this.window2 = window;
        }

        public AddPayment(MenuWindowForChairman window)
        {
            InitializeComponent();
            this.window3 = window;
        }

        public AddPayment(MenuWindowForAdmin window)
        {
            InitializeComponent();
            this.window4 = window;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewTypeOperationInComboBox();
        }

        public void viewTypeOperationInComboBox()
        {
            foreach (var item in context.TypeOperations.ToList())
            {
                comboBox1.Items.Add(item.TypeOperationName);
            }
        }

        void calculate()
        {
            if (textBox4.IsEnabled == false)
            {
                textBox6.Text = textBox3.Text;
            }
            else if (textBox4.IsEnabled == true)
            {
                if (textBox3.Text != "")
                {
                    if (double.TryParse(textBox3.Text, out double paymentForDeposit) && double.TryParse(textBox4.Text, out double paymentForLoan))
                    {
                        textBox6.Text = (paymentForDeposit + paymentForLoan).ToString();
                    }
                }
                else
                {
                    textBox6.Text = textBox4.Text;
                }
            }
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculate();
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculate();
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string currentClientFullName = label1.Content.ToString();

            string[] names = currentClientFullName.Split(' ');
            if (names.Length >= 3)
            {
                string currentClientFirstName = names[0];
                string currentClientMiddleName = names[1];
                string currentClientLastName = names[2];

                var selectedClient = context.Clients.FirstOrDefault(client => client.FirstName == currentClientFirstName && client.MiddleName == currentClientMiddleName && client.LastName == currentClientLastName);
                if (selectedClient != null)
                {
                    var selectedClientPayment = context.Payments.FirstOrDefault(payment => payment.ClientId == selectedClient.Id);
                    if ((string.IsNullOrWhiteSpace(dateTimePicker1.Text) || string.IsNullOrWhiteSpace(comboBox1.Text) || string.IsNullOrWhiteSpace(textBox5.Text)))
                    {
                        MessageBox.Show("Трябва да попълните дата, тип операция и номер на квитанция!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        var maxOperationNumber = 0;
                        if (selectedClientPayment == null)
                        {
                            maxOperationNumber = 1;
                        }
                        else
                        {
                            maxOperationNumber = context.Payments.Where(payment => payment.ClientId == selectedClientPayment.ClientId).Max(payment => payment.OperationNumber) + 1;
                        }

                        if (textBox3.Text == "" && textBox4.Text == "")
                        {
                            MessageBox.Show("Трябва да въведете вноска!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else if (textBox3.Text != "" && textBox4.Text == "")
                        {
                            var newTypeOperation = context.TypeOperations.FirstOrDefault(typeoperation => typeoperation.TypeOperationName == comboBox1.Text);
                            Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);
                            double CurrentCapitalBeforeChange = context.Clients.Where(client => client.Id == selectedClient.Id).Select(client => client.CurrentCapital).FirstOrDefault();
                            var newPayment = new Payments
                            {
                                OperationNumber = maxOperationNumber,
                                PaymentDate = Convert.ToDateTime(dateTimePicker1.Text),
                                CapitalPay = Convert.ToDouble(textBox3.Text),
                                LoanPay = 0,
                                PaymentCapital = CurrentCapitalBeforeChange + Convert.ToDouble(textBox3.Text),
                                DocumentNumber = textBox5.Text,
                                ClientId = selectedClient.Id,
                                TypeOperationId = newTypeOperation.Id,
                                UserId = existingUser.Id
                            };
                            context.Payments.Add(newPayment);
                            selectedClient.CurrentCapital += newPayment.CapitalPay;
                            context.SaveChanges();
                            MessageBoxResult result = MessageBox.Show($"Вноската е добавена успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (existingUser.PositionId == 1)
                            {
                                window1.LoadDataInDataGrid1();
                                window1.LoadDataInDataGrid2new();
                                this.Close();
                            }
                            else if (existingUser.PositionId == 2)
                            {
                                window2.LoadDataInDataGrid1();
                                window2.LoadDataInDataGrid2new();
                                this.Close();
                            }
                            else if (existingUser.PositionId == 3)
                            {
                                window3.LoadDataInDataGrid1();
                                window3.LoadDataInDataGrid2new();
                                this.Close();
                            }
                            else if (existingUser.PositionId == 4)
                            {
                                window4.LoadDataInDataGrid1();
                                window4.LoadDataInDataGrid2new();
                                this.Close();
                            }
                        }
                        else if(textBox3.Text == "" && textBox4.Text != "")
                        {
                            var newTypeOperation = context.TypeOperations.FirstOrDefault(typeoperation => typeoperation.TypeOperationName == comboBox1.Text);
                            Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);
                            var newPayment = new Payments
                            {
                                OperationNumber = maxOperationNumber,
                                PaymentDate = Convert.ToDateTime(dateTimePicker1.Text),
                                CapitalPay = 0,
                                LoanPay = Convert.ToDouble(textBox4.Text),
                                PaymentCapital = selectedClient.CurrentCapital,
                                DocumentNumber = textBox5.Text,
                                ClientId = selectedClient.Id,
                                TypeOperationId = newTypeOperation.Id,
                                UserId = existingUser.Id
                            };
                            context.Payments.Add(newPayment);
                            var selectedLoanRemainder = context.Loans.FirstOrDefault(loan => loan.ClientId == selectedClient.Id && loan.StatusId != 2);
                            if (selectedLoanRemainder != null)
                            {
                                selectedLoanRemainder.LoanRemainder = selectedLoanRemainder.LoanRemainder - newPayment.LoanPay;
                                context.SaveChanges();
                                MessageBoxResult result = MessageBox.Show($"Вноската е добавена успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                if (existingUser.PositionId == 1)
                                {
                                    window1.LoadDataInDataGrid2new();
                                    window1.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                                else if (existingUser.PositionId == 2)
                                {
                                    window2.LoadDataInDataGrid2new();
                                    window2.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                                else if (existingUser.PositionId == 3)
                                {
                                    window3.LoadDataInDataGrid2new();
                                    window3.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                                else if (existingUser.PositionId == 4)
                                {
                                    window4.LoadDataInDataGrid2new();
                                    window4.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                            }
                            
                        }
                        else if (textBox3.Text != "" && textBox4.Text != "")
                        {
                            var newTypeOperation = context.TypeOperations.FirstOrDefault(typeoperation => typeoperation.TypeOperationName == comboBox1.Text);
                            Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);
                            double CurrentCapitalBeforeChange = context.Clients.Where(client => client.Id == selectedClient.Id).Select(client => client.CurrentCapital).FirstOrDefault();
                            var newPayment = new Payments
                            {
                                OperationNumber = maxOperationNumber,
                                PaymentDate = Convert.ToDateTime(dateTimePicker1.Text),
                                CapitalPay = Convert.ToDouble(textBox3.Text),
                                LoanPay = Convert.ToDouble(textBox4.Text),
                                PaymentCapital = CurrentCapitalBeforeChange + Convert.ToDouble(textBox3.Text),
                                DocumentNumber = textBox5.Text,
                                ClientId = selectedClient.Id,
                                TypeOperationId = newTypeOperation.Id,
                                UserId = existingUser.Id
                            };
                            context.Payments.Add(newPayment);
                            selectedClient.CurrentCapital += newPayment.CapitalPay;
                            var selectedLoanRemainder = context.Loans.FirstOrDefault(loan => loan.ClientId == selectedClient.Id && loan.StatusId != 2);
                            if (selectedLoanRemainder != null)
                            {
                                selectedLoanRemainder.LoanRemainder = selectedLoanRemainder.LoanRemainder - newPayment.LoanPay;
                                context.SaveChanges();
                                MessageBoxResult result = MessageBox.Show($"Вноската е добавена успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                if (existingUser.PositionId == 1)
                                {
                                    window1.LoadDataInDataGrid1();
                                    window1.LoadDataInDataGrid2new();
                                    window1.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                                else if(existingUser.PositionId == 2)
                                {
                                    window2.LoadDataInDataGrid1();
                                    window2.LoadDataInDataGrid2new();
                                    window2.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                                else if (existingUser.PositionId == 3)
                                {
                                    window3.LoadDataInDataGrid1();
                                    window3.LoadDataInDataGrid2new();
                                    window3.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                                else if (existingUser.PositionId == 4)
                                {
                                    window4.LoadDataInDataGrid1();
                                    window4.LoadDataInDataGrid2new();
                                    window4.LoadDataInDataGrid3new();
                                    this.Close();
                                }
                            }
                        }
                        
                    }
                }
            }
        }
    }
}
