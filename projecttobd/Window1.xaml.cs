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

namespace projecttobd
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        TextBox[] MandatoryData;
        ComboBoxItem[] AddressMail;
        string newMail;

        char[] SpecialSymbols = { '@', '#', '=', '-', '_', '+', '=', ')', '(', '*', '&', '^', '%', '$', '!', '[', ']', '{', '}', '>', '<', '?', ':', ';', '/', '.', ',', '"', };
        bool presenceOfSpecialSymbols;
        public Window1()
        {
            InitializeComponent();
        }

        private void windowavto_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow win = new MainWindow();
            win.Show();
        }

        private void zareg_Click(object sender, RoutedEventArgs e)
        {
            var loginn = login.Text;
            var passs = pass.Text;
            var passsagain = passagain.Text;
            var email = mail.Text;
            var indexx = combobxx.Text;

            var context = new AppDbContext();

            MandatoryData = new TextBox[] { login, pass, mail, passagain };
            AddressMail = new ComboBoxItem[] { MailRU, GoogleMail, InboxMail, YandexMail};
            presenceOfSpecialSymbols = false;

            for (int i = 0; i < AddressMail.Length; i++)
            {
                if (AddressMail[i].IsSelected)
                {
                    newMail = mail.Text + AddressMail[i].Content;
                }
            }

            for (int i = 0; i < MandatoryData.Length; i++)
            {
                if (MandatoryData[i].Text == "")
                {
                    MandatoryData[i].FontSize = 15;
                    MandatoryData[i].Text = "Обязательное поле для заполнения";
                    MandatoryData[i].BorderBrush = Brushes.DarkRed;
                    if (i == MandatoryData.Length - 1)
                        return;
                }
            }


            for (int i = 0; i < pass.Text.Length; i++)
            {
                for (int j = 0; j < SpecialSymbols.Length; j++)
                {
                    if (pass.Text[i] == SpecialSymbols[j])
                        presenceOfSpecialSymbols = true;
                }

            }

            if (pass.Text.Length <= 8 || !presenceOfSpecialSymbols)
            {
                pass.FontSize = 15;
                passagain.FontSize = 15;
                pass.Text = "Пароль должен содержать специальные символы и быть больше 8 символов";
                pass.BorderBrush = Brushes.DarkRed;
                passagain.Text = "Пароль должен содержать специальные символы и быть больше 8 символов";
                passagain.BorderBrush = Brushes.DarkRed;
                return;
            }

            if (pass.Text != passagain.Text)
            {
                passagain.FontSize = 15;
                passagain.Text = "Пароли должны совпадать";
                passagain.BorderBrush = Brushes.DarkRed;
                return;
            }
            var user_exists = context.Users.FirstOrDefault(x => x.Login == loginn);
            if (user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже есть");
                return;
            }
            var user = new User { Login = loginn, Password=passs, Email=email+indexx };
            context.Users.Add(user);
            context.SaveChanges();

            MessageBox.Show("Вы успешно зарегистрировались");
           
        }
        private void GotFocusLogin(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Обязательное поле для заполнения")
            {
                textBox.FontSize = 20;
                textBox.Text = "";
                textBox.BorderBrush = Brushes.Black;
            }
        }
        private void GotFocusPass(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Обязательное поле для заполнения"
                || textBox.Text == "Пароль должен содержать специальные символы и быть больше 8 символов")
            {
                textBox.FontSize = 20;
                textBox.Text = "";
                textBox.BorderBrush = Brushes.Black;
            }
        }
        private void GotFocusPassagain(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Обязательное поле для заполнения"
                || textBox.Text == "Пароли должны совпадать"
                || textBox.Text == "Пароль должен содержать специальные символы и быть больше 8 символов")
            {
                textBox.FontSize = 20;
                textBox.Text = "";
                textBox.BorderBrush = Brushes.Black;
            }
        }
        private void GotFocusMail(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Обязательное поле для заполнения")
            {
                textBox.FontSize = 20;
                textBox.Text = "";
                textBox.BorderBrush = Brushes.Black;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passtext.Visibility == Visibility.Visible)
            {
                passtext.Visibility = Visibility.Hidden;
                pass.Text = passtext.Password;
                pass.Visibility = Visibility.Visible;

                passagaintext.Visibility = Visibility.Hidden;
                passagain.Text = passagaintext.Password;
                passagain.Visibility = Visibility.Visible;
            }
            else
            {
                pass.Visibility = Visibility.Hidden;
                passtext.Visibility = Visibility.Visible;

                passagain.Visibility = Visibility.Hidden;
                passagaintext.Visibility = Visibility.Visible;
            }
        }
    }
}
