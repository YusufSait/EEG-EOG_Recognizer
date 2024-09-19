using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Observer interface to the SignalSorce
 */
interface SignalListener
{
    void recieveData(short dotum);
    
SignalSource Source
    {
        get;
        set;
    }

    //private SignalSource source;
}
