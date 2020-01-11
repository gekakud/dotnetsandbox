using System;
using System.Collections.Generic;

namespace EqualityAndGetHashCode
{
    // The GetHashCode method provides this hash code for algorithms that need quick checks
    // of object equality. A hash code is a numeric value that is used to insert and identify
    // an object in a hash-based collection such as the Dictionary<TKey, TValue> class,
    // the Hashtable class, or a type derived from the DictionaryBase class.

    //Two objects that are equal return hash codes that are equal.However, the reverse is not true:
    //equal hash codes do not imply object equality, because different(unequal) objects can have identical hash codes.

    // Actually obj1 and obj2 are different objects, but:

    // 1- when both Equals and GetHashCode overriden with our logic we get obj1 and obj2 are equal
    // so we will get Exception trying to insert obj2

    // 2- if we comment both methods we will be able to insert both to Dictionary
    // because obj1 and obj2 are not equal

    // 3- if we override only one of methods it violates the rules and result of methods is not consistent
    // and can not be applicable for equality

    class Program
    {
        static void Main(string[] args)
        {
            //will override Equals() and GetHashCode()
            var obj1 = new AllowedItem("A-Key", "A-Value", true);
            var obj2 = new AllowedItem("A-Key", "A-Value", true);

            var dic = new Dictionary<AllowedItem, string>();
            dic.Add(obj1, "obj1");

            //throws exception because two objects are equal according to our implementation
            //If hash codes of 2 objects are same, it uses Equals Method to check if there are same of not
            dic.Add(obj2, "obj2");

            Console.ReadKey();
        }
    }

    public class AllowedItem
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public bool IsAllowed { get; private set; }

        public AllowedItem(string name, string value, bool isAllowed)
        {
            Name = name;
            Value = value;
            IsAllowed = isAllowed;
        }

        public override bool Equals(object obj)
        {
            if (obj is AllowedItem other)
            {
                if (Name == other.Name && Value == other.Value && IsAllowed == other.IsAllowed)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^
                   Value.GetHashCode() ^
                   IsAllowed.GetHashCode();
        }
    }
}
