namespace CsharpBasics.Valve
{
    public class ManualValve: GenericValve
    {
    
        public int SerialNumber { get; protected set; } =0;
        public ManualValve(string valveName, double diameter, ValveSeatType seatType)
                            :base(valveName, diameter, seatType)
                            // public GenericValve(x, y, z)
        {
            this.SerialNumber++;  
        }
        
    }
}