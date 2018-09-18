using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinCollectionManagement.DAO
{
    
    public class Coin : IComparable<Coin> {
        public string id { set; get; }
        public string userId { set; get; }
        public string categoryId { set; get; }
        public int counter { set; get; }
        public string description { set; get; }
        public string type { set; get; }
        public string marketValue { set; get; }
        public string facialValue { set; get; }
        public string country { set; get; }
        public string date { set; get; }

        public Coin() { }

        public Coin(string Id, string UserId, string CategoryId, int Counter, string Description, string Type, string MarketValue, 
            string FacialValue, string Country, string Date) {
            id = Id;
            userId = UserId;
            counter = Counter;
            description = Description;
            type = Type;
            facialValue = FacialValue;
            marketValue = MarketValue;
            country = Country;
            date = Date;
            categoryId = CategoryId;
        }

        public Coin(string UserId, string CategoryId, int Counter, string Description, string Type, string MarketValue, string FacialValue, 
            string Country, string Date) {
            userId = UserId;
            counter = Counter;
            description = Description;
            type = Type;
            marketValue = MarketValue;
            facialValue = FacialValue;
            country = Country;
            date = Date;
            categoryId = CategoryId;
        }

        public int CompareTo(Coin coinToCompare) {
            try {
                DateTime toCompareDate = DateTime.Parse(coinToCompare.date);
                DateTime thisDate = DateTime.Parse(this.date);

                if (thisDate > toCompareDate)
                    return -1;
                if (thisDate == toCompareDate)
                    return 0;

                return 1;
            } catch(FormatException ex) {
                return 0;
            }
        }
    }

    public class User {
        public string id { set; get; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string userRole { get; set; }
        public string email { get; set; }

        public User() {}

        public User(string Id, string Name, string Username, string Password, string UserRole, string Email) {
            id = Id;
            name = Name;
            username = Username;
            password = Password;
            userRole = UserRole;
            email = Email;
        }

        public User(string Name, string Username, string Password, string UserRole, string Email) {
            name = Name;
            username = Username;
            password = Password;
            userRole = UserRole;
            email = Email;
        }
    }

    public class CoinCategory: ICloneable {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string userId { get; set; }

        private List<Coin> _coinsList;
        public List<Coin> coinsList {
            get {
                if (this._coinsList == null) {
                    this._coinsList =  new List<Coin>();
                    return _coinsList;
                } else {
                    return this._coinsList;
                }
            }

            set {
                this._coinsList = value;
            }
        }

        public CoinCategory() {}

        public CoinCategory(string Id, string Name, string Description, string UserId) {
            id = Id;
            name = Name;
            description = Description;
            userId = UserId;
        }

        public CoinCategory(string Name, string Description, string UserId) {
            name = Name;
            description = Description;
            userId = UserId;
        }

        public override string ToString() {
            return name;
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
