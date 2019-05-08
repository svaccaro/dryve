using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InstrumentControlLibrary.Dryve
{
    public class DryveStatus
    {
        public bool ReadySwitchOn;
        public bool IsOn;
        public bool IsOpEnabled;
        public bool Fault;
        public bool VoltageEnabled;
        public bool QuickStop;
        public bool SwitchOnDiabled;
        public bool Warning;
        public bool MotorEnabled;
        public bool InternalLimitActive;
        public bool TargetReached;
        public bool? SetpointApplied;
        public bool? IsMoving;
        public bool? HomingAttained;
        private DryveModeOfOperation _modeOfOperation;
        private string _binaryStatus;

        public DryveStatus(string binaryStatus, DryveModeOfOperation opMode)
        {
            if (!Regex.IsMatch(binaryStatus, "^[01]{16}$"))
                throw new Exception("Invalid dryveStatus");
            bool[] statusArray = binaryStatus.Select(c => c == '1').ToArray();

            _modeOfOperation = opMode;
            _binaryStatus = binaryStatus;
            ReadySwitchOn = statusArray[15];
            IsOn = statusArray[14];
            IsOpEnabled = statusArray[13];
            Fault = statusArray[12];
            VoltageEnabled = statusArray[11];
            QuickStop = statusArray[10];
            SwitchOnDiabled = statusArray[9];
            Warning = statusArray[8];
            MotorEnabled = statusArray[6];
            TargetReached = statusArray[5];
            InternalLimitActive = statusArray[4];
            SetpointApplied = opMode == DryveModeOfOperation.Mode_ProfilePosition ? (bool?)statusArray[3] : null;
            IsMoving = opMode == DryveModeOfOperation.Mode_ProfileVelocity ? (bool?)!statusArray[3] : null;
            HomingAttained = opMode == DryveModeOfOperation.Mode_Homing ? (bool?)statusArray[3] : null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Status: " + _binaryStatus);
            sb.AppendLine("\tMode Of Operation: " + _modeOfOperation);
            sb.AppendLine("\tReady To Switch On: " + ReadySwitchOn );
            sb.AppendLine("\tIs On: " + IsOn);
            sb.AppendLine("\tIs Operation Enabled: " + IsOpEnabled);
            sb.AppendLine("\tFault: " + Fault);
            sb.AppendLine("\tVoltage Enabled: " + VoltageEnabled);
            sb.AppendLine("\tQuick Stop: " + QuickStop);
            sb.AppendLine("\tSwitch On Disabled: " + SwitchOnDiabled);
            sb.AppendLine("\tWarning: " + Warning);
            sb.AppendLine("\tMotor Enabled: " + MotorEnabled);
            sb.AppendLine("\tTarget Reached: " + TargetReached);
            sb.AppendLine("\tInternal Limit Active: " + InternalLimitActive);
            sb.AppendLine("\tSetpoint Applied: " + SetpointApplied);
            sb.AppendLine("\tIs Moving: " + IsMoving);
            sb.AppendLine("\tHoming Attained: " + HomingAttained);

            return sb.ToString();
        }

    }
}
