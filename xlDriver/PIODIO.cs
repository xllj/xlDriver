using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace PIODIO_Ns
{
    public class PIODIO
    {   
      
        //****************
        //PIODIO CARD ID
        //****************
    
        public const uint PIOD_24=0x800140;
        public const uint PIOD_48=0x800130;
        public const uint PIOD_56=0x800140;
        public const uint PIOD_64=0x800120;
        public const uint PIOD_96=0x800110;
        public const uint PIOD_144=0x800100;
        public const uint PIOD_168=0x98800150;
        public const uint PIOD_168A=0x800150;

        //****************
        //Error Code 
        //****************

        public const uint NoError = 0;
        public const uint DriverOpenError = 1;
        public const uint DriverNoOpen = 2;
        public const uint GetDriverVersionError = 3;
        public const uint InstallIrqError = 4;
        public const uint ClearIntCountError = 5;
        public const uint GetIntCountError = 6;
        public const uint RegisterApcError = 7;
        public const uint RemoveIrqError = 8;
        public const uint FindBoardError = 9;
        public const uint ExceedBoardNumber = 10;
        public const uint ResetError = 11;
        public const uint IrqMaskError = 12;
        public const uint ActiveModeError = 13;
        public const uint GetActiveFlagError = 14;
        public const uint ActiveFlagEndOfQueue = 15;

        //*****************
        //PIODIO ActiveMode
        //*****************
        
        // to trigger a interrupt when low -> high
        public const uint ActiveHigh =1;
        
        // to trigger a interrupt when high -> low
        public const uint  ActiveLow=0;

        //***********************************
        //define the interrupt signal source
        //***********************************
        public const uint PIOD144_P2C0 = 0;   // pin29 of CN1(37 pin D-type, pin1 to pin37)
        public const uint PIOD144_P2C1 = 1;   // pin28 of CN1(37 pin D-type, pin1 to pin37)
        public const uint PIOD144_P2C2 = 2;   // pin27 of CN1(37 pin D-type, pin1 to pin37)
        public const uint PIOD144_P2C3 = 3;   // pin26 of CN1(37 pin D-type, pin1 to pin37)

        //**********************************
        // Interrupt Channel for PIO-D48
        //**********************************
        public const uint PIOD48_INTCH0 = 1;  // INT_CHAN_0
        public const uint PIOD48_INTCH1 = 2;  // INT_CHAN_1
        public const uint PIOD48_INTCH2 = 4;  // INT_CHAN_2
        public const uint PIOD48_INTCH3 = 8;  // INT_CHAN_3

        //*********************************
        //Test functions
        //*********************************

       [DllImport ("Piodio.dll",EntryPoint ="PIODIO_FloatSub")]
        public static extern float FloatSub(float fA,float fB);        
       [DllImport ("Piodio.dll",EntryPoint ="PIODIO_ShortSub")]
        public static extern short ShortSub(short nA,short nB);
        
       [DllImport ("Piodio.dll",EntryPoint ="PIODIO_GetDllVersion")]
        public static extern ushort GetDllVersion();

       //**************
       // PIODIO Driver
       //**************
       [DllImport("Piodio.dll",EntryPoint="PIODIO_DriverInit")]
        public static extern ushort DriverInit();
        
        [DllImport("Piodio.dll",EntryPoint="PIODIO_DriverClose")]
        public static extern void DriverClose();
        [DllImport("Piodio.dll",EntryPoint="PIODIO_SearchCard")]
        public static extern ushort SearchCard(out ushort wBoards, uint dwPIOCardID);
        [DllImport ("Piodio.dll",EntryPoint ="PIODIO_GetDriverVision")]
        public static extern ushort GetDriverVersion(out ushort wDriverVersion);
        
      
        [DllImport("Piodio.dll",EntryPoint="PIODIO_GetConfigAddressSpace")]
        public static extern ushort GetConfigAddressSpace(
            ushort wBoardNo, out uint wAddrBase, out ushort wIrqNo,
            out ushort wSubVendor, out ushort wSubDevice, out ushort wSubAux,
            out ushort wSlotBus, out ushort wSlotDevice);
        [DllImport("Piodio.dll",EntryPoint="PIODIO_ActiveBoard")]
        public static extern ushort ActiveBoard(ushort wBoardNo);
        [DllImport("Piodio.dll",EntryPoint="PIODIO_WhichBoardActive")]
        public static extern ushort WhichBoardActive();

        // ******************************************
        [DllImport("Piodio.dll",EntryPoint="PIODIO_OutputByte")]
        public static extern void OutputByte(uint wBaseAddr, ushort bOutputValue);
        [DllImport("Piodio.dll",EntryPoint="PIODIO_InputByte")]
        public static extern ushort InputByte(uint wBaseAddr);

        //********************
        //PIODIO Interrupt
        //********************
        
        
        [DllImport("Piodio.dll", EntryPoint = "PIODIO_IntInstall")]
        public static extern ushort IntInstall(ushort wBoardNo, out uint hEvent, ushort wInterruptSource, ushort wActiveMode);
        [DllImport("Piodio.dll", EntryPoint = "PIODIO_IntRemove")]
        public static extern ushort IntRemove();


        [DllImport("Piodio.dll", EntryPoint = "PIODIO_IntGetCount")]
        public static extern ushort IntGetCount(out uint dwIntCount);

        [DllImport("Piodio.dll", EntryPoint = "PIODIO_IntResetCount")]
        public static extern ushort IntResetCount();
        
        //********************
        //PIODIO_48 Frequency
        //********************
        [DllImport("Piodio.dll")]
        public static extern uint PIOD48_Freq(uint wBaseAddr);

        //*********************
        //PIODIO_48 Counter
        //*********************
        [DllImport("Piodio.dll")]
        public static extern void PIOD48_SetCounter(uint dwBase,ushort wCounterNo,ushort bCounterMode,uint wCounterValue );
        [DllImport("Piodio.dll")]
        public static extern uint PIOD48_ReadCounter(uint dwBase,ushort wCounterNo,ushort bCounterMode);
        [DllImport ("Piodio.dll")]
        public static extern void PIOD48_SetCounterA(ushort wCounterNo, ushort bCounterMode,uint wCounterValue);
        [DllImport ("Piodio.dll")]
        public static extern uint PIOD48_ReadCounterA(ushort wCounterNo,ushort bCounterMode);
        
        //**********************
        //PIODIO_48 Interrupt
        //**********************
        [DllImport ("Piodio.dll")]
        public static extern ushort PIOD48_IntInstall(ushort wBoardNo, out uint hEvent, ushort  wIrqMask, ushort  wActiveMode);

        [DllImport ("Piodio.dll")]
        public static extern ushort PIOD48_IntRemove();
        [DllImport ("Piodio.dll")]
        public static extern ushort PIOD48_IntGetActiveFlag(out ushort bActiveHighFlag, out ushort bActiveLowFlag);
        [DllImport ("Piodio.dll")]
        public static extern ushort PIOD48_IntGetCount(out uint dwIntCount);


        //********************
        //PIODIO_64 Counter
        //********************
        [DllImport("Piodio.dll")]
        public static extern void  PIOD64_SetCounter(uint dwBase,ushort wCounterNo,ushort bCounterMode,uint wCounterValue);
        [DllImport("Piodio.dll")]
        public static extern uint PIOD64_ReadCounter(uint dwBase,ushort wCounterNo,ushort bCounterMode);

        [DllImport("Piodio.dll")]
        public static extern void PIOD64_SetCounterA(ushort wCounterNo, ushort bCounterMode,uint wCounterValue);
        [DllImport("Piodio.dll")]
        public static extern uint PIOD64_ReadCounterA(ushort wCounterNo, ushort bCounterMode);


        // ******************************************
        private int DriverOpened = 0;

        // ******************************************
        //public void OutputByte(ushort wBaseAddr, ushort bValue)
        //{
         //   OutputByte(wBaseAddr, bValue);
        //}
        //public ushort InputByte(ushort wBaseAddr)
        //{
         //   return InputByte(wBaseAddr);
        //}


        public PIODIO()//constroctor
        {
            DriverOpened = 0;
        }
        ~PIODIO()
        {
            if (DriverOpened != 0)
            {
                DriverOpened = 0;
                DriverClose();
            }
        }
    }
}
