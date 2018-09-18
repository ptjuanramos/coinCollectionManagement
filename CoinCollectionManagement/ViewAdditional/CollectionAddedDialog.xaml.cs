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

namespace CoinCollectionManagement.ViewAdditional {
    /// <summary>
    /// Interaction logic for CollectionAddedDialog.xaml
    /// </summary>
    public partial class CollectionAddedDialog : Window {
        public CollectionAddedDialog() {
            InitializeComponent();

            comboCategoryList.ItemsSource = SessionContext.CollectionCategories;
        }

        private void FinishCollectionButton_Click(object sender, RoutedEventArgs e) {

        }
    }
}
