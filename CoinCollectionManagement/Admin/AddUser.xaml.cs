using CoinCollectionManagement.DAO;
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

namespace CoinCollectionManagement.Admin {
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window {
        private readonly string USER_ADMIN_PATH = "/images/admin_icon.png";
        private readonly string USER_REGULAR_PATH = "/images/regular_icon.png";

        public AddUser() {
            InitializeComponent();
        }

        public void ChangeImageRole(object e, RoutedEventArgs args) {
            RadioButton roleIconRadioButton = (RadioButton) e;

            if(roleIconRadioButton.Name.Equals("adminRoleButton")) {
                userImageRole.Source = new BitmapImage(new Uri(USER_ADMIN_PATH, UriKind.Relative));
            } else {
                userImageRole.Source = new BitmapImage(new Uri(USER_REGULAR_PATH, UriKind.Relative));
            }
        }

        public void AddUserButtonHandler(object e, RoutedEventArgs args) {
            UserDao userDao = new UserDao();
            User currentUser = userDao.GetByUsername(usernameInput.Text);

            if(currentUser.username != null) {
                MessageBox.Show("Username já existente...");
            } else if(AreFieldsValid()) {
                User newUser = new User();
                newUser.name = nameInput.Text;
                newUser.username = usernameInput.Text;
                newUser.password = passwordInput.Password;
                string userRole = adminRoleButton.IsChecked == true ? "ADMIN" : "REGULAR";
                newUser.userRole = userRole;
                User insertedUser = userDao.Insert(newUser);

                if (insertedUser.id != null) {
                    MessageBox.Show("Utilizador foi adicionado...");
                    this.Close();
                } else {
                    MessageBox.Show("Algum problema aconteceu ao adicionar o utilizador...");
                }

            } else {
                MessageBox.Show("Preencha todos os campos...");
            }
        }

        private Boolean AreFieldsValid() {
            string name = nameInput.Text;
            string username = usernameInput.Text;
            string password = passwordInput.Password;
  
            if (name == null || username == null || password == null)
                return false;

            return true;
        }

        public void CloseAddUserWindow(object e, RoutedEventArgs args) {

        }
    }
}
