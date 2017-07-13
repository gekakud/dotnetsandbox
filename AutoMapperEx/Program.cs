using System;

namespace AutoMapperEx
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Automapper example...");
            //get entity object
            var dataObject = DataProvider.GetCustomer();

            //apply mapping
            var viewObject = new ToUiMapper().MapItem(dataObject);

            //show on UI
            Presenter.ShowOnUi(viewObject);

            Console.WriteLine("Deep cloning object with Automapper...");
            Console.WriteLine("Create deep copy of object - hashcodes must be different!");

            var dataObjectCopy = Copier.CopyPlan(dataObject);
            Console.WriteLine("Hashcode is " + dataObject.GetHashCode() + " for original");
            Console.WriteLine("Hashcode is " + dataObjectCopy.GetHashCode() + " for copy");

            Console.WriteLine("Hashcodes are different, this is " +
                              (dataObjectCopy.GetHashCode() != dataObject.GetHashCode()));
            Console.ReadKey();
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
