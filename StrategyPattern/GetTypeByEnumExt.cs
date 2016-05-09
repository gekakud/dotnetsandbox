using System;

namespace StrategyPattern
{
    public static class GetTypeByEnumExt
    {
        public static TOut GetAttributeOfType<TOut>(this Enum enumVal) where TOut : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(TOut), false);
            return (attributes.Length > 0) ? (TOut)attributes[0] : null;
        }
    }
}