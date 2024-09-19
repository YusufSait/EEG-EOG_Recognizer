using System;

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;

using NeuroSky.ThinkGear;
using NeuroSky.ThinkGear.Algorithms;


/*
 *Deals with ThinkGear Head Set connecention.
 *Recieves data.
 *Notfies oservers.
 */
class TGConnection : SignalSource
{
    private Connector connector;
    private String comPortName;
    private Connector.DeviceEventArgs de;
    private short dotum;    //State of signal source


    public TGConnection(String comPortName)
    {
        this.comPortName = comPortName;
        // Initialize a new Connector and add event handlers
        connector = new Connector();
        connector.DeviceConnected += new EventHandler(OnDeviceConnected);
        connector.DeviceConnectFail += new EventHandler(OnDeviceFail);
        connector.DeviceValidating += new EventHandler(OnDeviceValidating);
        connect(true);
    }

    public TGConnection()
    {
        comPortName = "COM3";
        // Initialize a new Connector and add event handlers
        connector = new Connector();
        connector.DeviceConnected += new EventHandler(OnDeviceConnected);
        connector.DeviceConnectFail += new EventHandler(OnDeviceFail);
        connector.DeviceValidating += new EventHandler(OnDeviceValidating);
        connect(true);
    }


    public override void start()
    {
        //TODO: Handle null pointer exception. (Occures when trying to start while no device connected)
        de.Device.DataReceived += new EventHandler(OnDataReceived);
    }

    public override void stop()
    {
        de.Device.DataReceived -= new EventHandler(OnDataReceived);
    }


    private void connect(bool status)
    {
        if (status)
        {
            connector.ConnectScan(comPortName);// Scan for devices across COM ports. Parameter is first one to check.
        }
        else
        {
            try
            {
                connector.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Already disconnected!\n" + e.Message);
            }

        }
    }

    public void setBlink(bool status)
    {
        // Blink detection needs to be manually turned on
        connector.setBlinkDetectionEnabled(status);
    }

    private void stopAfterTime(short secondsToRun)
    {
        Thread.Sleep(secondsToRun * 1000);
        System.Console.WriteLine("Closing...");
        connector.Close();
        Environment.Exit(0);
    }

    private void OnDataReceived(object sender, EventArgs e)
    {

        Device.DataEventArgs de = (Device.DataEventArgs)e;
        DataRow[] tempDataRowArray = de.DataRowArray;

        TGParser tgParser = new TGParser();
        tgParser.Read(de.DataRowArray);

        // Loops through the newly parsed data of the connected headset
        for (ushort i = 0; i < tgParser.ParsedData.Length; i++)
        {

            if (tgParser.ParsedData[i].ContainsKey("Raw"))
            {
                dotum = (short)tgParser.ParsedData[i]["Raw"];
                feedAll(dotum);
                //Console.WriteLine("Raw Value:" + dotum);
            }

            if (tgParser.ParsedData[i].ContainsKey("BlinkStrength"))
            {
                Console.WriteLine("Eyeblink ---------------------" + tgParser.ParsedData[i]["BlinkStrength"]);
            }
        }
    }

    // Called when a device is connected 
    private void OnDeviceConnected(object sender, EventArgs e)
    {
        de = (Connector.DeviceEventArgs)e;
        Console.WriteLine("Device found on: " + de.Device.PortName);
        start();
    }

    // Called when scanning fails
    private void OnDeviceFail(object sender, EventArgs e)
    {
        Console.WriteLine("No devices found! :(");
    }

    // Called when each port is being validated
    private void OnDeviceValidating(object sender, EventArgs e)
    {
        Console.WriteLine("Validating: ");
    }

}
