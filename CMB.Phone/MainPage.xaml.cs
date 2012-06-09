using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml.Linq;
using System.Xml;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;

namespace CMB.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<Country> _countries;
        private List<Service> _services;

        // Constructor
        public MainPage()
        {
            InitializeComponent();


            XDocument _data = XDocument.Load("data/data.xml");
             _countries = ConvertXml(_data);
             InvertTree();
            //ObservableCollection<Country> i = new ObservableCollection<Country>(_countries);
            
            DataContext = _services;
        }

       
      
        private void Button_Click(object sender, RoutedEventArgs e)
        {



            XDocument _data = XDocument.Load("data/data.xml");


            //var i = doc.Element("countries");


            //MessageBox.Show(i.ToList().Count.ToString());

        }

        public List<Country> ConvertXml(XDocument data)
        {
            List<Country> _return = new List<Country>();

            var test = from country in data.Descendants("country")
                       select

                           new Country()
                           {
                               Name = country.Attribute("name").Value,
                               ISOCode = country.Attribute("code").Value,
                               LanguageCode = Convert.ToInt16(country.Attribute("languagecode").Value),
                               Banks = (
                               from bank in


                                   country.Descendants("bank")
                               select

                                   new Bank()
                                   {
                                       Name = bank.Attribute("name").Value,
                                       Url = bank.Attribute("url").Value,
                                       Services = (
                                       from service in bank.Descendants("service")
                                       select

                                       new Service()
                                       {
                                           ServiceID = new Guid(service.Attribute("id").Value),
                                           Type = (ServiceType)Convert.ToInt16(service.Attribute("type").Value),
                                           NationalNumber = service.Attribute("national").Value,
                                           InternationalNumber = service.Attribute("international").Value
                                       }


                                       ).ToList<Service>()
                                   }).ToList<Bank>()

                           }
                        ;
            _return = test.ToList();




            return _return;

        }

        public void InvertTree() {
            _services = new List<Service>();
            foreach (var country in _countries)
            {
                foreach (var bank in country.Banks)
                {
                    foreach (var service in bank.Services)
                    {
                        service.Parent = bank;
                        _services.Add(service);
                    }
                    bank.Parent = country;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PhoneCallTask pt = new PhoneCallTask();
            Service s = (e.OriginalSource as Button).Tag as Service;

            pt.PhoneNumber = s.NationalNumber;
            pt.DisplayName = s.Parent.Name;
            pt.Show();
        }
    }
}