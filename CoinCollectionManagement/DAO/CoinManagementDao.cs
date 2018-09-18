using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoinCollectionManagement.DAO
{
    public abstract class IDao<T> {
        protected SqlConnection sqlConnection;

        protected readonly string ServerName = "den1.mssql1.gear.host";
        protected readonly string DataBaseName = "coinmanagement";
        protected readonly string UserName = "coinmanagement";
        protected readonly string Password = "Nt35c~B0~Dc2";

        abstract public T Insert(T objectKey);
        abstract public T Get(string id);
        abstract public Boolean Delete(string id);
        abstract public T Update(T objectKey);

        /*
         * Opens the database connection
         */
        protected void OpenDatabaseConnection() {
            try {
                sqlConnection.Open();
            }
            catch (Exception ex) {
                MessageBox.Show("Não foi possível utilizar a base de dados..." + ex.Message);
            }
        }

        /*
         * Closes the database connection
         */
        protected void CloseDatabaseConnection() {
            try {
                sqlConnection.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("Não foi possível utilizar a base de dados...");
            }
        }
    }

    public class CoinCategoryDao : IDao<CoinCategory> {
        private SqlCommand sqlCommand;
        private SqlDataReader dataReader;

        public CoinCategoryDao() {
            string connectionString = "Data Source=" + ServerName +
                ";Initial Catalog=" + DataBaseName + ";User ID=" + UserName + ";Password=" + Password;
            sqlConnection = new SqlConnection(connectionString);
        }

        public override bool Delete(string id) {
            throw new NotImplementedException();
        }

        public override CoinCategory Get(string id) {
            OpenDatabaseConnection();
            string query = "SELECT * FROM CoinsCategory WHERE Id = " + id;
            sqlCommand = new SqlCommand(query, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();
            CoinCategory result = new CoinCategory();

            while(dataReader.Read()) {
                result.id = dataReader.GetInt32(0) + "";
                result.name = dataReader.GetString(1);
                result.userId = dataReader.GetInt32(2) + "";
            }

            dataReader.Close();
            CloseDatabaseConnection();

            return result;
        }

        public List<CoinCategory> GetAll(string UserId) {
            string query = "SELECT * FROM CoinsCategory WHERE UserId = " + UserId; 
            List<CoinCategory> resultList = new List<CoinCategory>();
            CoinDao coinDao = new CoinDao();

            OpenDatabaseConnection();
            sqlCommand = new SqlCommand(query, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();

            while(dataReader.Read()) {
                CoinCategory temp = new CoinCategory();
                temp.id = dataReader.GetInt32(0) + "";
                temp.name = dataReader.GetString(1) + "";
                temp.userId = dataReader.GetInt32(2) + "";
                temp.coinsList = coinDao.GetCoinsByCategory(UserId, temp.id);

                resultList.Add(temp);
            }

            CloseDatabaseConnection();

            return resultList;
        }

        public override CoinCategory Insert(CoinCategory objectKey) {
            OpenDatabaseConnection();
            string query = "INSERT INTO CoinsCategory(Name, UserID) output INSERTED.ID VALUES ('" + objectKey.name + "','" + objectKey.userId + "')";
            sqlCommand = new SqlCommand(query, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();
            dataReader.Read();
            objectKey.id = dataReader.GetInt32(0) + "";
            CloseDatabaseConnection();

            return objectKey;
        }

        public CoinCategory InsertCategoryWithCoins(CoinCategory coinCategory) {
            if(coinCategory.coinsList == null || coinCategory.coinsList.Count == 0) {
                throw new OperationCanceledException("Coins list cannot be empty or null");
            }

            //persisting coin category
            CoinCategory persistedCategory = this.Insert(coinCategory);

            if(persistedCategory.id == null) {
                throw new OperationCanceledException("Couldn't persist the coin category");
            }

            coinCategory.id = persistedCategory.id;

            //persisting coin list
            CoinDao coinDao = new CoinDao();
            coinDao.InsertList(coinCategory.coinsList);

            return coinCategory;
        }

        public override CoinCategory Update(CoinCategory objectKey) {
            throw new NotImplementedException();
        }
    }

    public class CoinDao : IDao<Coin> {
        private SqlCommand sqlCommand;
        private SqlDataReader dataReader;

        public CoinDao() {
            string connectionString = "Data Source=" + ServerName + 
                ";Initial Catalog=" + DataBaseName + ";User ID=" + UserName + ";Password=" + Password;
            sqlConnection = new SqlConnection(connectionString);
        }

        public override bool Delete(string id) {
            OpenDatabaseConnection();
            string query = "DELETE FROM Coins WHERE Coins.ID = " + id;
            sqlCommand = new SqlCommand(query, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();


            dataReader.Close();
            CloseDatabaseConnection();
            return true;
        }

        public List<Coin> GetAll(string username) {
            OpenDatabaseConnection();
            List<Coin> listCoins = new List<Coin>();
            string query = "SELECT * FROM Coins WHERE UserId = (SELECT ID FROM Users WHERE Username = '" + username + "')";
            sqlCommand = new SqlCommand(query, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();

            while(dataReader.Read()) {
                Coin tempCoin = new Coin();
                tempCoin.id = dataReader.GetInt32(0) + "";
                tempCoin.userId = dataReader.GetInt32(1) + "";
                tempCoin.categoryId = dataReader.GetInt32(2) + "";
                tempCoin.counter = dataReader.GetInt32(3);
                tempCoin.description = dataReader.GetString(4);
                tempCoin.type = dataReader.GetString(5);
                tempCoin.facialValue = dataReader.GetString(6);
                tempCoin.marketValue = dataReader.GetString(7);
                tempCoin.country = dataReader.GetString(8);
                tempCoin.date = dataReader.GetString(9);

                listCoins.Add(tempCoin);
            }

            dataReader.Close();
            CloseDatabaseConnection();

            //sort list of coin
            listCoins.Sort();

            return listCoins;
        }

        //TODO - change to batch operation
        public void InsertList(List<Coin> listCoins) {
            foreach(Coin toInsertCoin in listCoins) {
                this.Insert(toInsertCoin);
            }
        }

        //TODO
        public override Coin Get(string id) {
            OpenDatabaseConnection();
            CloseDatabaseConnection();
            throw new NotImplementedException();
        }

        /**
         * get coins by a giving user id and coin category id
         */
        public List<Coin> GetCoinsByCategory(string userId, string categoryId) {
            OpenDatabaseConnection();
            List<Coin> coinsList = new List<Coin>();
            string query = "SELECT * FROM Coins WHERE UserId = '" + userId + "' AND CategoryID = '" + categoryId + "'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();

            while(dataReader.Read()) {
                Coin coin = new Coin();
                coin.id = dataReader.GetInt32(0) + "";
                coin.userId = dataReader.GetInt32(1) + "";
                coin.categoryId = dataReader.GetInt32(2) + "";
                coin.counter = dataReader.GetInt32(3);
                coin.description = dataReader.GetString(4);
                coin.type = dataReader.GetString(5);
                coin.facialValue = dataReader.GetString(6);
                coin.marketValue = dataReader.GetString(7);
                coin.country = dataReader.GetString(8);
                coin.date = dataReader.GetString(9);

                coinsList.Add(coin);
            }

            CloseDatabaseConnection();

            return coinsList;
        }

        public override Coin Insert(Coin objectKey) {
            OpenDatabaseConnection();

            string query = "INSERT INTO Coins(UserID, CategoryID, Counter, Description, CoinType, FacialValue, MarketValue, Country, CoinDate) " +
                "output INSERTED.ID VALUES ('" + objectKey.userId + "','" + objectKey.categoryId + "','" + objectKey.counter + "','" + objectKey.description + "','" + objectKey.type + "','" 
                + objectKey.facialValue + "','" + objectKey.marketValue + "','" + objectKey.country + "','" + objectKey.date + "')";

            sqlCommand = new SqlCommand(query, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();
            dataReader.Read();
            int coinId = dataReader.GetInt32(0);

            dataReader.Close();
            CloseDatabaseConnection();

            objectKey.id = coinId + "";
            return objectKey;
        }

        public override Coin Update(Coin objectKey) {
            OpenDatabaseConnection();
            CloseDatabaseConnection();
            throw new NotImplementedException();
        }
    }

    public class UserDao : IDao<User> {
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;

        public UserDao() {
            string connectionString = "Data Source=" + ServerName +
                ";Initial Catalog=" + DataBaseName + ";User ID=" + UserName + ";Password=" + Password;
            sqlConnection = new SqlConnection(connectionString);
        }

        public override bool Delete(string id) {
            OpenDatabaseConnection();
            CloseDatabaseConnection();
            throw new NotImplementedException();
        }

        public override User Get(string id) {
            OpenDatabaseConnection();
            CloseDatabaseConnection();
            throw new NotImplementedException();
        }

        public User GetByUsername(string username) {
            OpenDatabaseConnection();
            string query = "SELECT * FROM Users WHERE username = '" + username + "'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
        
            //Do not close data reader before GetUserFromOutput
            User user = GetUserFromOutput(sqlDataReader);

            sqlDataReader.Close();
            CloseDatabaseConnection();
            return user ; 
        }

        public override User Insert(User objectKey) {
            OpenDatabaseConnection();
            string query = "INSERT INTO User(Name, Username, Password, Role) " +
                " VALUE ('" + objectKey.name + "','" + objectKey.username 
                           + "','" + objectKey.password + "','" + objectKey.userRole + "')";
            sqlCommand = new SqlCommand(query, sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            int userId = sqlDataReader.GetInt32(0);

            sqlDataReader.Close();
            CloseDatabaseConnection();
            User insertedUser = new User(userId + "", objectKey.name, objectKey.username, 
                objectKey.password, objectKey.userRole, objectKey.email);

            return insertedUser;
        }

        public override User Update(User objectKey) {
            OpenDatabaseConnection();
            CloseDatabaseConnection();
            throw new NotImplementedException();
        }

        public Boolean IsPasswordCorrect(string username, string password) {
            User dbUser = GetByUsername(username);

            if (dbUser.password == null)
                return false;

            return dbUser.password.Equals(password);
        }

        private User GetUserFromOutput(SqlDataReader dataReader) {
            User user = new User();

            while (dataReader.Read()) {
                user.id = dataReader.GetInt32(0) + "";
                user.name = dataReader.GetString(1);
                user.username = dataReader.GetString(2);
                user.userRole = dataReader.GetString(4);
            }

            CloseDatabaseConnection();

            return user;
        }
    }
}
