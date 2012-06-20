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
using System.IO.IsolatedStorage;
using System.IO;
using SilverlightPhoneDatabase;

namespace CMB.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public DependencyProperty ServicesProperty = DependencyProperty.RegisterAttached("Services", typeof(List<Service>), typeof(MainPage), new PropertyMetadata(new List<Service>()));
        public DependencyProperty FavsProperty = DependencyProperty.RegisterAttached("Favs", typeof(List<Service>), typeof(MainPage), new PropertyMetadata(new List<Service>()));
        private static List<Country> _countries = new List<Country>();
        private List<Service> _services = new List<Service>();
        private List<Service> _favs = new List<Service>();

        public List<Service> Favs
        {
            get { return (List<Service>)GetValue(FavsProperty); }
            set { SetValue(FavsProperty, value); }
        }

        public List<Service> Services
        {
            get { return (List<Service>)GetValue(ServicesProperty); }
            set { SetValue(ServicesProperty, value); }
        }
        private IsolatedStorageFile myFile = IsolatedStorageFile.GetUserStoreForApplication();
        private string dfile = "data/data.xml";
        IsolatedStorageFileStream datafile;
        private Database database;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (!Database.DoesDatabaseExists("favs"))
            {
                database = Database.CreateDatabase("favs");
                database.CreateTable<Guid>();
                database.Save();
            }
            else
            {
                database = Database.OpenDatabase("favs", string.Empty, false);
            }

            Favs = new List<Service>();


            //if (!myFile.FileExists(dfile)){
            //    datafile = myFile.CreateFile(dfile);
            //    datafile.Close();
            //}

            XDocument _data = XDocument.Load(dfile);
            _countries = ConvertXml(_data);
            InvertTree();
            //ObservableCollection<Country> i = new ObservableCollection<Country>(_countries);

            foreach (Guid guid in database.Table<Guid>().ToList<Guid>())
            {
                foreach (var svc in Services)
                {
                    if (svc.ServiceID == guid)
                    {
                        Favs.Add(svc);
                        svc.Favourite = true;
                    }
                }
            }

            DataContext = this;
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
                                           InternationalNumber = service.Attribute("international").Value,
                                           Favourite = false
                                       }


                                       ).ToList<Service>()
                                   }).ToList<Bank>()

                           }
                        ;
            _return = test.ToList();




            return _return;

        }

        public void InvertTree()
        {
            Services = new List<Service>();
            foreach (var country in _countries)
            {
                foreach (var bank in country.Banks)
                {
                    foreach (var service in bank.Services)
                    {
                        service.Parent = bank;
                        Services.Add(service);
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var snd = (sender as CheckBox).Tag as Service;
            if (!database.Table<Guid>().Contains(snd.ServiceID))
            {
                database.Table<Guid>().Add(snd.ServiceID);
                database.Save();
                Favs = new List<Service>();
                foreach (Guid guid in database.Table<Guid>().ToList<Guid>())
                {
                    foreach (var svc in Services)
                    {
                        if (svc.ServiceID == guid)
                        {
                            Favs.Add(svc);

                            svc.Favourite = true;
                        }
                    }
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var snd = (sender as CheckBox).Tag as Service;
            database.Table<Guid>().Remove(snd.ServiceID);
            database.Save();
            Favs = new List<Service>();
            foreach (Guid guid in database.Table<Guid>().ToList<Guid>())
            {
                foreach (var svc in Services)
                {
                    if (svc.ServiceID == guid)
                    {
                        Favs.Add(svc);

                        svc.Favourite = false;
                    }
                }
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            database.Table<Guid>().Clear();
            database.Save();
            Favs = new List<Service>();
            foreach (var item in Services)
            {
                item.Favourite = false;
            }

        }
    }
}