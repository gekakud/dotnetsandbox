using System;
using System.Collections;

namespace ToRemove
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            char[] charsArray = new[] {'a', 'b', 'c'};

            // char c = 'X';
            // byte[] bytes = BitConverter.GetBytes(c);
            // BitArray bits = new BitArray(bytes);

            Console.WriteLine(sizeof(short));
            foreach (var c1 in charsArray)
            {
                byte[] charBytes = BitConverter.GetBytes(c1);
                
                BitArray arrayOfBits = new BitArray(charBytes);

                short cnt = 0;
                foreach (bool arrayOfBit in arrayOfBits)
                {
                    if (arrayOfBit.Equals(true))
                    {
                        cnt++;
                    }
                }
                
                if (cnt == 3)
                {
                    Console.WriteLine("Hoorey for "+ c1);
                }
                
                var tt = countSetBits(c1);
            }
            
            Console.ReadKey();
        }
        
        static int countSetBits(int n) 
        {
            // 0000 0101  =>
            // 0000 0010
            int count = 0; 
            while (n > 0) { 
                count += n & 1; 
                n >>= 1; 
            } 
            return count; 
        } 
    }
}