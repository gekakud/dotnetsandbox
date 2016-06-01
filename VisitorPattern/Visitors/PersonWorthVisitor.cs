namespace VisitorPattern.Visitors
{
    public class PersonWorthVisitor:IVisitor
    {
        public int Total { get; set; }

        public void Visit(RealEstate p_realEstate)
        {
            Total += p_realEstate.EstimatedValue;
        }

        public void Visit(BankAccount p_bankAccount)
        {
            Total += p_bankAccount.CurrentAmount;
        }

        public void Visit(Loan p_loan)
        {
            Total -= p_loan.Owed;
        }
    }
}