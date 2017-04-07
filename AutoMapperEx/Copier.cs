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
            return Xerox.Map<Customer>(p_plan);
        }
    }
}