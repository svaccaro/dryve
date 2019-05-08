using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentControlLibrary.Dryve
{
    public enum DryveObject
    {
        Controlword,
        Statusword,
        ModesOfOperation,
        ModesOfOperationDisplay,
        PositionActual,
        PositionWindow,
        PositionWindowTime,
        VelocityActual,
        TargetPosition,
        HomeOffset,
        ProfileVelocity,
        ProfileAcceleration,
        ProfileDeceleration,
        FeedConstant_Feed,
        FeedConstant_ShaftRevolutions,
        HomingMethod,
        HomingSpeeds_SearchZero,
        HomingSpeeds_SearchSwitch,
        HomingAcceleration,
        DigitalInputs,
        DigitalOutputs_PhysicalOutputs,
        DigitalOutputs_BitMask,
        TargetVelocity,
        SupportedModes,
        PositionDemand
    }

    public enum DryveModeOfOperation
    {
        Mode_ProfilePosition,
        Mode_ProfileVelocity,
        Mode_Homing
    }

    public enum DryveHomingMethod
    {
        Method0,
        Method1,
        Method2,
        Method3,
        Method4
    }

    public enum ResultFormat
    {
        Binary,
        Integer,
        UInt16,
        UInt32,
        Int16,
        Int32
    }

    public enum ObjectAccess
    {
        Read,
        Write,
        RO,
        RWW
    }
}
