using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CircularBuffer
{
    protected List<short> buffer;
    protected int size;
    protected int head;
    protected int next;
    private int tNext;

    public CircularBuffer(int size)
    {
        initBuffer(size);
        head = -1;
        next = 0;
    }

    public void add(short val)
    {
        buffer.Add(val);
        ++size;
        ++head;
    }

    public virtual void addToNext(short val)
    {
        head = (head + 1) % size;
        next = (head + 1) % size;
        buffer[head] = val;
    }

    public short getVal(int index)
    {
        return buffer[index];
    }

    public short getNext()
    {
        tNext = next;
        next = (next + 1) % size;
        return buffer[tNext];
    }

    public void display()
    {
        int c = 0;
        int ind = (head + 1) % size;
        while (c < size)
        {
            Console.Write(buffer[ind] + " ");
            ind = (ind + 1) % size;
            ++c;
        }
        Console.WriteLine();
    }

    public int Size
    {
        get { return size; }
        set
        {
            if (value >= 0)
                size = value;
            else
                Console.WriteLine("Size didn't get changed!");
        }
    }

    private void initBuffer(int size)
    {
        if (size >= 0)
            Size = size;
        else
            Size = 0;

        head = -1;
        buffer = new List<short>();

        for (int i = 0; i < size; i++)
            buffer.Add(0);
    }
}