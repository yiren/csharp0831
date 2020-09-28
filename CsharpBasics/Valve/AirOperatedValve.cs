using System;

namespace CsharpBasics.Valve
{

    public class AirOperatedValve: GenericValve
    {
        enum PositionerOpenMode{
        Linear,
        Equal,
        QuickOpen
        }    
        
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
}