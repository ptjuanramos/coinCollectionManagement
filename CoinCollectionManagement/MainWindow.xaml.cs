using CoinCollectionManagement.Admin;
using CoinCollectionManagement.DAO;
using CoinCollectionManagement.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoinCollectionManagement {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public string WelcomeMessageBind {
            get { return "Bem vindo " + SessionContext.CurrentUser.name;  }
        }

        public MainWindow() {
            InitializeComponent();
            DataContext = this;

            if(SessionContext.CurrentUser.userRole.Equals(DataSource.ADMIN_ROLE)) {
                addUserButton.Visibility = Visibility.Visible;
            }

            FillCoinCollectionCategoryList();
            FillCoinsListSession();

            //list listeners
            SessionContext.listOfCoins.CollectionChanged += ListOfCoins_CollectionChanged;
            SessionContext.CollectionCategories.CollectionChanged += ListOfCoins_CategoryChanged;
        }

        private void FillCoinCollectionCategoryList() {
            CoinCategoryDao coinCategoryDao = new CoinCategoryDao();
            ObservableCollection<CoinCategory> coinCategoriesObservableCollection 
                = new ObservableCollection<CoinCategory>(coinCategoryDao.GetAll(SessionContext.CurrentUser.id));
            SessionContext.CollectionCategories = coinCategoriesObservableCollection;
            categoryListView.ItemsSource = coinCategoriesObservableCollection.ToList<CoinCategory>();
        }

        private void ListOfCoins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if(e.Action == NotifyCollectionChangedAction.Add) {
                if(SessionContext.listOfCoins.Count != 0 && e.NewItems != null) {
                    List<Coin> listViewCurrentItems = (List<Coin>) coinsListView.ItemsSource;

                    foreach(Object newCoin in e.NewItems) {
                        listViewCurrentItems.Add((Coin) newCoin);
                    }       
                }
            } else if(e.Action == NotifyCollectionChangedAction.Remove) {
                List<Coin> listViewCurrentItems = new List<Coin>(); 
                foreach(Object oldCoin in e.NewItems) {
                    listViewCurrentItems.Add((Coin)oldCoin);
                }

                coinsListView.ItemsSource = listViewCurrentItems;
            }

            coinsListView.Items.Refresh();
        }

        private void ListOfCoins_CategoryChanged(object sender, NotifyCollectionChangedEventArgs e) {

            if(e.Action == NotifyCollectionChangedAction.Add) {

                categoryListView.ItemsSource = SessionContext.CollectionCategories;
            } else if(e.Action == NotifyCollectionChangedAction.Remove) {
                categoryListView.ItemsSource = SessionContext.CollectionCategories; 
            }

            categoryListView.Items.Refresh();
        }

        private Boolean clickCoinItemWindowOpen = false;

        private void DoubleClickCoinItemHandler(object sender, RoutedEventArgs args) {
            if (!clickCoinItemWindowOpen) {
                ListViewItem item = (sender as ListViewItem);
                Coin selectedItem = (Coin) item.Content;

                SessionContext.CurrentCoin = selectedItem;
                CoinInformation coinInformationWindow = new CoinInformation();
                coinInformationWindow.Closing += CoinItemClosing;

                coinInformationWindow.Show();
                clickCoinItemWindowOpen = true;
            }
        }

        private void OneClickCoinCategoryItemHandler(object sender, RoutedEventArgs args) {
            ListViewItem item = (sender as ListViewItem);
            CoinCategory selectedItem = (CoinCategory)item.Content;

            coinsListView.ItemsSource = selectedItem.coinsList;
        }

        private void CoinItemClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            clickCoinItemWindowOpen = false;
        }

        private void AddMultiCollection(object sender, RoutedEventArgs e) {

            AddCollection addCollectionWindow = new AddCollection();
            addCollectionWindow.Show();
        }

        private void AddUniqueCoin(object sender, RoutedEventArgs e) {
            AddUniqueCoin addUniqueCoin = new AddUniqueCoin();
            addUniqueCoin.Show();
        }

        private void AddUserHandler(object sender, RoutedEventArgs args) {
            if(SessionContext.CurrentUser.userRole.Equals(DataSource.ADMIN_ROLE)) {
                AddUser addUserWindow = new AddUser();
                addUserWindow.Show();
            }
        }

        private void LogoutHandler(object sender, RoutedEventArgs args) {
            SessionContext.ResetSession();
            Login loginWindow = new Login();
            App.Current.MainWindow = loginWindow;
            this.Close();

            loginWindow.Show();
        }

        //Fills the coin from the database and the association of the category
        private void FillCoinsListSession() {
            CoinDao coinDao = new CoinDao();
            List<Coin> coins = coinDao.GetAll(SessionContext.CurrentUser.username);
            coins.ToList().ForEach(SessionContext.listOfCoins.Add);
            coinsListView.ItemsSource = coins;
        }

        private void SearchInputFocusChange(object sender, RoutedEventArgs args) {
            SearchInputBox.Text = string.Empty;
            SearchInputBox.GotFocus -= SearchInputFocusChange;
        }
    }
}
