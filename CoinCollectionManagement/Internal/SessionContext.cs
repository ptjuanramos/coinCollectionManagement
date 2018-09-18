using CoinCollectionManagement.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinCollectionManagement.Internal {
    public static class SessionContext {
        public static Coin CurrentCoin;
        public static User CurrentUser;

        public static ObservableCollection<Coin> listOfCoins = new ObservableCollection<Coin>();
        public static ObservableCollection<CoinCategory> CollectionCategories = new ObservableCollection<CoinCategory>();

        public static void ResetSession() {
            CurrentCoin = null;
            CurrentUser = null;
            listOfCoins = new ObservableCollection<Coin>();
            CollectionCategories = new ObservableCollection<CoinCategory>();
        }
    }
}