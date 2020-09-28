using System;
using System.Threading;
namespace CsharpBasics
{
   


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

    enum ValveStatus
    {
        Opened,
        Closed,
        OpenClosing
    }
    //母類別
    class GenericValve
    {   
        // 0 = Fail as it is, 1= Fail to open, 2= fail to closed
        
        // 除了自己跟子類別以外，都看不到
        protected GenericValve(double diameter)
        {
            this._diameter = diameter;
        }

        public GenericValve(string valveName, double diameter, ValvePowerSource powerSource)
        {   
            this.ValveName = valveName;
            this._seatType = ValveSeatType.Ball;// "Ball" "Butterfly"
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
        
        // 屬性(Property)
        public string ValveName { get; protected set; }
        
        public ValveStatus Status { get; protected set; }

        public ValvePowerSource _powerSource {get; protected set;}
        public ValveSeatType _seatType {get; protected set;}
        
        //public string _ValveSeatType { get;protected set; }
        public readonly double _diameter; // 閥的size
        public bool FullClosed { get; protected set;}
        public bool FullOpened { get; protected set;}
        public int SerialID { get; set; }
        public bool IsOpenClosing { get; protected set; }

        public bool OpenLimitSwitchEnabled { get; protected set; }

        public bool CloseLimitSwitchEnabled { get; protected set; }
        
       
        // 方法(Method)-動作
        public virtual void Open()
        {
            if(this.FullOpened){
                System.Console.WriteLine($"{this.ValveName}已全開");
                return;
            }
            System.Console.WriteLine($"{this.ValveName}打開中");

            this.IsOpenClosing = true;
            this.FullOpened = false;
            if(this.IsOpened())
            {
                this.FullOpened = true;
                this.IsOpenClosing= false;
                System.Console.WriteLine($"{this.ValveName}已全開");
            }
        }
        public virtual void Close()
        {
            if(this.FullClosed){
                System.Console.WriteLine($"{this.ValveName}已關");
                return;
            }

            System.Console.WriteLine($"{this.ValveName}關閉中");
            
            this.IsOpenClosing = true;
            this.FullClosed = false;
            if(this.IsClosed())
            {
                this.FullClosed=true;
                this.IsOpenClosing= false;
                System.Console.WriteLine($"{this.ValveName}已關");
            }

        }

        public virtual bool IsOpened()
        {
            this.OpenLimitSwitchEnabled = true;
            System.Console.WriteLine($"{this.ValveName}已碰到Open Limit Switch");
            if(this.OpenLimitSwitchEnabled)
            {
                return true;
            }else{
                return false;
            }
            
            
            // tenery operator (三元運算子)
            //return this.OpenLimitSwitchEnabled ? true : false;
            
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
        // 覆寫母類別的方法定義
        public override string ToString()
        {
            return $"{this.ValveName} 是 {this._seatType} 型式，大小是{this._diameter}，是用{this._powerSource}驅動。";
        }
        
    }
    //子類別
    class ManualValve: GenericValve
    {
    
        public int SerialNumber { get; protected set; } =0;
        public ManualValve(string valveName, double diameter, ValveSeatType seatType)
                            :base(valveName, diameter, seatType)
                            // public GenericValve(x, y, z)
        {
            this.SerialNumber++;  
        }
        
    }
    enum PositionerOpenMode{
        Linear,
        Equal,
        QuickOpen
    }
   
    
    class AirOperatedValve: GenericValve
    {
        
        
        class SuperPositioner
        {
            public const double MIN_PRESSURE = 5;
            public const double MAX_PRESSURE = 60;
            public const double CLOSING_PRESSURE = 20;
            public const double OPENING_PRESSURE = 50;
            public bool IsReverse { get; private set; } = false;
            public bool IsManual { get; private set; } = false;
            public SuperPositioner()
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
                        // System.Console.WriteLine("氣動閥開啟中");
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
                        // System.Console.WriteLine("氣動閥關閉中");
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
                if(this.AirSupplyPressure < MIN_PRESSURE || this.OutputPressure < MIN_PRESSURE)
                {
                    System.Console.WriteLine("壓力不足，無法作動");
                    return false;
                }
                if(this.AirSupplyPressure > MAX_PRESSURE || this.OutputPressure > MAX_PRESSURE)
                {
                    System.Console.WriteLine("壓力過高，無法作動");
                    return false;
                }
                return true;
            }

        }

    
        private SuperPositioner positioner;

        private Random random = new Random();
        public double CurrentPositionPercentage { get; protected set; } 

        private static int StandardSerialNumber = 0;

        public static int GetSerialNumber() => StandardSerialNumber;
        public AirOperatedValve():base(6)
        {
            StandardSerialNumber++;
            this.ValveName = $"台電AOV標準品字第{StandardSerialNumber}";
            this._powerSource = ValvePowerSource.Air;
            this._seatType= ValveSeatType.Ball;
            this.CurrentPositionPercentage = random.Next(30, 50);
            this.positioner= new SuperPositioner();
            
        }
        public AirOperatedValve(string valveName, double diameter):base(valveName, diameter, ValvePowerSource.Air)
        {
            
            this.CurrentPositionPercentage = random.Next(30, 50);
            this.positioner= new SuperPositioner();
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

            
            // var manualValve =  new GenericValve("手動閥-1", 3.5, ValvePowerSource.Manual, ValveSeatType.Ball);
            // //var manualValve = new GenericValve();
            // System.Console.WriteLine(manualValve);
            // manualValve.Open();
            // manualValve.Close();
            // manualValve.ChangeName("newName");
            // manualValve.SerialID = 1235;
           

            // var airValve =  new GenericValve("氣動閥-1", 9, ValvePowerSource.Air, ValveSeatType.ButterFly);
            // System.Console.WriteLine(airValve);
            // airValve.Open();
            // airValve.Close();


            // var MV1 =  new ManualValve("特製手動閥", 5.5, ValveSeatType.ButterFly);
            // System.Console.WriteLine(MV1);
            // MV1.Open();
            // MV1.Close();
            // MV1.ChangeName($"手動閥{MV1.SerialNumber}號");


            // var MV2 =  new ManualValve("特製手動閥", 5.5, ValveSeatType.ButterFly);
            // System.Console.WriteLine(MV1);
            // MV2.Open();
            // MV2.Close();
            // MV2.ChangeName($"手動閥{MV1.SerialNumber}號");
    

            // var AOV1 = new AirOperatedValve("氣動閥2", 6);
            // System.Console.WriteLine(AOV1);
            // AOV1.PositionerCalibrate();
            // AOV1.Open();
            // AOV1.Close();


            // class像是設計圖
            // new class => 實體物件

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

            var StAOV3 = new AirOperatedValve();
            System.Console.WriteLine(StAOV2);
            StAOV2.PositionerCalibrate();
            StAOV2.RequestPosition(78);

            StAOV2.Open();
            StAOV2.Close();

            
            int TotalNumber = AirOperatedValve.GetSerialNumber();
            System.Console.WriteLine($"已經做出{TotalNumber}個標準氣動閥");  
            
            
        }
    }
}
