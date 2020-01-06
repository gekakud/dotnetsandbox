namespace LinkedList
{
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