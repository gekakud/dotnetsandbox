using AutoMapper;

namespace AutoMapperEx
{
    internal class Copier
    {
        private static readonly IMapper Xerox =
            new MapperConfiguration(p_conf => p_conf.CreateMap<Customer, Customer>()).CreateMapper();

        static Copier()
        {
            //for lazyness
        }

        public static Customer CopyPlan(Customer p_plan)
        {
            Mapper.Initialize(
                p_conf =>
                    p_conf.CreateMap<Customer, Customer>()
                        .ForMember(p_item => p_item.LastName,
                            p_opt => p_opt.NullSubstitute("Unknown"))
                        );
            var ttt = Mapper.Map<Customer>(p_plan);
            return ttt;
            //return Xerox.Map<Customer>(p_plan);
        }
    }
}