using CoinCollectionManagement.DAO;
using CoinCollectionManagement.Internal;
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
using System.Data.SqlClient;

namespace CoinCollectionManagement {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window {
        public Login() {
            InitializeComponent();
        }

        public void AttemptLogin(object sender, RoutedEventArgs routedEventArgs) {
            try {
                if (IsUserNameCorrect() && IsPasswordCorrect()) {
                    UserDao userDao = new UserDao();
                    SessionContext.CurrentUser = userDao.GetByUsername(usernameField.Text);

                    MainWindow mainWindow = new MainWindow();
                    App.Current.MainWindow = mainWindow;
                    this.Close();
                    mainWindow.Show();
                }
                else {
                    MessageBox.Show(Application.Current.MainWindow, "O Username ou a Password não está correcto(a)",
                        "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
                }
            } catch(SqlException ex) {
                MessageBox.Show(Application.Current.MainWindow, "Não foi possível fazer a conexão com a base de dados.",
                        "Database error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
            } 
        }

        private Boolean IsUserNameCorrect() {
            TextBox usernameTextBox = usernameField;
            UserDao userValidationDao = new UserDao();

            if(usernameTextBox.Text.Equals(StaticLoginInfo.Username)) {
                return true;
            } else if(userValidationDao.GetByUsername(usernameTextBox.Text).username != null) {
                return true;
            }

            return false;
        }

        private Boolean IsPasswordCorrect() {
            PasswordBox passwordBox = passwordField;
            string username = usernameField.Text;
            UserDao userValidationDao = new UserDao();

            if(passwordBox.Password.ToString().Equals(StaticLoginInfo.Password)) {
                return true;
            } else if(userValidationDao.IsPasswordCorrect(username, passwordBox.Password.ToString())) {
                return true;
            }

            return false;
        }
    }
}
