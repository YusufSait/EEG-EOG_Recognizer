using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading;

class SourceText : SignalSource
{
    private string filePath;
    private StreamReader reader;
    private ManualResetEvent run;
    public SourceText(string path)
    {
        run = new ManualResetEvent(false);

        filePath = path;
        try
        {
            reader = new StreamReader(path);
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot reach to source file! " + e.Message);
        }
    }

    public override void start()
    {
        run.Reset();
        short dotum;
        string line;
        try
        {
            while ((line = reader.ReadLine()) != null)
            {
                dotum = (short.Parse(line));
                feedAll(dotum);
                Thread.Sleep(4);
            }
        }
        catch (FormatException e)
        {
            Console.WriteLine("Invalid value on file " + filePath + "!" + e.Message);
            return;
        }
    }
    
    public override void stop()
    {
        //No need for this method only on SourceText
        return;
    }

    public void write(short val)
    {
        reader.ReadLine();
    }

    public void close()
    {
        reader.Close();
    }
}
