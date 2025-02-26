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
using System.Data.OracleClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using WpfApp1.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MahApps.Metro.Controls.Dialogs;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Sign_up.xaml
    /// </summary>
    public partial class Sign_up : Window
    {
        private readonly ModelContext context = new ModelContext();

        public Sign_up()
        {
            InitializeComponent();
        }
        
        private void Window_Load(object sender, RoutedEventArgs e)
        {
            viewPositionInComboBox();
        }

        void viewPositionInComboBox()
        {
            //linq query и EntityFramework преобразува заявките от базата в linq заявки
            foreach (var item in context.Positions.ToList())
            {
                comboBox1.Items.Add(item.PositionName);
            }
        }

        private bool SearchForDuplicateThenInsert()
        {
            string username = textBox1.Text;
            var duplicateUser = context.Users.FirstOrDefault(user => user.Username == username);
            if (duplicateUser != null)
            {
                //this username is already taken! 
                return false;
            }
            else
            {
                //this username is free
                return true;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(passwordBox1.Password) ||
                string.IsNullOrWhiteSpace(passwordBox2.Password) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(dateTimePicker1.Text) || string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(comboBox1.Text) ||
                textBox1.Foreground != Brushes.Green || passwordBox1.Foreground != Brushes.Green ||
                passwordBox2.Foreground != Brushes.Green || textBox2.Foreground != Brushes.Green ||
                dateTimePicker1.Foreground != Brushes.Green || textBox3.Foreground != Brushes.Green ||
                textBox4.Foreground != Brushes.Green || textBox5.Foreground != Brushes.Green ||
                textBox6.Foreground != Brushes.Green || comboBox1.Foreground != Brushes.Green)
            {
                MessageBox.Show("Всички полета трябва да са зелени и попълнени!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (SearchForDuplicateThenInsert())
                {
                    var newPosition = context.Positions.FirstOrDefault(position => position.PositionName == comboBox1.Text);

                    var sensitivedata = new SensitiveDataEncryption();
                    byte[] passwordSalt;
                    byte[] emailSalt;
                    string password1 = passwordBox1.Password;
                    string password2 = passwordBox2.Password;
                    if (password1 == password2)
                    {
                        string hashedPassword = sensitivedata.HashSensitiveData(password1, out passwordSalt);
                        string hashedEmail = sensitivedata.HashSensitiveData(textBox2.Text, out emailSalt);

                        string clientUsername = textBox1.Text;
                        string clientPositionName = comboBox1.Text;
                        if (clientPositionName != "админ")
                        {
                            var newUser = new Users
                            {
                                Username = textBox1.Text,
                                Pass = hashedPassword,
                                Salt = Convert.ToBase64String(passwordSalt),
                                Email = hashedEmail,
                                DateOfBirth = dateTimePicker1.SelectedDate.Value,
                                PhoneNumber = textBox3.Text,
                                Address = textBox4.Text,
                                FirstName = textBox5.Text,
                                LastName = textBox6.Text,
                                PositionId = newPosition.Id
                            };
                            context.Users.Add(newUser);
                            context.SaveChanges();
                            MessageBox.Show("Успешна регистрация.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            Window window = new Log_in();
                            this.Close();
                            window.ShowDialog();
                        }
                        else if (clientUsername == "darinadragneva" && clientPositionName == "админ")
                        {
                            var newUser = new Users
                            {
                                Username = textBox1.Text,
                                Pass = hashedPassword,
                                Salt = Convert.ToBase64String(passwordSalt),
                                Email = hashedEmail,
                                DateOfBirth = dateTimePicker1.SelectedDate.Value,
                                PhoneNumber = textBox3.Text,
                                Address = textBox4.Text,
                                FirstName = textBox5.Text,
                                LastName = textBox6.Text,
                                PositionId = newPosition.Id
                            };
                            context.Users.Add(newUser);
                            context.SaveChanges();
                            MessageBox.Show("Успешна регистрация.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            Window window = new Log_in();
                            this.Close();
                            window.ShowDialog();
                        }
                        
                        else if (clientPositionName == "админ")
                        {
                            MessageBox.Show("Нямате права за създаване на профил като админ!.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Това потребителско име вече се използва!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    textBox1.Foreground = Brushes.Red;
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            this.Close();
            window.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "^(?=.{8,16}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$"))
            {
                textBox1.Foreground = Brushes.Green;
            }
            else
            {
                textBox1.Foreground = Brushes.Red;
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