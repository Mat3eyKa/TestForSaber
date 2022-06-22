using System.Text.RegularExpressions;

class ListNode
{
    public ListNode Previous;
    public ListNode Next;
    public ListNode Random; // произвольный элемент внутри списка
    public string Data;
}


class ListRandom
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public void Serialize(Stream s)
    {
        if (!s.CanWrite)
        {
            throw new IOException("Stream does not support writing");
        }

        Dictionary<ListNode, int> dictionary = new Dictionary<ListNode, int>();
        int i = 0;
        for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
        {
            dictionary.Add(currentNode, i);
            ++i;
        }

        var list = Head;
        using StreamWriter sw = new StreamWriter(s);
        while (list != null)
        {
            sw.WriteLine($"<Data = {list.Data}, RandomIndex = {dictionary[list.Random]}>");
            list = list.Next;
        }
    }

    public void Deserialize(Stream s)
    {
        Dictionary<int, ListNode> dictionary = new Dictionary<int, ListNode>();
        using StreamReader sr = new StreamReader(s);
        string line;
        int i = 0;
        ListNode list = Head;
        while ((line = sr.ReadLine()) != null)
        {
            Match match = Regex.Match(line, @"<Data = (.*), RandomIndex = (\d*)>");
            Add(match.Groups[1].Value);
            dictionary.Add(i, Tail);
            _ = int.TryParse(match.Groups[2].Value, out int rand);
            list.Random = dictionary[rand];
            list = list.Next;
            i++;
        }
    }
    public void Add(string data)
    {
        ListNode node = new();
        node.Data = data;
        if (Head == null)
        {
            Head = node;
        }
        else
        {
            Tail.Next = node;
            node.Previous = Tail;
        }
            Tail = node;
            Count++;
    }
}

