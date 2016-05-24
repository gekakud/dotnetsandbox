using System;
using VisitorPattern.Visitors;

namespace VisitorPattern
{
    internal class Program
    {
        private static void Main()
        {
            var person = new Human();

            person.Assets.Add(new RealEstate {EstimatedValue = 122000, MonthlyRent = 1000});
            person.Assets.Add(new RealEstate {EstimatedValue = 96000, MonthlyRent = 800});

            person.Assets.Add(new BankAccount {CurrentAmount = 20000, MonthlyEstimatedIncome = 4000});

            person.Assets.Add(new Loan {Owed = 10000, MonthlyPayment = 800});
            person.Assets.Add(new Loan {Owed = 3000, MonthlyPayment = 200});

            var personWorthVisitor = new PersonWorthVisitor();
            person.Accept(personWorthVisitor);
            Console.WriteLine(personWorthVisitor.Total);

            var personIncomeVisitor = new PersonMonthlyIncomeVisitor();
            person.Accept(personIncomeVisitor);
            Console.WriteLine(personIncomeVisitor.Income);

            //we do not want to keep this logic
            //moreover, each time we add functionality(monthly income for instance)
            //we have to add it here
//            int personWorth = 0;
//
//            foreach (var bankAccount in person.BankAccounts)
//            {
//                personWorth += bankAccount.CurrentAmount;
//            }
//
//            foreach (var realEstate in person.RealEstates)
//            {
//                personWorth += realEstate.EstimatedValue;
//            }
//
//            foreach (var loan in person.Loans)
//            {
//                personWorth -= loan.Owed;
//            }
//            Console.WriteLine(personWorth);

            Console.ReadKey();
        }
    }
}