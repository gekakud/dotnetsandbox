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
            list.AddToEnd(new Node(5));
            list.AddToEnd(new Node(6));
            list.AddToStart(new Node(0));

            list.RemoveOdds();

            list.ReverseList();
            list.PrintList();

            Console.ReadKey();
        }
    }

    class OneDirList:ILinkedList
    {
        private Node _head;

        public void ReverseList()
        {
            Node curNode, nextNode, prevNode;

            nextNode = null;
            curNode = _head;
            prevNode = null;

            while (curNode!=null)
            {
                nextNode = curNode.NextNode;
                curNode.NextNode = prevNode;
                prevNode = curNode;
                curNode = nextNode;
            }

            _head = prevNode;
        }

        public void RemoveOdds()
        {
            Node cur = _head;
            Node nextToAppend = cur.NextNode.NextNode;

            while (nextToAppend.NextNode!=null)
            {
               
                cur.NextNode = nextToAppend;
                cur = cur.NextNode;
                nextToAppend = nextToAppend.NextNode.NextNode;
            }

            cur.NextNode = nextToAppend;
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
        void ReverseList();
        void RemoveOdds();
        void AddToStart(Node node);
        void AddToEnd(Node node);
        void PrintList();
    }
}
