using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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
using static WpfApp1.MenuWindowForAdmin;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout.Properties;
using Table = iText.Layout.Element.Table;
using Paragraph = iText.Layout.Element.Paragraph;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Colors;
using System.IO;
using iText.Layout;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MenuWindowForAdmin.xaml
    /// </summary>
    public partial class MenuWindowForAdmin : Window
    {
        public MenuWindowForAdmin()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private readonly ModelContext context = new ModelContext();
        private Client selectedClient;
        public static readonly String FONT = "C:\\UNIVERSITY\\4 kurs\\2 semestur\\Дипломна работа\\WpfApp1\\WpfApp1\\Resources\\Fonts\\Roboto-Light.ttf";

        public ObservableCollection<Client> clientsCollection { get; set; }
        public ObservableCollection<Loan> loansCollection { get; set; }
        public ObservableCollection<Payment> paymentsCollection { get; set; }

        public int currentUser;
        private string selectedPositionName;

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

            public string DivisionName { get; set; }
        }

        public class Loan
        {
            public int Id { get; set; }
            public int LoanNumber { get; set; }
            public DateTime LoanDate { get; set; }
            public double LoanSum { get; set; }
            public int LoanMonths { get; set; }
            public double LoanRemainder { get; set; }
            public int StatusId { get; set; }

            public string LoanStatusName { get; set; }
        }

        public class Payment
        {
            public int Id { get; set; }
            public int OperationNumber { get; set; }
            public DateTime PaymentDate { get; set; }
            public double CapitalPay { get; set; }
            public double LoanPay { get; set; }
            public double TotalPay { get; set; }
            public double PaymentCapital { get; set; }
            public string DocumentNumber { get; set; }
            public string TypeOperationName { get; set; }
            public int ClientId { get; set; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataInDataGrid1();
            viewPositionInComboBox();
            dataGrid1.MouseRightButtonDown += dataGrid1_MouseRightButtonDown;
            Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);
            var newPosition = context.Positions.FirstOrDefault(position => position.PositionName == label3.Content);

            int lastActiveClientId = Properties.Settings.Default.LastActiveClientId;
            selectedClient = clientsCollection.FirstOrDefault(client => client.Id == lastActiveClientId);
            if (selectedClient != null)
            {
                dataGrid1.SelectedItem = selectedClient;
                LoadDataInDataGrid2(selectedClient);
                LoadDataInDataGrid3(selectedClient);
            }
        }

        public void LoadDataInDataGrid1()
        {
            var clientsQuery = from client in context.Clients
                               join division in context.Divisions
                               on client.DivisionId equals division.Id
                               select new Client
                               {
                                   Id = client.Id,
                                   FirstName = client.FirstName,
                                   MiddleName = client.MiddleName,
                                   LastName = client.LastName,
                                   Gender = client.Gender,
                                   PhoneNumber = client.PhoneNumber,
                                   Address = client.Address,
                                   CurrentCapital = client.CurrentCapital,
                                   AutoPay = client.AutoPay,
                                   DivisionId = client.DivisionId,
                                   DivisionName = division.DivisionName
                               };
            clientsCollection = new ObservableCollection<Client>(clientsQuery.ToList());
            dataGrid1.ItemsSource = clientsCollection;
        }

        public void LoadDataInDataGrid2(Client selectedClient)
        {
            if (selectedClient != null)
            {
                var paymentsQuery = from payment in context.Payments
                                    join typeoperation in context.TypeOperations
                                    on payment.TypeOperationId equals typeoperation.Id
                                    where payment.ClientId == selectedClient.Id
                                    orderby payment.PaymentDate descending, payment.OperationNumber descending
                                    select new Payment
                                    {
                                        Id = payment.Id,
                                        OperationNumber = payment.OperationNumber,
                                        PaymentDate = payment.PaymentDate,
                                        CapitalPay = payment.CapitalPay,
                                        LoanPay = payment.LoanPay,
                                        TotalPay = payment.CapitalPay + payment.LoanPay,
                                        PaymentCapital = payment.PaymentCapital,
                                        DocumentNumber = payment.DocumentNumber,
                                        TypeOperationName = typeoperation.TypeOperationName
                                    };
                paymentsCollection = new ObservableCollection<Payment>(paymentsQuery.ToList());
                dataGrid2.ItemsSource = paymentsCollection;
            }
        }

        public void LoadDataInDataGrid2new()
        {
            var paymentsQuery = from payment in context.Payments
                                join typeoperation in context.TypeOperations
                                on payment.TypeOperationId equals typeoperation.Id
                                where payment.ClientId == selectedClient.Id
                                orderby payment.PaymentDate descending, payment.OperationNumber descending
                                select new Payment
                                {
                                    Id = payment.Id,
                                    OperationNumber = payment.OperationNumber,
                                    PaymentDate = payment.PaymentDate,
                                    CapitalPay = payment.CapitalPay,
                                    LoanPay = payment.LoanPay,
                                    TotalPay = payment.CapitalPay + payment.LoanPay,
                                    PaymentCapital = payment.PaymentCapital,
                                    DocumentNumber = payment.DocumentNumber,
                                    TypeOperationName = typeoperation.TypeOperationName
                                };
            paymentsCollection = new ObservableCollection<Payment>(paymentsQuery.ToList());
            dataGrid2.ItemsSource = paymentsCollection;
        }

        public void LoadDataInDataGrid3(Client selectedClient)
        {
            if (selectedClient != null)
            {
                var loansQuery = from loan in context.Loans
                                 join loanstatus in context.LoanStatus
                                 on loan.StatusId equals loanstatus.Id
                                 where loan.ClientId == selectedClient.Id
                                 orderby loan.LoanDate descending, loan.LoanNumber descending
                                 select new Loan
                                 {
                                     Id = loan.Id,
                                     LoanNumber = loan.LoanNumber,
                                     LoanDate = loan.LoanDate,
                                     LoanSum = loan.LoanSum,
                                     LoanMonths = loan.LoanMonths,
                                     LoanRemainder = loan.LoanRemainder,
                                     StatusId = loan.StatusId,
                                     LoanStatusName = loanstatus.LoanStatusName
                                 };
                loansCollection = new ObservableCollection<Loan>(loansQuery.ToList());
                dataGrid3.ItemsSource = loansCollection;
            }
        }

        public void LoadDataInDataGrid3new()
        {
            var loansQuery = from loan in context.Loans
                             join loanstatus in context.LoanStatus
                             on loan.StatusId equals loanstatus.Id
                             where loan.ClientId == selectedClient.Id
                             orderby loan.LoanDate descending, loan.LoanNumber descending
                             select new Loan
                             {
                                 Id = loan.Id,
                                 LoanNumber = loan.LoanNumber,
                                 LoanDate = loan.LoanDate,
                                 LoanSum = loan.LoanSum,
                                 LoanMonths = loan.LoanMonths,
                                 LoanRemainder = loan.LoanRemainder,
                                 StatusId = loan.StatusId,
                                 LoanStatusName = loanstatus.LoanStatusName
                             };
            loansCollection = new ObservableCollection<Loan>(loansQuery.ToList());
            dataGrid3.ItemsSource = loansCollection;
        }

        private void LoadClientsForSpecificDivision(int divisionId)
        {
            var clientsQuery = from client in context.Clients
                               join division in context.Divisions
                               on client.DivisionId equals division.Id
                               where client.DivisionId == divisionId
                               select new Client
                               {
                                   Id = client.Id,
                                   FirstName = client.FirstName,
                                   MiddleName = client.MiddleName,
                                   LastName = client.LastName,
                                   Gender = client.Gender,
                                   PhoneNumber = client.PhoneNumber,
                                   Address = client.Address,
                                   CurrentCapital = client.CurrentCapital,
                                   AutoPay = client.AutoPay,
                                   DivisionId = client.DivisionId,
                                   DivisionName = division.DivisionName
                               };
            clientsCollection = new ObservableCollection<Client>(clientsQuery.ToList());
            dataGrid1.ItemsSource = clientsCollection;
            dataGrid1.Items.Refresh();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                selectedClient = dataGrid1.SelectedItem as Client;
                LoadDataInDataGrid2(selectedClient);
                LoadDataInDataGrid3(selectedClient);
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedDivisionName = comboBox1.SelectedItem.ToString();
                if (selectedDivisionName == "+")
                {
                    AddDivision window = new AddDivision();
                    window.ShowDialog();
                    viewPositionInComboBox();
                }
                else
                {
                    var selectedDivision = context.Divisions.FirstOrDefault(division => division.DivisionName == selectedDivisionName);

                    if (selectedDivision != null)
                    {
                        LoadClientsForSpecificDivision(selectedDivision.Id);
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dataGrid1.SelectedItem is Client selectedClient)
            {
                Properties.Settings.Default.LastActiveClientId = selectedClient.Id;
                Properties.Settings.Default.Save();
            }
        }

        public void viewPositionInComboBox()
        {
            comboBox1.Items.Clear();
            foreach (var item in context.Divisions.ToList())
            {
                comboBox1.Items.Add(item.DivisionName);
            }
            comboBox1.Items.Add("+");
        }

        public void SetSelectedPositionName(string positionName)
        {
            selectedPositionName = positionName;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string currentUserFullName = menuItem3.Header.ToString();
            string[] names = currentUserFullName.Split(' ');
            if (names.Length >= 2)
            {
                string currentUserFirstName = names[0];
                string currentUserLastName = names[1];
                var selectedUser = context.Users.FirstOrDefault(user => user.FirstName == currentUserFirstName && user.LastName == currentUserLastName);
                AddClient window = new AddClient(this);
                window.currentUser = selectedUser.Id;
                window.ShowDialog();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                Client selectedClient = (Client)dataGrid1.SelectedItem;
                MessageBoxResult result = MessageBox.Show($"Сигурни ли сте, че искате да изтриете този потребител \n '{selectedClient.FirstName + " " + selectedClient.LastName}'?", "Потвърждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    clientsCollection.Remove(selectedClient);
                    var dbClient = context.Clients.Find(selectedClient.Id);
                    context.Clients.Remove(dbClient);
                    context.SaveChanges();
                    dataGrid1.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show($"Не сте избрали клиент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Row.DataContext is Client editedClient)
                {
                    var existingClient = context.Clients.Find(editedClient.Id);
                    var newDivision1 = context.Divisions.FirstOrDefault(division => division.DivisionName == editedClient.DivisionName);

                    if (existingClient != null)
                    {
                        existingClient.FirstName = editedClient.FirstName;
                        existingClient.MiddleName = editedClient.MiddleName;
                        existingClient.LastName = editedClient.LastName;
                        existingClient.Gender = editedClient.Gender;
                        existingClient.PhoneNumber = editedClient.PhoneNumber;
                        existingClient.Address = editedClient.Address;
                        existingClient.CurrentCapital = editedClient.CurrentCapital;
                        existingClient.AutoPay = editedClient.AutoPay;
                        existingClient.DivisionId = newDivision1.Id;
                    }
                    context.SaveChanges();
                }
            }
        }

        private void dataGrid2_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Row.DataContext is Payment editedPayment)
                {
                    var existingPayment = context.Payments.Find(editedPayment.Id);
                    var newTypeOperation1 = context.TypeOperations.FirstOrDefault(typeoperation => typeoperation.TypeOperationName == editedPayment.TypeOperationName);
                    if (existingPayment != null)
                    {
                        existingPayment.OperationNumber = editedPayment.OperationNumber;
                        existingPayment.PaymentDate = editedPayment.PaymentDate;
                        existingPayment.CapitalPay = editedPayment.CapitalPay;
                        existingPayment.LoanPay = editedPayment.LoanPay;
                        existingPayment.PaymentCapital = editedPayment.PaymentCapital;
                        existingPayment.DocumentNumber = editedPayment.DocumentNumber;
                        existingPayment.TypeOperationId = newTypeOperation1.Id;
                    }
                    context.SaveChanges();
                }
            }
        }

        private void dataGrid3_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Row.DataContext is Loan editedLoan)
                {
                    var existingLoan = context.Loans.Find(editedLoan.Id);
                    var newStatus1 = context.LoanStatus.FirstOrDefault(status => status.LoanStatusName == editedLoan.LoanStatusName);

                    if (existingLoan != null)
                    {
                        existingLoan.LoanNumber = editedLoan.LoanNumber;
                        existingLoan.LoanDate = editedLoan.LoanDate;
                        existingLoan.LoanSum = editedLoan.LoanSum;
                        existingLoan.LoanMonths = editedLoan.LoanMonths;
                        existingLoan.LoanRemainder = editedLoan.LoanRemainder;
                        existingLoan.StatusId = newStatus1.Id;
                    }
                    context.SaveChanges();
                }
            }
        }

        private void dataGrid1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem addItem1 = new MenuItem { Header = "Теглене на заем" };
                //MenuItem addItem2 = new MenuItem { Header = "Теглене на дялов капитал" };
                MenuItem addItem3 = new MenuItem { Header = "Добави вноска" };
                addItem1.Click += (menuSender, menuE) => AddWithdrawalOfALoan_Click(menuSender, menuE, sender);
                //addItem2.Click += (menuSender, menuE) => AddWithdrawalOfACapital_Click(menuSender, menuE, sender);
                addItem3.Click += (menuSender, menuE) => AddPayment_Click(menuSender, menuE, sender);
                contextMenu.Items.Add(addItem1);
                //contextMenu.Items.Add(addItem2);
                contextMenu.Items.Add(addItem3);

                DataGrid dataGrid = (DataGrid)sender;
                DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

                contextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void AddWithdrawalOfALoan_Click(object sender, RoutedEventArgs e, object dataGridSender)
        {
            if (dataGridSender is DataGrid dataGrid1 && dataGrid1.SelectedItem is Client selectedClient)
            {
                int clientId = selectedClient.Id;

                var client = context.Clients.Find(clientId);
                if (client != null)
                {
                    string firstName = client.FirstName;
                    string middleName = client.MiddleName;
                    string lastName = client.LastName;

                    string combinedNames = $"{firstName} {middleName} {lastName}";
                    double currentCapital = client.CurrentCapital;

                    string currentUserFullName = menuItem3.Header.ToString();
                    string[] names = currentUserFullName.Split(' ');
                    if (names.Length >= 2)
                    {
                        string currentUserFirstName = names[0];
                        string currentUserLastName = names[1];
                        var selectedUser = context.Users.FirstOrDefault(user => user.FirstName == currentUserFirstName && user.LastName == currentUserLastName);
                        AddLoan window = new AddLoan(this);
                        window.currentUser = selectedUser.Id;
                        window.label8.Content = combinedNames;
                        window.textBox1.Text = currentCapital.ToString();
                        window.ShowDialog();
                    }
                }
            }
        }

        //private void AddWithdrawalOfACapital_Click(object sender, RoutedEventArgs e, object dataGridSender)
        //{
        //    //
        //}

        private void AddPayment_Click(object sender, RoutedEventArgs e, object dataGridSender)
        {
            if (dataGridSender is DataGrid dataGrid1 && dataGrid1.SelectedItem is Client selectedClient)
            {
                int clientId = selectedClient.Id;

                var client = context.Clients.Find(clientId);
                if (client != null)
                {
                    string firstName = client.FirstName;
                    string middleName = client.MiddleName;
                    string lastName = client.LastName;

                    string combinedNames = $"{firstName} {middleName} {lastName}";
                    var selectedLoan = context.Loans.FirstOrDefault(loans => loans.ClientId == client.Id);
                    if (selectedLoan != null)
                    {
                        var maxLoanNumber = context.Loans.Where(loan => loan.ClientId == clientId).Max(loan => (int?)loan.LoanNumber);

                        if (maxLoanNumber.HasValue)
                        {
                            var loanWithMaxNumber = context.Loans.FirstOrDefault(loan => loan.ClientId == clientId && loan.LoanNumber == maxLoanNumber.Value);

                            if (loanWithMaxNumber != null && loanWithMaxNumber.StatusId != 2)
                            {
                                double loanSum = loanWithMaxNumber.LoanSum;
                                int loanMonths = loanWithMaxNumber.LoanMonths;
                                string currentUserFullName = menuItem3.Header.ToString();
                                string[] names = currentUserFullName.Split(' ');
                                if (names.Length >= 2)
                                {
                                    string currentUserFirstName = names[0];
                                    string currentUserLastName = names[1];
                                    var selectedUser = context.Users.FirstOrDefault(user => user.FirstName == currentUserFirstName && user.LastName == currentUserLastName);
                                    AddPayment window = new AddPayment(this);
                                    window.currentUser = selectedUser.Id;
                                    window.label1.Content = combinedNames;
                                    window.textBox1.Text = loanSum.ToString();
                                    window.textBox2.Text = loanMonths.ToString();
                                    string formattedmonthlyFee = String.Format("{0:0.##}", loanSum / loanMonths);
                                    double monthlyFee = Math.Ceiling(double.Parse(formattedmonthlyFee));
                                    window.textBox4.Text = monthlyFee.ToString("N2");
                                    window.textBox4.IsEnabled = true;
                                    window.ShowDialog();
                                }
                            }
                            else
                            {
                                double loanSum = 0.00;
                                int loanMonths = 0;
                                string currentUserFullName = menuItem3.Header.ToString();
                                string[] names = currentUserFullName.Split(' ');
                                if (names.Length >= 2)
                                {
                                    string currentUserFirstName = names[0];
                                    string currentUserLastName = names[1];
                                    var selectedUser = context.Users.FirstOrDefault(user => user.FirstName == currentUserFirstName && user.LastName == currentUserLastName);
                                    AddPayment window = new AddPayment(this);
                                    window.currentUser = selectedUser.Id;
                                    window.label1.Content = combinedNames;
                                    window.textBox1.Text = loanSum.ToString();
                                    window.textBox4.IsEnabled = false;
                                    window.textBox2.Text = loanMonths.ToString();
                                    window.ShowDialog();
                                }
                            }
                        }
                    }
                    else
                    {
                        double loanSum = 0.00;
                        int loanMonths = 0;
                        string currentUserFullName = menuItem3.Header.ToString();
                        string[] names = currentUserFullName.Split(' ');
                        if (names.Length >= 2)
                        {
                            string currentUserFirstName = names[0];
                            string currentUserLastName = names[1];
                            var selectedUser = context.Users.FirstOrDefault(user => user.FirstName == currentUserFirstName && user.LastName == currentUserLastName);
                            AddPayment window = new AddPayment(this);
                            window.currentUser = selectedUser.Id;
                            window.label1.Content = combinedNames;
                            window.textBox1.Text = loanSum.ToString();
                            window.textBox4.IsEnabled = false;
                            window.textBox2.Text = loanMonths.ToString();
                            window.ShowDialog();
                        }

                    }


                }
            }
        }

        private void dataGrid3_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem addItem1 = new MenuItem { Header = "Изтриване на заем" };
                addItem1.Click += (menuSender, menuE) => DeleteLoan_Click(menuSender, menuE, sender);
                contextMenu.Items.Add(addItem1);

                DataGrid dataGrid = (DataGrid)sender;
                DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

                contextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void DeleteLoan_Click(object sender, RoutedEventArgs e, object dataGridSender)
        {
            if (dataGrid3.SelectedItem != null)
            {
                Loan selectedLoan = (Loan)dataGrid3.SelectedItem;
                MessageBoxResult result = MessageBox.Show($"Сигурни ли сте, че искате да изтриете този заем?", "Потвърждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var maxLoanNumber = context.Loans.Where(loan => loan.ClientId == selectedClient.Id).Max(loan => loan.LoanNumber);
                    if (selectedLoan.LoanNumber != maxLoanNumber)
                    {
                        MessageBox.Show($"Нямате право да триете този заем!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        loansCollection.Remove(selectedLoan);
                        var dbLoan = context.Loans.Find(selectedLoan.Id);
                        context.Loans.Remove(dbLoan);
                        context.SaveChanges();
                        dataGrid3.Items.Refresh();
                    }
                }
            }
        }

        private void dataGrid2_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                //context.SaveChanges();
                //LoadDataInDataGrid1();
                //LoadDataInDataGrid3new();
                ContextMenu contextMenu = new ContextMenu();
                MenuItem addItem1 = new MenuItem { Header = "Изтриване на вноска" };
                addItem1.Click += (menuSender, menuE) => DeletePayment_Click(menuSender, menuE, sender);
                contextMenu.Items.Add(addItem1);

                DataGrid dataGrid = (DataGrid)sender;
                DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

                contextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void DeletePayment_Click(object sender, RoutedEventArgs e, object dataGridSender)
        {
            if (dataGrid2.SelectedItem != null)
            {
                Payment selectedPayment = (Payment)dataGrid2.SelectedItem;
                MessageBoxResult result = MessageBox.Show($"Сигурни ли сте, че искате да изтриете тази вноска?", "Потвърждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var maxOperationNumber = context.Payments.Where(payment => payment.ClientId == selectedClient.Id).Max(payment => payment.OperationNumber);
                    if (selectedPayment.OperationNumber != maxOperationNumber)
                    {
                        MessageBox.Show($"Нямате право да триете тази вноска!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        var information = context.Payments.FirstOrDefault(payment => payment.ClientId == selectedClient.Id && payment.OperationNumber == maxOperationNumber);
                        if (information != null)
                        {
                            if (information.CapitalPay != 0 && information.LoanPay == 0)
                            {
                                var dbPayment = context.Payments.Find(selectedPayment.Id);
                                var selectedNewClient = context.Clients.FirstOrDefault(client => client.Id == selectedClient.Id);
                                if (selectedNewClient != null)
                                {
                                    var capitalPayBefore = selectedClient.CurrentCapital;
                                    var capitalPayToSubtract = information.CapitalPay;
                                    selectedNewClient.CurrentCapital = capitalPayBefore - capitalPayToSubtract;
                                    context.Payments.Remove(dbPayment);
                                    context.SaveChanges();
                                    LoadDataInDataGrid1();
                                    LoadDataInDataGrid2new();
                                }
                            }
                            else if (information.CapitalPay == 0 && information.LoanPay != 0)
                            {
                                var dbPayment = context.Payments.Find(selectedPayment.Id);
                                var selectedNewLoan = context.Loans.FirstOrDefault(loan => loan.ClientId == selectedClient.Id && loan.StatusId != 2);
                                if (selectedNewLoan != null)
                                {
                                    var loanPayToAdd = information.LoanPay;
                                    selectedNewLoan.LoanRemainder = selectedNewLoan.LoanRemainder + loanPayToAdd;
                                    context.Payments.Remove(dbPayment);
                                    context.SaveChanges();
                                    LoadDataInDataGrid3new();
                                    LoadDataInDataGrid2new();
                                }
                            }
                            else if (information.CapitalPay != 0 && information.LoanPay != 0)
                            {
                                var dbPayment = context.Payments.Find(selectedPayment.Id);
                                var selectedNewClient = context.Clients.FirstOrDefault(client => client.Id == selectedClient.Id);
                                var selectedNewLoan = context.Loans.FirstOrDefault(loan => loan.ClientId == selectedClient.Id && loan.StatusId != 2);
                                if (selectedNewClient != null && selectedNewLoan != null)
                                {
                                    var capitalPayBefore = selectedClient.CurrentCapital;
                                    var capitalPayToSubtract = information.CapitalPay;
                                    var loanPayToAdd = information.LoanPay;
                                    selectedNewClient.CurrentCapital = capitalPayBefore - capitalPayToSubtract;
                                    selectedNewLoan.LoanRemainder = selectedNewLoan.LoanRemainder + loanPayToAdd;
                                    context.Payments.Remove(dbPayment);
                                    context.SaveChanges();
                                    LoadDataInDataGrid1();
                                    LoadDataInDataGrid2new();
                                    LoadDataInDataGrid3new();
                                }
                            }

                        }
                    }
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            string selectedFullName = menuItem3.Header.ToString();

            string[] names = selectedFullName.Split(' ');
            if (names.Length >= 2)
            {
                string selectedFirstName = names[0];
                string selectedLastName = names[1];

                var selectedUser = context.Users.FirstOrDefault(user => user.FirstName == selectedFirstName && user.LastName == selectedLastName);

                if (selectedUser != null)
                {
                    UpdateUser window = new UpdateUser();
                    window.currentUser = selectedUser.Id;
                    //var positionId = selectedUser.PositionId;
                    var positionName = context.Positions.FirstOrDefault(position => position.Id == selectedUser.PositionId)?.PositionName;

                    window.textBox1.Text = selectedUser.Username;
                    //window.passwordBox1.Password = selectedUser.Pass;
                    //window.passwordBox2.Password = selectedUser.Salt;
                    //window.textBox2.Text = selectedUser.Email;
                    window.dateTimePicker1.SelectedDate = selectedUser.DateOfBirth;
                    window.textBox3.Text = selectedUser.PhoneNumber;
                    window.textBox4.Text = selectedUser.Address;
                    window.textBox5.Text = selectedUser.FirstName;
                    window.textBox6.Text = selectedUser.LastName;
                    window.comboBox1.Text = positionName;
                    window.SetSelectedPositionName(positionName);
                    window.ShowDialog();
                    selectedUser.Username = window.textBox1.Text;
                    selectedUser.Pass = window.passwordBox1.Password;
                    selectedUser.Salt = window.passwordBox2.Password;
                    selectedUser.Email = window.textBox2.Text;
                    selectedUser.DateOfBirth = Convert.ToDateTime(window.dateTimePicker1.Text);
                    selectedUser.PhoneNumber = window.textBox3.Text;
                    selectedUser.Address = window.textBox4.Text;
                    selectedUser.FirstName = window.textBox5.Text;
                    selectedUser.LastName = window.textBox6.Text;
                    string userFirstName1 = window.textBox5.Text;
                    string userLastName1 = window.textBox6.Text;
                    string combinedNames = $"{userFirstName1} {userLastName1}";
                    menuItem3.Header = combinedNames;
                    string selectedPositionName = window.comboBox1.Text;
                    var selectedPosition = context.Positions.FirstOrDefault(position => position.PositionName == selectedPositionName);
                    if (selectedPosition != null)
                    {
                        selectedUser.PositionId = selectedPosition.Id;
                        label3.Content = selectedPosition.PositionName;
                    }
                }
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (menuItem3.Header != null)
            {
                string selectedFullName = menuItem3.Header.ToString();
                string[] names = selectedFullName.Split(' ');
                if (names.Length >= 2)
                {
                    string selectedFirstName = names[0];
                    string selectedLastName = names[1];

                    var selectedUser = context.Users.FirstOrDefault(user => user.FirstName == selectedFirstName && user.LastName == selectedLastName);

                    MessageBoxResult result = MessageBox.Show($"Сигурни ли сте, че искате да изтриете този потребител \n '{selectedFullName}'?", "Потвърждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (selectedUser != null)
                        {
                            context.Users.Remove(selectedUser);
                            context.SaveChanges();
                            MainWindow window = new MainWindow();
                            this.Close();
                            window.Show();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали поделение от падащото меню!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedDivisionName = comboBox1.SelectedItem.ToString();
                var selectedDivision = context.Divisions.FirstOrDefault(division => division.DivisionName == selectedDivisionName);
                if (selectedDivision != null)
                {
                    string divisionName = selectedDivision.DivisionName;
                    double divisionInterest = selectedDivision.Interest;
                    int currentSelection = comboBox1.SelectedIndex;
                    UpdateDivision window = new UpdateDivision();
                    window.currentDivision = selectedDivision.Id;
                    window.textBox1.Text = divisionName;
                    window.textBox2.Text = divisionInterest.ToString();
                    window.ShowDialog();
                    comboBox1.Items[currentSelection] = selectedDivision.DivisionName = window.textBox1.Text;
                    selectedDivision.Interest = Convert.ToDouble(window.textBox2.Text);
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали поделение от падащото меню!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedDivisionName = comboBox1.SelectedItem.ToString();
                var selectedDivision = context.Divisions.FirstOrDefault(division => division.DivisionName == selectedDivisionName.ToString());
                MessageBoxResult result = MessageBox.Show($"Сигурни ли сте, че искате да изтриете поделение '{selectedDivisionName}'?", "Потвърждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    comboBox1.Items.Remove(selectedDivisionName);
                    if (selectedDivision != null)
                    {
                        context.Divisions.Remove(selectedDivision);
                        context.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали поделение от падащото меню!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }


        //private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(parent, i);
        //        if (child != null && child is T found)
        //        {
        //            return found;
        //        }
        //        else
        //        {
        //            T childOfChild = FindVisualChild<T>(child);
        //            if (childOfChild != null)
        //            {
        //                return childOfChild;
        //            }
        //        }
        //    }
        //    return null;
        //}

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            string path = "C:\\UNIVERSITY\\Report\\Report1.pdf";
            using (MemoryStream pdfStream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(path))
                {
                    using (PdfDocument pdfDoc = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdfDoc))
                        {
                            var client = context.Clients.Find(selectedClient.Id);
                            string firstName = client.FirstName;
                            string middleName = client.MiddleName;
                            string lastName = client.LastName;
                            string combinedNames = $"{firstName} {middleName} {lastName}";
                            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

                            Paragraph title1 = new Paragraph("С П Р А В К А\n").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(16);
                            title1.SetFont(font);
                            document.Add(title1);

                            Paragraph title2 = new Paragraph("за движението на паричните средства").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title2.SetFont(font);
                            document.Add(title2);

                            Paragraph title3 = new Paragraph("на " + combinedNames.ToString()).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title3.SetFont(font);
                            document.Add(title3);

                            Paragraph title4 = new Paragraph("от " + selectedClient.DivisionName.ToString()).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title4.SetFont(font);
                            document.Add(title4);

                            Paragraph title5 = new Paragraph("за целия период на членство във ВСК").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title5.SetFont(font);
                            document.Add(title5);

                            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 14, 14, 10, 13, 15, 10, 20 }));

                            table.SetFont(font);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("№"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Дата на вноска"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("За дялов капитал"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("За погасяване на заем"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Сума"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Дялов капитал"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Квитанция №"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Тип операция"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                            foreach (Payment payment in paymentsCollection)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(payment.OperationNumber.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(payment.PaymentDate.ToString("dd.MM.yyyy"))));
                                table.AddCell(new Cell().Add(new Paragraph(payment.CapitalPay.ToString("0.00 лв."))));
                                table.AddCell(new Cell().Add(new Paragraph(payment.LoanPay.ToString("0.00 лв."))));
                                table.AddCell(new Cell().Add(new Paragraph(payment.TotalPay.ToString("0.00 лв."))));
                                table.AddCell(new Cell().Add(new Paragraph(payment.PaymentCapital.ToString("0.00 лв."))));
                                table.AddCell(new Cell().Add(new Paragraph(payment.DocumentNumber.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(payment.TypeOperationName.ToString())));

                                table.StartNewRow();
                            }
                            document.Add(table);
                            document.Close();
                            Report1 window = new Report1();
                            window.pdfViewer1.Load(pdfStream);
                            window.ShowDialog();
                        }
                    }
                }
            }
        }

        //private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        //{
        //    string path = "C:\\UNIVERSITY\\Report\\Report1.pdf";
        //    using (MemoryStream pdfStream = new MemoryStream())
        //    {
        //        using (PdfWriter writer = new PdfWriter(path))
        //        {
        //            using (PdfDocument pdfDoc = new PdfDocument(writer))
        //            {
        //                using (Document document = new Document(pdfDoc))
        //                {
        //                    PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

        //                    Paragraph title1 = new Paragraph("С П Р А В К А\n").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(16);
        //                    title1.SetFont(font);
        //                    document.Add(title1);

        //                    Paragraph title2 = new Paragraph("за движението на средства на членовете на ВСК").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                    title2.SetFont(font);
        //                    document.Add(title2);

        //                    string clientsDivision = comboBox1.SelectedItem?.ToString();
        //                    if (clientsDivision != null)
        //                    {
        //                        Paragraph title3 = new Paragraph("във " + clientsDivision).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                        title3.SetFont(font);
        //                        document.Add(title3);

        //                        Table table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 34, 14, 10, 13, 15 }));

        //                        table.SetFont(font);
        //                        table.AddHeaderCell(new Cell().Add(new Paragraph("№"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                        table.AddHeaderCell(new Cell().Add(new Paragraph("Име, презиме и фамилия"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                        table.AddHeaderCell(new Cell().Add(new Paragraph("Текущ капитал"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                        table.AddHeaderCell(new Cell().Add(new Paragraph("За дялов капитал"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                        table.AddHeaderCell(new Cell().Add(new Paragraph("За погасяване на заем"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                        table.AddHeaderCell(new Cell().Add(new Paragraph("Сума"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);


        //                        //int number = 1; 
        //                        foreach (Client client in clientsCollection)
        //                        {
        //                            string combinedNames = $"{client.FirstName} {client.MiddleName} {client.LastName}";

        //                            //var newDivision = context.Divisions.FirstOrDefault(division => division.DivisionName == comboBox1.Text);
        //                            var maxNumber = paymentsCollection.Where(payment => payment.ClientId == client.Id).Max(payment => payment.OperationNumber);
        //                            var maxOperationPayment = paymentsCollection.Where(payment => payment.ClientId == client.Id && payment.OperationNumber == maxNumber).OrderByDescending(payment => payment.OperationNumber).FirstOrDefault();                             
        //                            if (maxOperationPayment != null && client.DivisionName == clientsDivision)
        //                            {
        //                                table.AddCell(new Cell().Add(new Paragraph(maxOperationPayment.ToString())));
        //                                table.AddCell(new Cell().Add(new Paragraph(combinedNames.ToString())));
        //                                table.AddCell(new Cell().Add(new Paragraph(maxOperationPayment.PaymentCapital.ToString("0.00 лв."))));
        //                                table.AddCell(new Cell().Add(new Paragraph(maxOperationPayment.CapitalPay.ToString("0.00 лв."))));
        //                                table.AddCell(new Cell().Add(new Paragraph(maxOperationPayment.LoanPay.ToString("0.00 лв."))));
        //                                table.AddCell(new Cell().Add(new Paragraph(maxOperationPayment.TotalPay.ToString("0.00 лв."))));

        //                                table.StartNewRow();
        //                                //number++;
        //                            }


        //                        }
        //                        document.Add(table);
        //                        document.Close();
        //                        Report1 window = new Report1();
        //                        window.pdfViewer1.Load(pdfStream);
        //                        window.ShowDialog();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show($"Не сте избрали поделение от падащото меню!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            string path = "C:\\UNIVERSITY\\Report\\Report1.pdf";
            using (MemoryStream pdfStream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(path))
                {
                    using (PdfDocument pdfDoc = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdfDoc))
                        {
                            var client = context.Clients.Find(selectedClient.Id);
                            string firstName = client.FirstName;
                            string middleName = client.MiddleName;
                            string lastName = client.LastName;
                            string combinedNames = $"{firstName} {middleName} {lastName}";
                            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

                            Paragraph title1 = new Paragraph("С П Р А В К А\n").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(16);
                            title1.SetFont(font);
                            document.Add(title1);

                            Paragraph title2 = new Paragraph("за теглени заеми").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title2.SetFont(font);
                            document.Add(title2);

                            Paragraph title3 = new Paragraph("на " + combinedNames.ToString()).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title3.SetFont(font);
                            document.Add(title3);

                            Paragraph title4 = new Paragraph("от " + selectedClient.DivisionName.ToString()).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title4.SetFont(font);
                            document.Add(title4);

                            Paragraph title5 = new Paragraph("за целия период на членство във ВСК").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            title5.SetFont(font);
                            document.Add(title5);

                            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 6, 20, 20, 15, 18, 20 }));

                            table.SetFont(font);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("№"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Дата на теглене"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Изтеглена сума"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Месеци"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Оставаща сума"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Статус"))).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                            foreach (Loan loan in loansCollection)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(loan.LoanNumber.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(loan.LoanDate.ToString("dd.MM.yyyy"))));
                                table.AddCell(new Cell().Add(new Paragraph(loan.LoanSum.ToString("0.00 лв."))));
                                table.AddCell(new Cell().Add(new Paragraph(loan.LoanMonths.ToString(""))));
                                table.AddCell(new Cell().Add(new Paragraph(loan.LoanRemainder.ToString("0.00 лв."))));
                                table.AddCell(new Cell().Add(new Paragraph(loan.LoanStatusName.ToString())));

                                table.StartNewRow();
                            }
                            document.Add(table);
                            document.Close();
                            Report1 window = new Report1();
                            window.pdfViewer1.Load(pdfStream);
                            window.ShowDialog();
                        }
                    }
                }
            }
        }
    }
}
