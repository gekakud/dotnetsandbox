namespace VisitorPattern.Visitors
{
    public class PersonMonthlyIncomeVisitor:IVisitor
    {
        public int Income { get; set; }

        public void Visit(RealEstate p_realEstate)
        {
            Income -= p_realEstate.MonthlyRent;
        }

        public void Visit(BankAccount p_bankAccount)
        {
            Income += p_bankAccount.MonthlyEstimatedIncome - p_bankAccount.MonthlyPayments;
        }

        public void Visit(Loan p_loan)
        {
            Income -= p_loan.MonthlyPayment;
        }
    }
}