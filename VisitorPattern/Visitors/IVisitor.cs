namespace VisitorPattern.Visitors
{
    public interface IVisitor
    {
        void Visit(RealEstate p_realEstate);
        void Visit(BankAccount p_bankAccount);
        void Visit(Loan p_loan);
    }
}