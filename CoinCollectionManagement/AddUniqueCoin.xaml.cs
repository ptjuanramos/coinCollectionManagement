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

namespace CoinCollectionManagement {
    /// <summary>
    /// Interaction logic for AddUniqueCoin.xaml
    /// </summary>
    public partial class AddUniqueCoin : Window {
        private readonly string URI_COIN_IMAGE = "/images/coin_icon.png";
        private readonly string URI_BILL_IMAGE = "/images/bill_icon.jpg";

        private readonly string TYPE_COIN = "Moeda";
        private readonly string TYPE_BILL = "Nota";

        public AddUniqueCoin() {
            InitializeComponent();
            FillCategoryList();
        } 

        private void FillCategoryList() {
            comboCategoryList.ItemsSource = SessionContext.CollectionCategories;
        }

        public void ChangeIconToCoin(object sender, EventArgs e) {    
            Image collectionTypeImage = collectionTypeIcon;
            collectionTypeImage.Source = new BitmapImage(new Uri(URI_COIN_IMAGE, UriKind.Relative));
        }

        public void ChangeIconToBill(object sender, EventArgs e) {
            Image collectionTypeImage = collectionTypeIcon;
            collectionTypeImage.Source = new BitmapImage(new Uri(URI_BILL_IMAGE, UriKind.Relative));
        }

        public void AddCoinButtonHandle(object sender, EventArgs e) {
            Boolean validFields = true;
            Boolean newCategory = false;
            validFields = validFields && ValidateField(description.Text);
            validFields = validFields && ValidateField(facialValue.Text);
            validFields = validFields && ValidateField(marketValue.Text);
            validFields = validFields && ValidateField(countryValue.Text);
            validFields = validFields && ValidateField(dateValue.Text);

            if(!validFields) {
                MessageBox.Show("Preencha todos os campos", "Falta de informação",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            string type = "";

            if(coinType.IsChecked == true) {
                type = TYPE_COIN;
            } else if(billType.IsChecked == true) {
                type = TYPE_BILL;
            } else {
                MessageBox.Show("Escolha o tipo de moeda", "Falta de informação", 
                    MessageBoxButton.OK, MessageBoxImage.Information);       
            }

            int collectionCount = 1;

            try {
                collectionCount = Int32.Parse(collectionCounter.Text);

            } catch (FormatException ex) {
                if (!collectionCounter.Text.Equals(String.Empty)) {
                    validFields = false;
                    MessageBox.Show("No campo 'número de moedas', tem de inserir um valor numérico.");
                }
            } catch (OverflowException  ex) {
                validFields = false;
                MessageBox.Show("O valor de 'número de moedas' é muito alto");
            }

            if((newCollectionCategoryField == null || newCollectionCategoryField.Text.Equals(String.Empty))
                && (comboCategoryList.SelectedItem == null || comboCategoryList.SelectedItem.ToString().Equals(String.Empty))) {
                validFields = false;
                MessageBox.Show("Selecione uma categoria ou criar uma nova");
            } else if(newCollectionCategoryField != null && !newCollectionCategoryField.Text.Equals(String.Empty)
                && comboCategoryList.SelectedItem != null && comboCategoryList.SelectedItem.ToString().Equals(String.Empty)) {
                validFields = false;
                MessageBox.Show("Não pode adicionar uma nova categoria e escolher uma antiga");
            }

            if(validFields) {
                //Create a new coin flow
                CoinCategory coinCategory = null;

                if(newCollectionCategoryField != null && !newCollectionCategoryField.Text.Equals(String.Empty)) {
                    coinCategory = createNewCategory(newCollectionCategoryField.Text);

                    if(coinCategory == null) {
                        MessageBox.Show("Não foi possível criar uma nova categoria para esta moeda... Moeda Não criada");
                        this.Close();
                    }

                    newCategory = true;
                } else {
                    coinCategory = (CoinCategory)comboCategoryList.SelectedItem;
                }

                string desc = description.Text;
                string facial = facialValue.Text;
                string mValue = marketValue.Text;
                string country = countryValue.Text;
                string date = dateValue.Text;

                CoinDao coinDao = new CoinDao();
                string userId = SessionContext.CurrentUser.id;
                string categoryId = coinCategory.id;

                Coin coin = new Coin(userId, categoryId, collectionCount, desc, type, mValue, facial, country, date);
                Coin insertedCoin = coinDao.Insert(coin);

                if (insertedCoin.id != null) {
                    //upd+ting the coin category coins list

                    if (!newCategory) {
                        CoinCategory updatedCategory = (CoinCategory)coinCategory.Clone();
                        updatedCategory.coinsList.Add(coin);
                    } else {
                        //new category == add new coin to list any way
                        coinCategory.coinsList.Add(coin);
                        SessionContext.CollectionCategories.Add(coinCategory);
                    }

                    this.Close();
                }
                else {
                    MessageBox.Show("Não foi possível adicionar uma nova moeda/nota...");
                }
            }
        }
        
        private CoinCategory createNewCategory(string newCategoryName) {
            CoinCategoryDao coinCategoryDao = new CoinCategoryDao();
            CoinCategory coinCategory = new CoinCategory(newCategoryName, "", SessionContext.CurrentUser.id);

            if(coinCategoryDao.Insert(coinCategory).id == null) {
                return null;
            }

            return coinCategory;
        }

        private Boolean ValidateField(string field) {
            if(string.IsNullOrEmpty(field)) {
                return false;
            }

            return true;
        }
    }
}
