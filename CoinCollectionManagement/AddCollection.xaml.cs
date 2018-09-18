using CoinCollectionManagement.DAO;
using CoinCollectionManagement.Internal;
using CoinCollectionManagement.ViewAdditional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace CoinCollectionManagement {
    /// <summary>
    /// Interaction logic for AddCollection.xaml
    /// </summary>
    public partial class AddCollection : Window {
        private string[] files = new string[1];

        public AddCollection() {
            InitializeComponent();
        }

        private void DropExcelBoxHandler(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] inputFile = (string[])e.Data.GetData(DataFormats.FileDrop);
                files[0] = inputFile[0];
                HandleFile(files[0]);
            }
        }

        //TODO - TO REMOVE
        //TODO - handle csv on next interations
        private void HandleFile(string inputFile) {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(inputFile);

            //all sheets from the excel file
            Excel.Sheets listOfWorksheets = xlWorkbook.Sheets;
            List<CoinCategory> coinCategories = GetFilledCoinCategoriesFromSheets(listOfWorksheets);

            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            /*CoinCategoryDao coinCategoryDao = new CoinCategoryDao();

            foreach(CoinCategory coinCategory in coinCategories) {
                CoinCategory persistedCoinCategory = coinCategoryDao.InsertCategoryWithCoins(coinCategory);
                SessionContext.CollectionCategories.Add(persistedCoinCategory);
            } */

            MessageBox.Show("Coleção adicionada...");
            this.Close();
        }

        /*
         * Obtain a list of categories based on the file sheets 
         */
        private List<CoinCategory> GetFilledCoinCategoriesFromSheets(Excel.Sheets listOfSheets) {
            List<CoinCategory> coinCategories = new List<CoinCategory>();

            //TODO - impl this
            if(listOfSheets.Count == 1) {
                //Handle only one sheet - TODO - check for manual category
            }

            foreach(Excel._Worksheet sheet in listOfSheets) {
                //Configure before reading file!
                Dictionary<ReadingCategory, List<int>> sheetConfigurations = GetMultipleColumnsNames(sheet);

                //TODO - before this we need to fix the HandleSingleFile method
                String categoryName = sheet.Name;
                String categoryDescription = ""; //TODO - maybe on next interations
                CoinCategory obtainedCategory = new CoinCategory(categoryName, categoryDescription, SessionContext.CurrentUser.id);
                //obtainedCategory = HandleSingleSheet(sheet, obtainedCategory);
                
                coinCategories.Add(obtainedCategory);
            }

            return coinCategories;
        }

        private Dictionary<ReadingCategory, List<int>> GetMultipleColumnsNames(Excel._Worksheet sheet) {
            Excel.Range xlRange = sheet.UsedRange;

            int columnCount = xlRange.Columns.Count;
            Dictionary<string, string> cellsDescriptionAndIndexWithAddress = new Dictionary<string, string>();

            for(int i = 1; i <= columnCount; i++) {
                Excel.Range firstRowRange = xlRange.Cells[1, i];

                if(firstRowRange.Value2 != null) {
                    string cellNameAndAddress = firstRowRange.Value2.ToString() + "_" + GetColumnAdress(i);
                    string cellIndex = i + "";
                    cellsDescriptionAndIndexWithAddress.Add(cellIndex, cellNameAndAddress);
                }
            }

            Dictionary<ReadingCategory, List<int>> headerResult = OpenColumnsDescriptionConfiguration(cellsDescriptionAndIndexWithAddress);

            return headerResult;
        }

        /*
         * Open the excel sheet configuration menu to choose 
         */
        private Dictionary<ReadingCategory, List<int>> OpenColumnsDescriptionConfiguration(Dictionary<string, string> cellDescriptionAndIndex) {
            Dictionary<ReadingCategory, List<int>> headerResult = new Dictionary<ReadingCategory, List<int>>();

            ConfigureSheetColumns configureSheetColumns = new ConfigureSheetColumns(cellDescriptionAndIndex);

            //window onyl closes when the configuration is done or some 
            bool? isClosed = configureSheetColumns.ShowDialog();

            if(isClosed == true) {
                //TODO check if something went wrong
            }

            return headerResult;
        }

        private CoinCategory HandleSheetAfterConfiguration(Dictionary<ReadingCategory, List<int>> sheetConfiguration,
            Excel._Worksheet sheet) {
            CoinCategory coinCategory = new CoinCategory();
            //TODOs
            return coinCategory;
        }

        /*
         * Obtains the columns address string from column index 
         */
        private string GetColumnAdress(int columnNumber) {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0) {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        /*
         * TODO - fix all method
         * Handle a sheet for each Coin categoary - obtain a list of coins/bills 
         */
        private CoinCategory HandleSingleSheet(Excel._Worksheet xlWorksheet, CoinCategory coinCategory) {

            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int columnCount = xlRange.Columns.Count;
            List<Coin> coinsToInsert = new List<Coin>();

            //TODO - Associate Coin values with the configuration settings from user input
            /*
            for (int i = 2; i <= rowCount; i++) {
                Coin coin = new Coin();
                coin.userId = SessionContext.CurrentUser.id;

                for (int j = 1; j <= columnCount; j++) {
                    //avoiding the double dot rule
                    Excel.Range range = xlRange.Cells[i, j];

                    if (range != null && range.Value2 != null) {
                        switch (j) {
                            case 1:
                                coin.type = xlRange.Cells[i, j].Value2.ToString();
                                break;

                            case 2:
                                coin.marketValue = xlRange.Cells[i, j].Value2.ToString();
                                break;

                            case 3:
                                coin.facialValue = xlRange.Cells[i, j].Value2.ToString();
                                break;

                            case 4:
                                coin.country = xlRange.Cells[i, j].Value2.ToString();
                                break;

                            case 5:
                                coin.date = xlRange.Cells[i, j].Value2.ToString();
                                break;
                        }
                    }
                }

                coinsToInsert.Add(coin);
            } */

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //fill the CoinCategory with the obtained list of coins
            coinCategory.coinsList = coinsToInsert;

            return coinCategory;
        }

        private void ButtonConfigurationHandler(object sender, EventArgs e) {
            
        }

        /*
         * Represents the associated header columns with the reading file 
         * TODO for now only manual settings
         */
        public enum ReadingCategory{
            AUTO_NAME,
            MANUAL_NAME,
            AUTO_COIN_DESCRIPTION,
            MANUAL_COIN_DESCRIPTION,
            AUTO_COUNTER,
            MANUAL_COUNTER,
            AUTO_MARKET_VALUE,
            MANUAL_MARKET_VALUE,
            AUTO_FACIAL_VALUE,
            MANUAL_FACIAL_VALUE,
            AUTO_DATE,
            MANUAL_DATE,
            AUTO_COUNTRY,
            MANUAL_COUNTRY,
            AUTO_TYPE,
            MANUAL_TYPE,
            AUTO_DESCRIPTION,
            MANUAL_DESCRIPTION,
            AUTO_CATEGORY,
            MANUAL_CATEGORY,

            //DEFAULT VALUE doesnt do anything - just a controll value
            DEFAULT_VALUE
        }
    }
}
