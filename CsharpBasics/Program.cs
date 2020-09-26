using System;
using System.Threading;
namespace CsharpBasics
{
    enum ValveSeatType
    {
        Ball,
        ButterFly,
        Gate
    }

    enum ValvePowerSource
    {
        Manual,
        Air,
        Motor
    }


    interface IValve
    {
        float Diameter { get; }
        bool IsClosed { get;}
        bool IsOpened { get; }

        ValvePowerSource ValvePowerSource {get; }
        ValveSeatType ValveSeatType {get; }
        void Open();
        void Close();
    }

    class GenericValve
    {   
        // 0 = Fail as it is, 1= Fail to open, 2= fail to closed
        

        protected GenericValve(double diameter)
        {
            this._diameter = diameter;
        }

        public GenericValve(string valveName, double diameter, ValvePowerSource powerSource)
        {   
            this.ValveName = valveName;
            this._seatType = ValveSeatType.Ball;
            this._diameter=diameter;
            this._powerSource = powerSource;
        }

        public GenericValve(string valveName, double diameter, ValveSeatType seatType)
        {   
            this.ValveName = valveName;
            this._seatType = seatType;
            this._diameter=diameter;
            this._powerSource = ValvePowerSource.Manual;
        }
        public GenericValve(string valveName, double diameter, ValvePowerSource powerSource, ValveSeatType seatType)
        {   
            this.ValveName = valveName;
            this._powerSource = powerSource;
            this._seatType = seatType;
            this._diameter=diameter;
        }
        public string ValveName { get; protected set; }
        public int SerialID { get; set; }
        public ValvePowerSource _powerSource {get; protected set;}
        public ValveSeatType _seatType {get; protected set;}
        public readonly double _diameter;
        public bool Closed { get; protected set;}
        public bool Opened { get; protected set;}

        public bool IsOpenClosing { get; protected set; }

        public bool OpenLimitSwitchEnabled { get; protected set; }

        public bool CloseLimitSwitchEnabled { get; protected set; }
        public virtual void Open()
        {
            if(this.Opened){
                System.Console.WriteLine($"{this.ValveName}已開");
                return;
            }
            System.Console.WriteLine($"{this.ValveName}打開中");

            this.IsOpenClosing = true;
            this.Opened = false;
            if(this.IsOpened())
            {
                this.Opened = true;
                this.IsOpenClosing= false;
                System.Console.WriteLine($"{this.ValveName}已開");
            }
        }
        public virtual void Close()
        {
            if(this.Closed){
                System.Console.WriteLine($"{this.ValveName}已關");
                return;
            }

            System.Console.WriteLine($"{this.ValveName}關閉中");
            
            this.IsOpenClosing = true;
            this.Closed = false;
            if(this.IsClosed())
            {
                this.Closed=true;
                this.IsOpenClosing= false;
                System.Console.WriteLine($"{this.ValveName}已關");
            }

        }

        public virtual bool IsOpened()
        {
            
            System.Console.WriteLine($"{this.ValveName}已碰到Open Limit Switch");
            this.OpenLimitSwitchEnabled = true;
            // tenery operator 
            return this.OpenLimitSwitchEnabled ? true : false;
        }

        public virtual bool IsClosed()
        {
            
            System.Console.WriteLine($"{this.ValveName}已碰到Close Limit Switch");
            this.CloseLimitSwitchEnabled = true;
            // tenery operator 
            return this.CloseLimitSwitchEnabled ? true : false;
        }

        public void ChangeName(string newName)
        {
            this.ValveName = newName;
            System.Console.WriteLine($"閥名稱已改為{this.ValveName}");
        }

        public override string ToString()
        {
            return $"{this.ValveName} 是 {this._seatType} 型式，大小是{this._diameter}，是用{this._powerSource}驅動。";
        }

    }

    class ManualValve: GenericValve
    {
        
        public ManualValve(string valveName, double diameter, ValveSeatType seatType):base(valveName, diameter, seatType)
        {
    
        }
        
    }

    enum PositionerOpenMode{
        Linear,
        Equal,
        QuickOpen
    }
    
    class AirOperatedValve: GenericValve
    {
        
        private class Positioner
        {
            public const double MIN_PRESSURE = 10;
            public const double MAX_PRESSURE = 90;
            public const double CLOSING_PRESSURE = 20;
            public const double OPENING_PRESSURE = 50;
            public bool IsReverse { get; private set; } = false;
            public bool IsManual { get; private set; } = false;
            public Positioner()
            {
                this.InitializePositioner();
            }
            public double TravelLength { get; set; }
            public double AirSupplyPressure { get; set; }
            public double OutputPressure { get; set; }
            public PositionerOpenMode OpenMode { get; set; }

            public bool Calibrate()
            {
            System.Console.WriteLine("進行校正中");
            
            System.Console.WriteLine("進行校正完成");
            return true;
            }

            public void InitializePositioner()
            {
                this.AirSupplyPressure = 35;
                this.OutputPressure = 35;
                this.OpenMode = PositionerOpenMode.Linear;
                this.TravelLength =100;
            }

            public void SwitchToManual(){
                this.IsManual = true;
            }

            public void SetOutputReverse(){
                this.IsReverse = true;
            }

            public double RequestCommand(double currentPercentage, double targetPercentage){
                // Opening
                if(targetPercentage > currentPercentage)
                {
                    
                    // I/P Module Calculation
                    while(this.OutputPressure < OPENING_PRESSURE)
                    {
                        System.Console.WriteLine("氣動閥開啟中");
                        this.OutputPressure+=5;
                    }
                    System.Console.WriteLine($"輸出壓力{this.OutputPressure} psi");
                    return targetPercentage;
        
                }

                //Closing
                if(targetPercentage < currentPercentage)
                {
                    // I/P Module Calculation
                    // while(this.OutputPressure > CLOSING_PRESSURE)
                    // {
                    //     System.Console.WriteLine("氣動閥關閉中");
                    //     this.OutputPressure -= 5;
                    // }
                    for (double i = OutputPressure; i > CLOSING_PRESSURE; i-=5)
                    {
                        System.Console.WriteLine("氣動閥關閉中");
                        this.OutputPressure -= 5;   
                    }
                    System.Console.WriteLine($"輸出壓力{this.OutputPressure} psi");
                    return targetPercentage;
                }

                System.Console.WriteLine($"開度{currentPercentage}%無變化");
                return currentPercentage;
            }

            public bool IsPositionerNormal()
            {
                if(this.AirSupplyPressure < 10 || this.OutputPressure <10)
                {
                    System.Console.WriteLine("壓力不足，無法作動");
                    return false;
                }
                if(this.AirSupplyPressure > 90 || this.OutputPressure >90)
                {
                    System.Console.WriteLine("壓力過高，無法作動");
                    return false;
                }
                return true;
            }

        }

        private Positioner positioner;

        private Random random = new Random();
        public double CurrentPositionPercentage { get; protected set; } 

        public static int StandardSerialNumber = 0;
        public AirOperatedValve():base(6)
        {
            StandardSerialNumber++;
            this.ValveName = $"台電AOV標準品字第{StandardSerialNumber}";
            this._powerSource = ValvePowerSource.Air;
            this._seatType= ValveSeatType.Ball;
            this.CurrentPositionPercentage = random.Next(30, 50);
            this.positioner= new Positioner();
            
        }
        public AirOperatedValve(string valveName, double diameter):base(valveName, diameter, ValvePowerSource.Air)
        {
            
            this.CurrentPositionPercentage = random.Next(30, 50);
            this.positioner= new Positioner();
        }

        public override void Open()
        {
            if(!this.positioner.IsPositionerNormal())
                return ;
            else
            {
                System.Console.WriteLine($"{this.ValveName}-目前開度{this.CurrentPositionPercentage}%，指定全開");
                
                this.CurrentPositionPercentage = this.positioner.RequestCommand(this.CurrentPositionPercentage, 100);
                if(this.IsOpened()){
                    System.Console.WriteLine($"{this.ValveName}已全開");
                }
            }

        }

        public override bool IsClosed(){
               
           return this.CurrentPositionPercentage == 0 ? true : false;
        }

        public override bool IsOpened(){
               return this.CurrentPositionPercentage == 100 ? true : false;
        }

        public override void Close()
        {
            if(!this.positioner.IsPositionerNormal())
                return ;
            else
            {
                System.Console.WriteLine($"{this.ValveName}-目前開度{this.CurrentPositionPercentage}%，指定全關");
                
                this.CurrentPositionPercentage = this.positioner.RequestCommand(this.CurrentPositionPercentage, 0);
                if(this.IsClosed()){
                    System.Console.WriteLine($"{this.ValveName}已全關");
                }
            }


        }

        public void RequestPosition(double requestedPosition)
        {
            if(!this.positioner.IsPositionerNormal())
                return;
            else
            {
                System.Console.WriteLine($"{this.ValveName}-目前開度{this.CurrentPositionPercentage}%，指定開度{requestedPosition}%");
                this.CurrentPositionPercentage = this.positioner.RequestCommand(this.CurrentPositionPercentage, requestedPosition);
                System.Console.WriteLine($"已達指定開度{this.CurrentPositionPercentage}%");
            }
        }

        public bool IsCalibrated { get; protected set; } = false;
        public void PositionerCalibrate(){
            if(!this.IsCalibrated){
                System.Console.WriteLine("定位器需要校正");
                this.positioner.Calibrate();
            }
            
        }


    }

    class MotorOperatedValve
    {
        // TODO 
    }


    class Program
    {
        static void Main(string[] args)
        {
            var manualValve =  new GenericValve("手動閥-1", 3.5, ValvePowerSource.Manual, ValveSeatType.Ball);
            System.Console.WriteLine(manualValve);
            manualValve.Open();
            manualValve.Close();


            var airValve =  new GenericValve("氣動閥-1", 9, ValvePowerSource.Air, ValveSeatType.ButterFly);
            System.Console.WriteLine(airValve);
            airValve.Open();
            airValve.Close();


            var MV1 =  new ManualValve("手動閥-2", 3.5, ValveSeatType.Ball);
            System.Console.WriteLine(MV1);
            MV1.Open();
            MV1.Close();

            var AOV1 = new AirOperatedValve("氣動閥-2", 6);
            System.Console.WriteLine(AOV1);
            AOV1.PositionerCalibrate();
            AOV1.Open();
            AOV1.Close();

            var StAOV1 = new AirOperatedValve();
            System.Console.WriteLine(StAOV1);
            StAOV1.PositionerCalibrate();
            StAOV1.Open();
            StAOV1.Close();
            StAOV1.RequestPosition(32);

            var StAOV2 = new AirOperatedValve();
            System.Console.WriteLine(StAOV2);
            StAOV2.PositionerCalibrate();
            StAOV2.RequestPosition(78);

            StAOV2.Open();
            StAOV2.Close();
            
        }
    }
}
