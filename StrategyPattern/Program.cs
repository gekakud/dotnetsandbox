using System;
using System.Collections.Generic;

namespace StrategyPattern
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var contacts = new List<CreditCard>();

            contacts = CreateCardsList();
            ValidateCards(contacts);
        
            Console.ReadKey();
        }

        public static void ValidateCards(List<CreditCard> p_cardList)
        {
            foreach (var card in p_cardList)
            {
                Console.WriteLine(card.Number+" is valid:"+card.IsValid());
            }
        }

        public static List<CreditCard> CreateCardsList()
        {
            var cardList = new List<CreditCard>();
            
            cardList.Add(new CreditCard(CardType.Visa)
            {
                Cvv = 234,Name = "Vasya",Number = 458001124422
            });

            cardList.Add(new CreditCard(CardType.Visa)
            {
                Cvv = 234,
                Name = "Kolya",
                Number = 418001124422
            });

            cardList.Add(new CreditCard(CardType.MasterCard)
            {
                Cvv = 231,
                Name = "Petya",
                Number = 314021111229
            });

            cardList.Add(new CreditCard(CardType.AmericanExpress)
            {
                Cvv = 231,
                Name = "Vova",
                Number = 556621111229
            });

            return cardList;
        }
    }
}
