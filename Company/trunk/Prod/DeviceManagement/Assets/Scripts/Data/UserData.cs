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
                //return "3ed2fbbf-7aae-4430-a582-79d2e8867a53";
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
                //return "a0303603-25b8-4735-b6e3-8594684ef536";
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
                //return "456456";
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
                //return "\uc810\uc8fc";
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