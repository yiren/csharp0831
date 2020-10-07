using CsharpBasics.Valve.TypeEnum;
namespace CsharpBasics.Valve
{
    
    public class GenericValve
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
}