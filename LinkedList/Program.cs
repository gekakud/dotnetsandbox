using System;

namespace LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new OneDirList();
            list.AddToEnd(new Node(1));
            list.AddToEnd(new Node(2));
            list.AddToEnd(new Node(3));
            list.AddToEnd(new Node(4));

            list.AddToStart(new Node(0));

            list.PrintList();

            Console.ReadKey();
        }
    }

    class OneDirList:ILinkedList
    {
        private Node _head;

        public void InsertAfter(Node node)
        {
            throw new NotImplementedException();
        }

        public void AddToStart(Node newNode)
        {
            if (_head == null)
            {
                _head = newNode;
                return;
            }

            newNode.NextNode = _head;
            _head = newNode;
        }

        public void AddToEnd(Node newNode)
        {
            if (_head == null)
            {
                _head = newNode;
                return;
            }

            Node lastNode = _head;
            while (lastNode.NextNode!=null)
            {
                lastNode = lastNode.NextNode;
            }

            lastNode.NextNode = newNode;
        }

        public void PrintList()
        {
            if (_head == null)
            {
                Console.WriteLine("empty");
                return;
            }
            if (_head.NextNode == null)
            {
                Console.WriteLine(_head.GetValue());
                return;
            }

            Node currentNode = _head;
            Console.WriteLine(currentNode.GetValue());
            do
            {
                currentNode = currentNode.NextNode;
                Console.WriteLine(currentNode.GetValue());
            } while (currentNode.NextNode != null);
        }
    }

    interface ILinkedList
    {
        void InsertAfter(Node node);
        void AddToStart(Node node);
        void AddToEnd(Node node);
        void PrintList();
    }

    class Node
    {
        public Node NextNode;

        private int Data;

        public Node(int newData)
        {
            Data = newData;
            NextNode = null;
        }

        public int GetValue()
        {
            return Data;
        }
    }
}
