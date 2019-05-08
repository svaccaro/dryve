using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using InstrumentControlLibrary;
using InstrumentControlLibrary.Comm;

namespace InstrumentControlLibrary.Dryve
{
    public class IgusDryve
    {
        //private UInt16 _txnId = 0;
        private TcpPort _client;
        private string _ip;
        private int _port;
        private DryveMessageHandler _msgHandler;
        public byte[] TestStatus;

        // constructor
        public IgusDryve(string ip, int port)
        {
            _ip = ip;
            _port = port;
            _client = new TcpPort(ip, port);
            _msgHandler = new DryveMessageHandler(_client);
        }

        public DryveStatus Status
        {
            get { return _msgHandler.GetStatus(); }
        }

        public DryveModeOfOperation ModeOfOperation
        {
            get { return _msgHandler.GetModeOfOperation(); }
            set { _msgHandler.SetModeOfOperation(value); }
        }

        public string SupportedModes
        {
            get { return _msgHandler.GetSupportedModes(); }
        }

        public double Position
        {
            get { return _msgHandler.GetPositionActual(); }
        }

        public double PositionWindow
        {
            get { return _msgHandler.GetPositionWindow(); }
        }

        public UInt16 PositionWindowTime
        {
            get { return _msgHandler.GetPositionWindowTime(); }
        }

        public double Velocity
        {
            get { return _msgHandler.GetVelocityActual(); }
        }

        public double TargetPosition
        {
            get { return _msgHandler.GetTargetPosition(); }
            set { _msgHandler.SetTargetPosition(value); }
        }

        public double HomeOffset
        {
            get { return _msgHandler.GetHomeOffset(); }
        }

        public double ProfileVelocity
        {
            get { return _msgHandler.GetProfileVelocity(); }
            set { _msgHandler.SetProfileVelocity(value); }
        }

        public double ProfileAcceleration
        {
            get { return _msgHandler.GetProfileAcceleration(); }
            set { _msgHandler.SetProfileAcceleration(value); }
        }

        public double ProfileDeceleration
        {
            get { return _msgHandler.GetProfileDeceleration(); }
            set { _msgHandler.SetProfileDeceleration(value); }
        }

        public UInt32 FeedConstant_Feed
        {
            get { return _msgHandler.GetFeedConstant_Feed(); }
            set { _msgHandler.SetFeedConstant_Feed(value); }
        }

        public UInt32 FeedConstant_ShaftRevolutions
        {
            get { return _msgHandler.GetFeedConstant_ShaftRevolutions(); }
            set { _msgHandler.SetFeedConstant_ShaftRevolutions(value); }
        }

        public DryveHomingMethod HomingMethod
        {
            get { return _msgHandler.GetHomingMethod(); }
        }

        public double HomingSpeed_SearchSwitch
        {
            get { return _msgHandler.GetHomingSpeeds_SearchSwitch(); }
            set { _msgHandler.SetHomingSpeeds_SearchSwitch(value); }
        }

        public double HomingSpeed_SearchZero
        {
            get { return _msgHandler.GetHomingSpeeds_SearchZero(); }
            set { _msgHandler.SetHomingSpeeds_SearchZero(value); }
        }

        public double HomingAcceleration
        {
            get { return _msgHandler.GetHomingAcceleration(); }
            set { _msgHandler.SetHomingAcceleration(value); }
        }

        public string DigitalInputs
        {
            get { return _msgHandler.GetDigitalInputs(); }
        }

        public string DigitalOutputs_PhysicalOutputs
        {
            get { return _msgHandler.GetDigitalOutputs_PhysicalOutputs(); }
        }

        public string DigitalOutputs_BitMask
        {
            get { return _msgHandler.GetDigitalOutputs_BitMask(); }
        }

        public double TargetVelocity
        {
            get { return _msgHandler.GetTargetVelocity(); }
            set { _msgHandler.SetTargetVelocity(value); }
        }

        public string PositionDemand
        {
            get { return _msgHandler.GetPositionDemand(); }
        }


        public void Initialize(UInt32 feed, UInt32 shaftRevolutions)
        {
            var result = _msgHandler.EnableSwitchOn();
            result = _msgHandler.EnableOperation();
            SetFeedConstant_Feed(feed);
            SetFeedConstant_ShaftRevolutions(shaftRevolutions);
        }
        public void SetModeOfOperation(DryveModeOfOperation modeName)
        {
            var result = _msgHandler.SetModeOfOperation(modeName);
        }

        public void SetFeedConstant_Feed(UInt32 feed)
        {
            var result = _msgHandler.SetFeedConstant_Feed(feed);
        }

        public void SetFeedConstant_ShaftRevolutions(UInt32 shaftRevolutions)
        {
            var result = _msgHandler.SetFeedConstant_ShaftRevolutions(shaftRevolutions);
        }

        public void SetTargetPosition(double targetPosition)
        {
            Int32 targetPositionInt = Convert.ToInt32(targetPosition * 100);
            var result = _msgHandler.SetTargetPosition(targetPositionInt);
        }

        public void Move()
        {
            var result = _msgHandler.Move();
        }

        public void Home()
        {
            var result = _msgHandler.Home();
        }

        public void SwitchOnDO()
        {
            var result = _msgHandler.SwitchOnDO();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Igus Dryve connected on Port" + _port + " IP " + _ip);
            sb.AppendLine("\tMode Of Operation: " + ModeOfOperation);
            sb.AppendLine("\tCurrent Position: " + Position);
            sb.AppendLine("\tTarget Position: " + TargetPosition);
            sb.AppendLine("\tPosition Window: " + PositionWindow);
            sb.AppendLine("\tPosition Window Time: " + PositionWindowTime);
            sb.AppendLine("\tCurrent Velocity: " + Velocity);
            sb.AppendLine("\tHome Offset: " + HomeOffset);
            sb.AppendLine("\tProfile Velocity: " + ProfileVelocity);
            sb.AppendLine("\tProfile Acceleration: " + ProfileAcceleration);
            sb.AppendLine("\tProfile Deceleration: " + ProfileDeceleration);
            sb.AppendLine("\tFeed: " + FeedConstant_Feed);
            sb.AppendLine("\tShaft Revolutions: " + FeedConstant_ShaftRevolutions);
            sb.AppendLine("\tHoming Method: " + HomingMethod);
            sb.AppendLine("\tHoming Speed - Search for Switch: " + HomingSpeed_SearchSwitch);
            sb.AppendLine("\tHoming Speed - Search for Zero: " + HomingSpeed_SearchZero);
            sb.AppendLine("\tHoming Acceleration: " + HomingAcceleration);
            sb.AppendLine("\tDigital Inputs: " + DigitalInputs);
            sb.AppendLine("\tDigital Outputs - Physical Outputs: " + DigitalOutputs_PhysicalOutputs);
            sb.AppendLine("\tDigital Outputs - Bit Mask: " + DigitalOutputs_BitMask);
            sb.AppendLine("\tTarget Velocity: " + TargetVelocity);

            return sb.ToString();
        }
    }
}
