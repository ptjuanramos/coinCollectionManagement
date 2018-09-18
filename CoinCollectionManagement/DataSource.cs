using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinCollectionManagement {
    public static class DataSource {
        public static string ADMIN_ROLE {
            get {
                return "ADMIN";
            }
        }
        public static string REGULAR_ROLE {
            get {
                return "REGULAR";
            }
        }
    }

    public static class StaticLoginInfo {
        public static String Username {
            get { return "username"; }
        }

        public static String Password {
            get { return "password"; }
        }
    }
}
