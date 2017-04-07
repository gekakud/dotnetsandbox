using AutoMapper;

namespace AutoMapperEx
{
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
}