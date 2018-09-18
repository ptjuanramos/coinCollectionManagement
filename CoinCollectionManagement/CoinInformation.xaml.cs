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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoinCollectionManagement {
    /// <summary>
    /// Interaction logic for CoinInformation.xaml
    /// </summary>
    public partial class CoinInformation : Window {

        public CoinInformation() {
            InitializeComponent();
            GetInformation();
        }

        private void GetInformation() {
            Coin currentCoin = SessionContext.CurrentCoin;
            facialValue.Text = currentCoin.facialValue;
            marketValue.Text = currentCoin.marketValue;
            countryValue.Text = currentCoin.country;
            dateValue.Text = currentCoin.date;

            this.Topmost = true; //bring window to upfront
        }

        public void DeleteCoinButtonHandler(object sender, RoutedEventArgs args) {

        }

        public void UpdateCoinButtonHandle(object sender, RoutedEventArgs args) {

        }
    }
}
