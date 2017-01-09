using System;
using AutoMapper;

namespace AutoMapperExample
{
    internal class Program
    {
        private static void Main()
        {
            //get entity object
            var dataObject = DataProvider.GetCustomer();

            //apply mapping
            var viewObject = new ToUiMapper().MapItem(dataObject);

            //show on UI
            Presenter.ShowOnUi(viewObject);

            Console.ReadKey();
        }
    }

    internal class ToUiMapper
    {
        public ToUiMapper()
        {
            //the mapper will produce:
            //  -if FirstName is null it will substitute value "Unknown"
            //  -it will ignore mapping for Address
            //  -it checks if LastName is nul or empty but does nothing
            Mapper.Initialize(
                p_conf =>
                    p_conf.CreateMap<Customer, CustomerViewItem>()
                        .ForMember(p_item => p_item.FirstName,
                            p_opt => p_opt.NullSubstitute("Unknown"))
                        .ForMember(p_item => p_item.LastName,
                            p_opt => p_opt.Condition(p_src => !string.IsNullOrEmpty(p_src.LastName)))
                        .ForMember(p_item => p_item.Address, p_opt => p_opt.Ignore()));
        }

        public CustomerViewItem MapItem(Customer p_customerDataObject)
        {
            var mappedItem = Mapper.Map<CustomerViewItem>(p_customerDataObject);
            return mappedItem;
        }
    }

    internal class DataProvider
    {
        public static Customer GetCustomer()
        {
            return new Customer
            {
                DateOfBirth = DateTime.Now,
                LastName = "Kud",
                ID = 3244444,
                NumberOfOrders = 22,
                Address = "karmiel"
            };
        }
    }

    internal class Presenter
    {
        public static void ShowOnUi(CustomerViewItem p_customer)
        {
            Console.WriteLine("");
        }
    }

    #region Two dependent by data classes

    // We have Customer entity, and we are going to show Customers on UI
    // and for that, we need a much lighter object CustomerViewItem

    // problem:
    // we need to map all fields from Customer to CustomerViewItem
    // but these two objects are different and we do not want to make it manually like
    //      customerView.FirstName = customer.FirstName and so on.
    //      moreover, maybe we want to ignore or apply additional logic on property
    //      AutoMapper allows us to add conditions to properties that must be met before that property will be mapped
    internal class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int NumberOfOrders { get; set; }
        public int ID { get; set; }

        //we want to ignore mapping this property because it has different data types
        //in Customer and CustomerViewItem classes(as example)
        public string Address { get; set; }
    }

    internal class CustomerViewItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }

        //we want to ignore mapping this property because it has different data types
        //in Customer and CustomerViewItem classes(as example)
        public Address Address { get; set; }
    }

    internal class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
    }

    #endregion
}
