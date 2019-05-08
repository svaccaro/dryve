using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using InstrumentControlLibrary.Comm;

namespace InstrumentControlLibrary.Dryve
{
    class DryveMessageHandler
    {
        private UInt16 _txnId = 0;
        private TcpPort _client;

        public Dictionary<DryveModeOfOperation, short> ModeDict = new Dictionary<DryveModeOfOperation, short>
        {
            {DryveModeOfOperation.Mode_ProfilePosition, 1},
            {DryveModeOfOperation.Mode_ProfileVelocity, 3},
            {DryveModeOfOperation.Mode_Homing, 6}
        };

        public Dictionary<DryveHomingMethod, short> HomingMethodDict = new Dictionary<DryveHomingMethod, short>
        {
            {DryveHomingMethod.Method0, 0},
            {DryveHomingMethod.Method1, 1},
            {DryveHomingMethod.Method2, 2},
            {DryveHomingMethod.Method3, 3},
            {DryveHomingMethod.Method4, 4}
        };

        public DryveMessageHandler(TcpPort client)
        {
            _client = client;
        }

        public DryveStatus GetStatus()
        {
            DryveModeOfOperation modeOfOperation = GetModeOfOperation();
            byte[] result = SendAndReceive(DryveObject.Statusword);
            DryveStatus dryveStatus = new DryveStatus(ParseReadData(result), modeOfOperation);
            return dryveStatus;
        }

        public DryveModeOfOperation GetModeOfOperation()
        {
            byte[] result = SendAndReceive(DryveObject.ModesOfOperationDisplay);
            string modeVal = ParseReadData(result, ResultFormat.Integer);
            DryveModeOfOperation modeOfOperation = ModeDict.FirstOrDefault(x => x.Value == short.Parse(modeVal)).Key;
            return modeOfOperation;
        }

        public byte[] SetModeOfOperation(DryveModeOfOperation modeOfOperation)
        {
            short modeByte = ModeDict[modeOfOperation];
            byte[] modeBytes = new byte[1] { (byte)modeByte };
            var result = SendAndReceive(messageType: DryveObject.ModesOfOperation, accessType: ObjectAccess.Write, dataValue: modeBytes);
            return result;
        }

        public double GetPositionActual()
        {
            byte[] result = SendAndReceive(DryveObject.PositionActual, format:ResultFormat.Integer);
            double positionActual = Int32.Parse(ParseReadData(result, format: ResultFormat.Int32)) / 100.0;
            return positionActual;
        }

        public double GetPositionWindow()
        {
            byte[] result = SendAndReceive(DryveObject.PositionWindow, format: ResultFormat.Integer);
            double positionWindow = Int32.Parse(ParseReadData(result, format: ResultFormat.Int32)) / 100.0;
            return positionWindow;
        }

        public UInt16 GetPositionWindowTime()
        {
            byte[] result = SendAndReceive(DryveObject.PositionWindowTime, format: ResultFormat.Integer);
            UInt16 PositionWindowTime = UInt16.Parse(ParseReadData(result, format: ResultFormat.UInt16));
            return PositionWindowTime;
        }

        public double GetVelocityActual()
        {
            byte[] result = SendAndReceive(DryveObject.VelocityActual, format: ResultFormat.Integer);
            double velocityActual = Int32.Parse(ParseReadData(result, format: ResultFormat.Int32)) / 100.0;
            return velocityActual;
        }

        public double GetTargetPosition()
        {
            byte[] result = SendAndReceive(DryveObject.TargetPosition, format: ResultFormat.Integer);
            double targetPosition = Int32.Parse(ParseReadData(result, format: ResultFormat.Int32)) / 100.0;
            return targetPosition;
        }

        public byte[] SetTargetPosition(double targetPosition)
        {
            Int32 targetPositionInt = Convert.ToInt32(targetPosition * 100);
            byte[] targetPositionBytes = BitConverter.GetBytes(targetPositionInt);
            var result = SendAndReceive(messageType: DryveObject.TargetPosition, accessType: ObjectAccess.Write, dataValue: targetPositionBytes);
            return result;
        }

        public double GetHomeOffset()
        {
            byte[] result = SendAndReceive(DryveObject.HomeOffset, format: ResultFormat.Integer);
            double homeOffset = Int32.Parse(ParseReadData(result, format: ResultFormat.Int32)) / 100.0;
            return homeOffset;
        }

        public double GetProfileVelocity()
        {
            byte[] result = SendAndReceive(DryveObject.ProfileVelocity, format: ResultFormat.Integer);
            double profileVelocity = UInt32.Parse(ParseReadData(result, format:ResultFormat.UInt32)) / 100.0;
            return profileVelocity;
        }

        public byte[] SetProfileVelocity(double profileVelocity)
        {
            UInt32 profileVelocityInt = Convert.ToUInt32(profileVelocity * 100);
            byte[] profileVelocityBytes = BitConverter.GetBytes(profileVelocityInt);
            var result = SendAndReceive(messageType: DryveObject.ProfileVelocity, accessType: ObjectAccess.Write, dataValue: profileVelocityBytes);
            return result;
        }

        public double GetProfileAcceleration()
        {
            byte[] result = SendAndReceive(DryveObject.ProfileAcceleration, format: ResultFormat.Integer);
            double profileAcceleration = UInt32.Parse(ParseReadData(result, format: ResultFormat.UInt32)) / 100;
            return profileAcceleration;
        }

        public byte[] SetProfileAcceleration(double profileAcceleration)
        {
            UInt32 profileAccelerationInt = Convert.ToUInt32(profileAcceleration * 100);
            byte[] profileAccelerationBytes = BitConverter.GetBytes(profileAccelerationInt);
            var result = SendAndReceive(messageType: DryveObject.ProfileAcceleration, accessType: ObjectAccess.Write, dataValue: profileAccelerationBytes);
            return result;
        }

        public double GetProfileDeceleration()
        {
            byte[] result = SendAndReceive(DryveObject.ProfileDeceleration, format: ResultFormat.Integer);
            double profileDeceleration = UInt32.Parse(ParseReadData(result, format: ResultFormat.UInt32)) / 100;
            return profileDeceleration;
        }

        public byte[] SetProfileDeceleration(double profileDeceleration)
        {
            UInt32 profileDecelerationInt = Convert.ToUInt32(profileDeceleration * 100);
            byte[] profileDecelerationBytes = BitConverter.GetBytes(profileDecelerationInt);
            var result = SendAndReceive(messageType: DryveObject.ProfileDeceleration, accessType: ObjectAccess.Write, dataValue: profileDecelerationBytes);
            return result;
        }

        public UInt32 GetFeedConstant_Feed()
        {
            byte[] result = SendAndReceive(messageType: DryveObject.FeedConstant_Feed, format: ResultFormat.Integer);
            UInt32 feedConstant_Feed = UInt32.Parse(ParseReadData(result, format: ResultFormat.UInt32));
            return feedConstant_Feed;
        }

        public byte[] SetFeedConstant_Feed(UInt32 feed)
        {
            byte[] feedBytes = BitConverter.GetBytes(feed);
            byte[] result = SendAndReceive(messageType: DryveObject.FeedConstant_Feed, accessType: ObjectAccess.Write, dataValue: feedBytes);
            return result;
        }

        public UInt32 GetFeedConstant_ShaftRevolutions()
        {
            byte[] result = SendAndReceive(messageType: DryveObject.FeedConstant_ShaftRevolutions, format: ResultFormat.Integer);
            UInt32 feedConstant_ShaftRevolutions = UInt32.Parse(ParseReadData(result, format: ResultFormat.UInt32));
            return feedConstant_ShaftRevolutions;
        }

        public byte[] SetFeedConstant_ShaftRevolutions(UInt32 shaftRevolutions)
        {
            byte[] shaftRevolutionsBytes = BitConverter.GetBytes(shaftRevolutions);
            byte[] result = SendAndReceive(messageType: DryveObject.FeedConstant_ShaftRevolutions, accessType: ObjectAccess.Write, dataValue: shaftRevolutionsBytes);
            return result;
        }

        public DryveHomingMethod GetHomingMethod()
        {
            byte[] result = SendAndReceive(DryveObject.HomingMethod);
            string homingMethodVal = ParseReadData(result, ResultFormat.Integer);
            DryveHomingMethod homingMethod = HomingMethodDict.FirstOrDefault(x => x.Value == short.Parse(homingMethodVal)).Key;
            return homingMethod;
        }

        public double GetHomingSpeeds_SearchSwitch()
        {
            byte[] result = SendAndReceive(DryveObject.HomingSpeeds_SearchSwitch);
            double homingSpeeds_SearchSwitch = UInt32.Parse(ParseReadData(result, format: ResultFormat.UInt32)) / 100;
            return homingSpeeds_SearchSwitch;
        }

        public byte[] SetHomingSpeeds_SearchSwitch(double homingSpeedSwitch)
        {
            UInt32 homingSpeedSwitchInt = Convert.ToUInt32(homingSpeedSwitch * 100);
            byte[] homingSpeedSwitchBytes = BitConverter.GetBytes(homingSpeedSwitchInt);
            byte[] result = SendAndReceive(DryveObject.HomingSpeeds_SearchSwitch, accessType: ObjectAccess.Write, dataValue: homingSpeedSwitchBytes);
            return result;
        }

        public double GetHomingSpeeds_SearchZero()
        {
            byte[] result = SendAndReceive(DryveObject.HomingSpeeds_SearchZero);
            double homingSpeed_SearchZero = UInt32.Parse(ParseReadData(result, format: ResultFormat.UInt32)) / 100;
            return homingSpeed_SearchZero;
        }

        public byte[] SetHomingSpeeds_SearchZero(double homingSpeedZero)
        {
            UInt32 homingSpeedZeroInt = Convert.ToUInt32(homingSpeedZero * 100);
            byte[] homingSpeedZeroBytes = BitConverter.GetBytes(homingSpeedZeroInt);
            byte[] result = SendAndReceive(DryveObject.HomingSpeeds_SearchZero, accessType: ObjectAccess.Write, dataValue: homingSpeedZeroBytes);
            return result;
        }

        public double GetHomingAcceleration()
        {
            byte[] result = SendAndReceive(DryveObject.HomingAcceleration);
            double homingAcceleration = UInt32.Parse(ParseReadData(result, format: ResultFormat.UInt32)) / 100;
            return homingAcceleration;
        }

        public byte[] SetHomingAcceleration(double homingAcceleration)
        {
            UInt32 homingAccelerationInt = Convert.ToUInt32(homingAcceleration * 100);
            byte[] homingAccelerationBytes = BitConverter.GetBytes(homingAccelerationInt);
            byte[] result = SendAndReceive(DryveObject.HomingAcceleration, accessType: ObjectAccess.Write, dataValue: homingAccelerationBytes);
            return result;
        }

        public string GetDigitalInputs()
        {
            byte[] result = SendAndReceive(DryveObject.DigitalInputs);
            string digitalInputs = ParseReadData(result);
            return digitalInputs;
        }

        public string GetDigitalOutputs_PhysicalOutputs()
        {
            byte[] result = SendAndReceive(DryveObject.DigitalOutputs_PhysicalOutputs);
            string digitalOutputs_PhysicalOutputs = ParseReadData(result);
            return digitalOutputs_PhysicalOutputs;
        }

        public byte[] SetDigitalOutputs_PhysicalOutputs(UInt32 digitalOutputsPhys)
        {
            byte[] digitalOutputsPhysBytes = BitConverter.GetBytes(digitalOutputsPhys);
            byte[] result = SendAndReceive(DryveObject.DigitalOutputs_PhysicalOutputs, accessType: ObjectAccess.Write, dataValue: digitalOutputsPhysBytes);
            return result;
        }

        public string GetDigitalOutputs_BitMask()
        {
            byte[] result = SendAndReceive(DryveObject.DigitalOutputs_BitMask);
            string digitalOutputs_BitMask = ParseReadData(result);
            return digitalOutputs_BitMask;
        }

        public byte[] SetDigitalOutputs_BitMask(UInt32 digitalOutputsBitMask)
        {
            byte[] digitalOutputsBitMaskBytes = BitConverter.GetBytes(digitalOutputsBitMask);
            byte[] result = SendAndReceive(DryveObject.DigitalOutputs_BitMask, accessType: ObjectAccess.Write, dataValue: digitalOutputsBitMaskBytes);
            return result;
        }

        public double GetTargetVelocity()
        {
            byte[] result = SendAndReceive(DryveObject.TargetVelocity);
            double targetVelocity = Int32.Parse(ParseReadData(result, format: ResultFormat.Int32)) / 100;
            return targetVelocity;
        }

        public byte[] SetTargetVelocity(double targetVelocity)
        {
            Int32 targetVelocityInt = Convert.ToInt32(targetVelocity * 100);
            byte[] targetVelocityBytes = BitConverter.GetBytes(targetVelocityInt);
            byte[] result = SendAndReceive(messageType: DryveObject.TargetVelocity, accessType: ObjectAccess.Write, dataValue: targetVelocityBytes);
            return result;
        }

        public string GetSupportedModes()
        {
            byte[] result = SendAndReceive(DryveObject.SupportedModes);
            string supportedModes = ParseReadData(result);
            return supportedModes;
        }

        public string GetPositionDemand()
        {
            byte[] result = SendAndReceive(DryveObject.PositionDemand);
            string positionDemand = ParseReadData(result);
            return positionDemand;
        }

        public byte[] EnableSwitchOn()
        {
            byte[] enableCode = new byte[] { (byte)0b0110, 00 };
            var result = SendAndReceive(messageType: DryveObject.Controlword, accessType: ObjectAccess.Write, dataValue: enableCode);
            return result;
        }

        public byte[] EnableOperation()
        {
            byte[] enableCode = new byte[] { (byte)0b1111, 00 };
            var result = SendAndReceive(messageType:DryveObject.Controlword, accessType:ObjectAccess.Write, dataValue:enableCode);
            return result;  
        }

        public byte[] SwitchOnDO()
        {
            byte[] enableCode = new byte[] { (byte)1, 00, 00, 00};
            var result = SendAndReceive(messageType: DryveObject.DigitalOutputs_BitMask, accessType: ObjectAccess.Write, dataValue: enableCode);
            enableCode = new byte[] { 00, (byte)101, 00, 00 };
            result = SendAndReceive(messageType: DryveObject.DigitalOutputs_PhysicalOutputs, accessType: ObjectAccess.Write, dataValue: enableCode);
            return result;
        }

        public byte[] Home()
        {
            SetModeOfOperation(DryveModeOfOperation.Mode_Homing);
            byte[] homeBytes = new byte[] { (byte)0b00011111, (byte)0b00000000 };
            var result = SendAndReceive(messageType: DryveObject.Controlword, accessType: ObjectAccess.Write, dataValue: homeBytes);
            return result;
        }

        public byte[] Move()
        {
            SetModeOfOperation(DryveModeOfOperation.Mode_ProfilePosition);
            byte[] moveBytes = new byte[] { (byte)0b00011111, (byte)0b00000000};
            var result = SendAndReceive(messageType: DryveObject.Controlword, accessType: ObjectAccess.Write, dataValue: moveBytes);
            return result;
        }

        private byte[] SendAndReceive(DryveObject messageType, int wait = 50, ObjectAccess accessType = ObjectAccess.Read, byte[] dataValue = null, ResultFormat format = ResultFormat.Binary)
        {
            DryveMessage msg = new DryveMessage(_txnId, messageType, accessType, dataValue);
            byte[] message = msg.MessageBytes;
            _txnId++;

            _client.SendMessage(message);
            System.Threading.Thread.Sleep(wait);

            var msgLength = accessType == ObjectAccess.Read ? message.Length + (int)message[18] : 19 ;
            var result = _client.ReceiveMessage(msgLength);

            if (!result.Take(2).SequenceEqual(message.Take(2)))
            {
                throw new Exception("Communication error - Transaction ID mismatch.");
            }
            return result;
        }

        public string ParseReadData(byte[] message, ResultFormat format = ResultFormat.Binary)
        {
            string result = "";
            int byteCount = message[5] - 13;
            List<byte> dataBytes = new List<byte>(message);
            var data = dataBytes.Skip(Math.Max(0, dataBytes.Count() - byteCount)).ToArray();
            string[] binaryStringArray = new string[data.Length];
            if (format == ResultFormat.Integer)
            {
                if(byteCount == 4)
                    result = BitConverter.ToInt32(data, 0).ToString();
                else if (byteCount == 2)
                    result = BitConverter.ToInt16(data, 0).ToString();
                else if (byteCount == 1)
                    result = data[0].ToString();
                else
                    throw new Exception("Cannot parse result to integer.");
            }
            else if (format == ResultFormat.UInt16)
            {
                result = BitConverter.ToUInt16(data, 0).ToString();
            }
            else if (format == ResultFormat.UInt32)
            {
                result = BitConverter.ToUInt32(data, 0).ToString();
            }
            else if (format == ResultFormat.Int16)
            {
                result = BitConverter.ToInt16(data, 0).ToString();
            }
            else if (format == ResultFormat.Int32)
            {
                result = BitConverter.ToInt32(data, 0).ToString();
            }
            else if (format == ResultFormat.Binary)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    binaryStringArray[i] = Convert.ToString(data[i], 2).PadLeft(8, '0');
                }

                result = String.Join("", binaryStringArray.Reverse());
            }
            return result;
        }
    }
}
