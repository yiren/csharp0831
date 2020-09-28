namespace CsharpBasics.Valve
{
    public interface IValve
    {
        float Diameter { get; }
        bool IsClosed { get;}
        bool IsOpened { get; }

        ValvePowerSource ValvePowerSource {get; }
        ValveSeatType ValveSeatType {get; }
        void Open();
        void Close();
    }
}