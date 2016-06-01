using System.Collections.Generic;
using VisitorPattern.Visitors;

namespace VisitorPattern
{
    public interface IAsset
    {
        void Accept(IVisitor p_visitor);
    }

    public class Human : IAsset
    {
        public List<IAsset> Assets = new List<IAsset>();

//        public List<RealEstate> RealEstates = new List<RealEstate>();
//        public List<BankAccount> BankAccounts = new List<BankAccount>();
//        public List<Loan> Loans = new List<Loan>(); 
        public void Accept(IVisitor p_visitor)
        {
            foreach (var asset in Assets)
            {
                asset.Accept(p_visitor);
            }
        }
    }

    public class RealEstate : IAsset
    {
        public int EstimatedValue { get; set; }
        public int MonthlyRent { get; set; }

        public void Accept(IVisitor p_visitor)
        {
            p_visitor.Visit(this);
        }
    }

    public class BankAccount : IAsset
    {
        public int MonthlyEstimatedIncome { get; set; }
        public int MonthlyPayments { get; set; }
        public int CurrentAmount { get; set; }

        public void Accept(IVisitor p_visitor)
        {
            p_visitor.Visit(this);
        }
    }

    public class Loan : IAsset
    {
        public int Owed { get; set; }
        public int MonthlyPayment { get; set; }

        public void Accept(IVisitor p_visitor)
        {
            p_visitor.Visit(this);
        }
    }
}