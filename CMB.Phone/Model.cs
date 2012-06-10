namespace CMB.Phone
{
    using System.Collections.Generic;

    // commento
    public partial class Countries
    {

        private List<CountriesCountry> countryField;

        public Countries()
        {
            this.countryField = new List<CountriesCountry>();
        }

        public List<CountriesCountry> country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }

    public partial class CountriesCountry
    {

        private List<CountriesCountryBank> banksField;

        private string nameField;

        private string codeField;

        private ushort languagecodeField;

        public CountriesCountry()
        {
            this.banksField = new List<CountriesCountryBank>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("bank", IsNullable = false)]
        public List<CountriesCountryBank> banks
        {
            get
            {
                return this.banksField;
            }
            set
            {
                this.banksField = value;
            }
        }

        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        public ushort languagecode
        {
            get
            {
                return this.languagecodeField;
            }
            set
            {
                this.languagecodeField = value;
            }
        }
    }

    public partial class CountriesCountryBank
    {

        private List<countriesCountryBankService> servicesField;

        private string nameField;

        private string urlField;

        public CountriesCountryBank()
        {
            this.servicesField = new List<countriesCountryBankService>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("service", IsNullable = false)]
        public List<countriesCountryBankService> services
        {
            get
            {
                return this.servicesField;
            }
            set
            {
                this.servicesField = value;
            }
        }

        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }

    public partial class countriesCountryBankService
    {

        private string idField;

        private string typeField;

        private uint nationalField;

        private string internationalField;

        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        public uint national
        {
            get
            {
                return this.nationalField;
            }
            set
            {
                this.nationalField = value;
            }
        }

        public string international
        {
            get
            {
                return this.internationalField;
            }
            set
            {
                this.internationalField = value;
            }
        }
    }
}
