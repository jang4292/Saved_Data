namespace CommonData.Data
{
    public class UserData
    {
        private static string ownerUID;
        private static string machineUID;
        private static string mileage;
        private static string userName;
        private static string priceOfTax;
        private static string portNumber;

        public static string MACHINE_UID
        {
            get
            {
                return machineUID;
            }

            set
            {
                machineUID = value;
            }
        }


        public static string OWNER_UID
        {
            get
            {
                return ownerUID;
            }

            set
            {
                ownerUID = value;
            }
        }

        public static string MILEAGE
        {
            get
            {
                return mileage;
            }

            set
            {
                mileage = value;
            }
        }

        public static string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public static string PriceOfTax
        {
            get
            {
                return priceOfTax;
            }

            set
            {
                priceOfTax = value;
            }
        }

        public static string PortNumber
        {
            get
            {
                return portNumber;
            }

            set
            {
                portNumber = value;
            }
        }
    }
}