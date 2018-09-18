using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoinCollectionManagement.AddCollection;

namespace CoinCollectionManagement.ViewModel
{
    #region DropExcellConfigurationViewModel
    class DropExcellConfigurationViewModel
    {
        private List<HeaderExcelColumn> _headerExcelColumns;

        public List<HeaderExcelColumn> HeaderExcelColumns {
            get {
                if(_headerExcelColumns == null) {
                    return new List<HeaderExcelColumn>();
                } else {
                    return _headerExcelColumns;
                }
            }

            set {
                _headerExcelColumns = value;
            }
        }

        private List<ConfigurationOption> _configurationOptions;
    
        public List<ConfigurationOption> ConfigurationOptions {
            get {
                _configurationOptions = new List<ConfigurationOption>();
                _configurationOptions.Add(new ConfigurationOption(1, ReadingCategory.MANUAL_NAME, "Nome da Moeda"));
                _configurationOptions.Add(new ConfigurationOption(2, ReadingCategory.MANUAL_COUNTER, "Número de moedas"));
                _configurationOptions.Add(new ConfigurationOption(3, ReadingCategory.MANUAL_MARKET_VALUE, "Valor do mercado"));
                _configurationOptions.Add(new ConfigurationOption(4, ReadingCategory.MANUAL_FACIAL_VALUE, "Valor facial"));
                _configurationOptions.Add(new ConfigurationOption(5, ReadingCategory.MANUAL_DATE, "Data da Moeda"));
                _configurationOptions.Add(new ConfigurationOption(6, ReadingCategory.MANUAL_COUNTRY, "País da moeda"));
                _configurationOptions.Add(new ConfigurationOption(7, ReadingCategory.MANUAL_TYPE, "Tipo da Moeda"));
                _configurationOptions.Add(new ConfigurationOption(8, ReadingCategory.MANUAL_DESCRIPTION, "Descrição da moeda"));
                _configurationOptions.Add(new ConfigurationOption(9, ReadingCategory.MANUAL_CATEGORY, "Categoria da moeda"));

                return _configurationOptions;
            }
        }

        public static DropExcellConfigurationViewModel GetDropExcellConfigurationViewModel(List<HeaderExcelColumn> HeaderExcelColumns) {
            DropExcellConfigurationViewModel dropExcellConfigurationViewModel = new DropExcellConfigurationViewModel();
            dropExcellConfigurationViewModel.HeaderExcelColumns = HeaderExcelColumns;
            return dropExcellConfigurationViewModel;
        }
    }
    #endregion

    #region ConfigurationOption
    class ConfigurationOption {
        private int _ID;
        private ReadingCategory _categoryEnumerator;
        private string _categoryString;

        public int ID {
            get {
                return _ID;
            }

            set {
                _ID = value;
            }
        }

        public ReadingCategory categoryEnumerator {
            get {
                return _categoryEnumerator;
            }

            set {
                _categoryEnumerator = value;
            }
        }

        public string categoryString {
            get {
                return _categoryString;
            }

            set {
                _categoryString = value;
            }
        }

        public override string ToString() {
            return _categoryString;
        }

        public ConfigurationOption(int ID, ReadingCategory categoryEnumerator, string categoryString) {
            _ID = ID;
            _categoryEnumerator = categoryEnumerator;
            _categoryString = categoryString;
        }
    }
    #endregion

    #region HeaderExcelColumn
    public class HeaderExcelColumn {
        public int Index {
            get;
            set;
        }

        public string Address {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }

        public override string ToString() {
            return Description + "(" + Address + ")";
        }
    }
    #endregion
}
