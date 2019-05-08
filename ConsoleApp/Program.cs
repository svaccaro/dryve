using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using InstrumentControlLibrary.Dryve;

namespace TestingConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            // Igus Dryve Testing
            string dryveIP = ConfigurationManager.AppSettings.Get("DryveIP");
            int dryvePort = int.Parse(ConfigurationManager.AppSettings.Get("DryvePort"));
            UInt32 dryveFeed = UInt32.Parse(ConfigurationManager.AppSettings.Get("DryveFeed"));
            UInt32 dryveShaftRevs = UInt32.Parse(ConfigurationManager.AppSettings.Get("DryveShaftRevolutions"));

            IgusDryve infeedDryve = new IgusDryve(dryveIP, dryvePort);

            infeedDryve.Initialize(dryveFeed, dryveShaftRevs);


            Console.WriteLine(infeedDryve.ToString());
            Console.WriteLine(infeedDryve.Status.ToString());

            //Homing Run
            infeedDryve.ModeOfOperation = DryveModeOfOperation.Mode_Homing;
            Console.WriteLine("\nHoming...\n");
            infeedDryve.HomingSpeed_SearchSwitch = 5;
            infeedDryve.HomingSpeed_SearchZero = 10;
            infeedDryve.HomingAcceleration = 2;
            infeedDryve.Home();
            Thread.Sleep(5000);

            Console.WriteLine(infeedDryve.ToString());
            Console.WriteLine(infeedDryve.Status.ToString());

            //Params for profile position
            infeedDryve.ModeOfOperation = DryveModeOfOperation.Mode_ProfilePosition;

            infeedDryve.ProfileVelocity = 10;
            infeedDryve.ProfileAcceleration = 10;
            infeedDryve.ProfileDeceleration = 2;
            infeedDryve.TargetVelocity = 15;
            infeedDryve.TargetPosition = 10;

            Console.WriteLine("\nMoving to position in profile position mode...\n");
            if(infeedDryve.Position != infeedDryve.TargetPosition)
                infeedDryve.Move();
            Thread.Sleep(5000);
            //infeedDryve.Move();

            Console.WriteLine(infeedDryve.ToString());
            Console.WriteLine(infeedDryve.Status.ToString());

        }
    }
}
