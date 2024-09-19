using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gui;

//public interface listener

class Correlator : CircularBuffer, SignalListener
{
    private ChartPlotter plotter;
    private SignalSource source;
    private CircularBuffer searchPattern;
    private bool record;

    private int cInd = 0;
    private int sum;
    private int seqSum1;
    private int seqSum2;
    private int val;
    private double xcor = 0;
    private double variance = 0;
    private double lastCor = 0;

    

    public Correlator(int size)
        : base(size)
    {
        searchPattern = new CircularBuffer(size);
        record = false;

    }

    public override void addToNext(short dotum)
    {
        base.addToNext(dotum);
        /*
        if (max < dotum)
            max = dotum;
        if (maxIndex == srcMaxIndex)
            display();
         * */
        //unutma
        //TODO: hashing ile aramaları azalt
        if(base.buffer.Count>0)
            display();
    }

    public void display()
    {
        lastCor = correlate();
        Console.WriteLine("Correlation: " + lastCor);
        if (plotter != null)
            plotter.LastCorData = lastCor;
    }

    /*
     * Normalized cross correlation with "buffer" and "searchPattern".
     * correlation=sum(xi*yi)/[sqrt(sum(xi^2)*sum(yi^2)]
     */
    private double correlate()
    {
        cInd = (head + 1) % size;
        val = 0;
        sum = 0;
        seqSum1 = 0;
        seqSum2 = 0;
        xcor = 0;
        variance = 0;

        try
        {
            for (int i = 0; i < searchPattern.Size; i++)
            {
                val = searchPattern.getNext();
                sum += val * buffer[cInd];
                seqSum1 += (int)(val * val);
                seqSum2 += (int)(buffer[cInd] * buffer[cInd]);
                cInd = (cInd + 1) % size;
            }
            variance = Math.Sqrt((double)seqSum1 * (double)seqSum2) + 0.000000000000001;    //'+1' is for preventing division by 0
            xcor = sum / (variance);
            return xcor;
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            Console.Error.WriteLine("Buffer and pattern sizes aren't equal! \n" + e.Message);
            return 0;
        }
    }

    public ChartPlotter Plotter
    {
        set { plotter = value; }
    }

    public double Xcor
    {
        get { return xcor; }
    }

    private int patternSize = 0;
    private short max = -2048;
    void SignalListener.recieveData(short dotum)
    {
        if (!record)
        {
            addToNext(dotum);
        }
        else
        {
            ++patternSize;
            searchPattern.add(dotum);
        }
    }

    public SignalSource Source
    {
        get { return source; }
        set { source = value; }
    }

    public bool Record
    {
        get { return record; }
        set
        {
            record = value;

                if (record)
                {
                    searchPattern = new CircularBuffer(1);
                }
                else
                {   //Clear and reinitialize buffer.
                    this.Size = searchPattern.Size;
                    buffer = new List<short>();

                    for (int i = 0; i < size; i++)
                        buffer.Add(0);
                    head = -1;
                    next = 0;
                }
        }
    }
}
