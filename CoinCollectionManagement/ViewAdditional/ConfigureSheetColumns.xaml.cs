using CoinCollectionManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static CoinCollectionManagement.AddCollection;

namespace CoinCollectionManagement.ViewAdditional {
    /// <summary>
    /// Interaction logic for ConfigureSheetColumns.xaml
    /// </summary>
    public partial class ConfigureSheetColumns : Window {
        private Dictionary<string, string> _cellDescriptionAndIndex;

        public bool IsConfigurationAvailable {
            get;
            set;
        }

        public Dictionary<ReadingCategory, List<int>> ConfigurationResult = new Dictionary<ReadingCategory, List<int>>();

        //ConfigurationOptions
        public List<HeaderExcelColumn> listOfHeaderColumns = new List<HeaderExcelColumn>();

        public ConfigureSheetColumns(Dictionary<string, string> cellDescriptionAndIndex) {
            _cellDescriptionAndIndex = cellDescriptionAndIndex;
            SetupConfigurationOptions(cellDescriptionAndIndex);

            InitializeComponent();

            //fill list view with combobox
            FillListOfHeaderColumns();

            DataContext = DropExcellConfigurationViewModel.GetDropExcellConfigurationViewModel(listOfHeaderColumns);
        }

        private void SetupConfigurationOptions(Dictionary<string, string> cellIndexAndDescription) {
            
        }

        private ReadingCategory GetReadingCategoryFromCellDescription(string cellDescription) {

            //TODO
            return ReadingCategory.DEFAULT_VALUE;
        }

        private void FillListOfHeaderColumns() {
            foreach (string cellDescriptionAndIndexKey in _cellDescriptionAndIndex.Keys) {
                string cellDescriptionAndIndexValue = _cellDescriptionAndIndex[cellDescriptionAndIndexKey];
                string cellAddress = cellDescriptionAndIndexValue.Split('_')[1];
                string cellDescription = cellDescriptionAndIndexValue.Split('_')[0];

                HeaderExcelColumn headerExcelColumnTemp = new HeaderExcelColumn();
                headerExcelColumnTemp.Index = Int32.Parse(cellDescriptionAndIndexKey);
                headerExcelColumnTemp.Address = cellAddress;
                headerExcelColumnTemp.Description = cellDescription;

                listOfHeaderColumns.Add(headerExcelColumnTemp);
            }
        }

        public void FinishConfigurationButton_Handler(object sender, RoutedEventArgs args) {
            foreach(var listViewItem in configurationListView.Items) {
                return; //TODO 
            }
            //TODO more stuff
        }

        public void SaveConfigurationButton_Handler(object sender, RoutedEventArgs args) {
            //TODO save configuration memo
        }

        private void CheckForUncheckedConfigurations() {

        }
    }
}
