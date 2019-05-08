using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentControlLibrary.Dryve
{
    public class DryveMessage
    {
        private DryveObject _dryveObject;
        private ObjectAccess _objectAccess;
        private UInt16 _txnId;
        private byte[] _dataValue;
        public byte[] MessageBytes;

        public DryveMessage(UInt16 txnId, DryveObject dryveObject, ObjectAccess objectAccess, byte[] dataValue = null)
        {
            _txnId = txnId;
            _dryveObject = dryveObject;
            _objectAccess = objectAccess;
            _dataValue = dataValue == null ? new byte[0] : dataValue;
            MessageBytes = ConstructMessage();
        }

        private byte[] ConstructMessage()
        {
            byte[] sdoObject;
            short subSdoObject = 0;
            short dataLength;
            ObjectAccess accessType;
            

            switch (_dryveObject)
            {
                case DryveObject.Controlword:
                    dataLength = 2;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x40 };
                    break;
                case DryveObject.Statusword:
                    dataLength = 2;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x41 };
                    break;
                case DryveObject.ModesOfOperation:
                    dataLength = 1;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x60 };
                    break;
                case DryveObject.ModesOfOperationDisplay:
                    dataLength = 1;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x61 };
                    break;
                case DryveObject.PositionActual:
                    dataLength = 4;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x64 };
                    break;
                case DryveObject.PositionWindow:
                    dataLength = 4;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x67 };
                    break;
                case DryveObject.PositionWindowTime:
                    dataLength = 2;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x68 };
                    break;
                case DryveObject.VelocityActual:
                    dataLength = 4;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x6C };
                    break;
                case DryveObject.TargetPosition:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x7A };
                    break;
                case DryveObject.HomeOffset:
                    dataLength = 4;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x7C };
                    break;
                case DryveObject.ProfileVelocity:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x81 };
                    break;
                case DryveObject.ProfileAcceleration:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x83 };
                    break;
                case DryveObject.ProfileDeceleration:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x84 };
                    break;
                case DryveObject.FeedConstant_Feed:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x92 };
                    subSdoObject = 1;
                    break;
                case DryveObject.FeedConstant_ShaftRevolutions:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x92 };
                    subSdoObject = 2;
                    break;
                case DryveObject.HomingMethod:
                    dataLength = 1;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x98 };
                    break;
                case DryveObject.HomingSpeeds_SearchSwitch:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x99 };
                    subSdoObject = 1;
                    break;
                case DryveObject.HomingSpeeds_SearchZero:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x99 };
                    subSdoObject = 2;
                    break;
                case DryveObject.HomingAcceleration:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0x9A };
                    break;
                case DryveObject.DigitalInputs:
                    dataLength = 4;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0xFD };
                    break;
                case DryveObject.DigitalOutputs_PhysicalOutputs:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0xFE };
                    subSdoObject = 1;
                    break;
                case DryveObject.DigitalOutputs_BitMask:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0xFE };
                    subSdoObject = 2;
                    break;
                case DryveObject.TargetVelocity:
                    dataLength = 4;
                    accessType = ObjectAccess.RWW;
                    sdoObject = new byte[] { 0x60, 0xFF };
                    break;
                case DryveObject.SupportedModes:
                    dataLength = 4;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x65, 0x02 };
                    break;
                case DryveObject.PositionDemand:
                    dataLength = 4;
                    accessType = ObjectAccess.RO;
                    sdoObject = new byte[] { 0x60, 0x63 };
                    break;
                default:
                    throw new Exception("Undefined Dryve message type.");
            }

            var sendLength = _objectAccess.Equals(ObjectAccess.Read) ? 13 : 13 + dataLength;
            int isWrite = _objectAccess.Equals(ObjectAccess.Write) ? 1 : 0;

            byte[] mbapHeaderBytes = { (byte)(_txnId >> 8), (byte)_txnId, 0, 0, 0, (byte)sendLength, 0xFF };
            byte[] functionBytes = {0x2B, 0x0D, (byte)isWrite, 0, 0, sdoObject[0], sdoObject[1], (byte)subSdoObject, 0, 0, 0, (byte)dataLength};
            byte[] sendDataBytes = FixEndian(_dataValue);

            var msg = new List<byte>();
            msg.AddRange(mbapHeaderBytes);
            msg.AddRange(functionBytes);
            msg.AddRange(sendDataBytes);
            byte[] message = msg.ToArray();

            return message;
        }

        private byte[] FixEndian(byte[] byteArray)
        {
            byte[] returnByteArray;
            if (BitConverter.IsLittleEndian)
            {
                returnByteArray = byteArray.ToArray();
            }
            else
            {
                returnByteArray = byteArray.Reverse().ToArray();
            }

            return returnByteArray;
        }

        public override string ToString()
        {
            return BitConverter.ToString(MessageBytes);
        }

    }
}
