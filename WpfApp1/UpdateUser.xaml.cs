using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Window
    {
        public UpdateUser()
        {
            InitializeComponent();
        }

        public int currentUser;

        private string selectedPositionName;

        private readonly ModelContext context = new ModelContext();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewPositionInComboBox(selectedPositionName);
        }
        public void SetSelectedPositionName(string positionName)
        {
            selectedPositionName = positionName;
        }

        void viewPositionInComboBox(string selectedPositionName)
        {
            foreach (var item in context.Positions.ToList())
            {
                comboBox1.Items.Add(item.PositionName);
            }
            comboBox1.SelectedItem = selectedPositionName;
        }

        private bool SearchForDuplicateThenUpdate()
        {
            string username = textBox1.Text;
            var thisUser = context.Users.FirstOrDefault(user => user.Username == username);
            if (thisUser == null)
            {
                return true;
            }
            Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);
            if (existingUser.Username == thisUser.Username)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(dateTimePicker1.Text) || string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(comboBox1.Text) ||
                textBox1.Foreground != Brushes.Green || passwordBox3.Foreground == Brushes.Red || passwordBox1.Foreground == Brushes.Red ||
                passwordBox2.Foreground == Brushes.Red || textBox2.Foreground == Brushes.Red ||
                dateTimePicker1.Foreground != Brushes.Green || textBox3.Foreground != Brushes.Green ||
                textBox4.Foreground != Brushes.Green || textBox5.Foreground != Brushes.Green ||
                textBox6.Foreground != Brushes.Green || comboBox1.Foreground != Brushes.Green)
            {
                MessageBox.Show("Всички полета трябва да са зелени и попълнени!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (SearchForDuplicateThenUpdate())
                {
                    string newUserUsername = textBox1.Text;
                    string oldUserPass = passwordBox3.Password.ToString();
                    string newUserPass1 = passwordBox1.Password.ToString();
                    string newUserPass2 = passwordBox2.Password.ToString();
                    string newUserEmail = textBox2.Text;
                    DateTime newUserDateOfBirth = Convert.ToDateTime(dateTimePicker1.Text);
                    string newUserPhoneNumber = textBox3.Text;
                    string newUserAddress = textBox4.Text;
                    string newUserFirstName = textBox5.Text;
                    string newUserLastName = textBox6.Text;
                    string newUserPositionName = comboBox1.Text;
                    var newPosition = context.Positions.FirstOrDefault(position => position.PositionName == comboBox1.Text);
                    Users existingUser = context.Users.FirstOrDefault(user => user.Id == currentUser);

                    string storedHashedPassword = existingUser.Pass;
                    var sensitivedata = new SensitiveDataEncryption();
                    byte[] passwordSalt;
                    byte[] emailSalt;
                    byte[] storedSalt = Convert.FromBase64String(existingUser.Salt);
                    bool passwordIsCorrect = sensitivedata.VerifySensitiveData(oldUserPass, storedHashedPassword, storedSalt);
                    if ((existingUser != null) && (passwordIsCorrect) && (newUserPass1 == newUserPass2))
                    {
                        if (!string.IsNullOrEmpty(newUserUsername))
                        {
                            existingUser.Username = newUserUsername;
                        }
                        string hashedPassword = sensitivedata.HashSensitiveData(newUserPass1, out passwordSalt);
                        existingUser.Pass = hashedPassword;
                        existingUser.Salt = Convert.ToBase64String(passwordSalt);
                        string hashedEmail = sensitivedata.HashSensitiveData(textBox2.Text, out emailSalt);
                        existingUser.Email = hashedEmail;
                        
                        if (newUserDateOfBirth != existingUser.DateOfBirth)
                        {
                            existingUser.DateOfBirth = Convert.ToDateTime(newUserDateOfBirth);
                        }
                        if (!string.IsNullOrEmpty(newUserPhoneNumber))
                        {
                            existingUser.PhoneNumber = newUserPhoneNumber;
                        }
                        if (!string.IsNullOrEmpty(newUserAddress))
                        {
                            existingUser.Address = newUserAddress;
                        }
                        if (!string.IsNullOrEmpty(newUserFirstName))
                        {
                            existingUser.FirstName = newUserFirstName;
                        }
                        if (!string.IsNullOrEmpty(newUserLastName))
                        {
                            existingUser.LastName = newUserLastName;
                        }
                        if (!string.IsNullOrEmpty(newUserPositionName))
                        {
                            existingUser.PositionId = newPosition.Id;
                        }
                        context.SaveChanges();
                        MessageBox.Show("Потребителят е редактиран успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else if (existingUser != null)
                    {
                        if (!string.IsNullOrEmpty(newUserUsername))
                        {
                            existingUser.Username = newUserUsername;
                        }

                        string hashedEmail = sensitivedata.HashSensitiveData(textBox2.Text, out emailSalt);
                        existingUser.Email = hashedEmail;

                        if (newUserDateOfBirth != existingUser.DateOfBirth)
                        {
                            existingUser.DateOfBirth = Convert.ToDateTime(newUserDateOfBirth);
                        }
                        if (!string.IsNullOrEmpty(newUserPhoneNumber))
                        {
                            existingUser.PhoneNumber = newUserPhoneNumber;
                        }
                        if (!string.IsNullOrEmpty(newUserAddress))
                        {
                            existingUser.Address = newUserAddress;
                        }
                        if (!string.IsNullOrEmpty(newUserFirstName))
                        {
                            existingUser.FirstName = newUserFirstName;
                        }
                        if (!string.IsNullOrEmpty(newUserLastName))
                        {
                            existingUser.LastName = newUserLastName;
                        }
                        if (!string.IsNullOrEmpty(newUserPositionName))
                        {
                            existingUser.PositionId = newPosition.Id;
                        }
                        context.SaveChanges();
                        MessageBox.Show("Потребителят е редактиран успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Потребителят не е намерен!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Това потребителско име вече се използва!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    textBox1.Foreground = Brushes.Red;
                }
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$"))
            {
                textBox1.Foreground = Brushes.Green;
            }
            else
            {
                textBox1.Foreground = Brushes.Red;
            }
        }
        
        private void passwordBox3_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(passwordBox3.Password.ToString(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$&])[a-zA-Z!@#$&]{8}$"))
            {
                passwordBox3.Foreground = Brushes.Green;
            }
            else
            {
                passwordBox3.Foreground = Brushes.Red;
            }
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(passwordBox1.Password.ToString(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$&])[a-zA-Z!@#$&]{8}$"))
            {
                passwordBox1.Foreground = Brushes.Green;
            }
            else
            {
                passwordBox1.Foreground = Brushes.Red;
            }
        }

        private void passwordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBox2.Password.ToString() == passwordBox1.Password.ToString())
            {
                passwordBox2.Foreground = Brushes.Green;
            }
            else
            {
                passwordBox2.Foreground = Brushes.Red;
            }
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(textBox2.Text, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            {
                textBox2.Foreground = Brushes.Green;
            }
            else
            {
                textBox2.Foreground = Brushes.Red;
            }
        }

        private void dateTimePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateTimePicker1.Foreground = Brushes.Green;
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(textBox3.Text, "^[0]{1}[8]{1}[0-9]{8}"))
            {
                textBox3.Foreground = Brushes.Green;
            }
            else
            {
                textBox3.Foreground = Brushes.Red;
            }
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(textBox4.Text, "[А-Яа-я]"))
            {
                textBox4.Foreground = Brushes.Green;
            }
            else
            {
                textBox4.Foreground = Brushes.Red;
            }
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(textBox5.Text, "^[А-Я]{1}[а-я]"))
            {
                textBox5.Foreground = Brushes.Green;
            }
            else
            {
                textBox5.Foreground = Brushes.Red;
            }
        }

        private void textBox6_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(textBox6.Text, "^[А-Я]{1}[а-я]"))
            {
                textBox6.Foreground = Brushes.Green;
            }
            else
            {
                textBox6.Foreground = Brushes.Red;
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox1.Foreground = Brushes.Green;
        }
        
    }
}
