using CsharpBasics.Valve;
namespace CsharpBasics
{
    class Program
    {
        static void Main(string[] args)
        {

            
            var manualValve =  new GenericValve("手動閥-1", 3.5, ValvePowerSource.Manual, ValveSeatType.Ball);
            //var manualValve = new GenericValve();
            System.Console.WriteLine(manualValve);
            manualValve.Open();
            manualValve.Close();
            manualValve.ChangeName("newName");
            manualValve.SerialID = 1235;
           

            var airValve =  new GenericValve("氣動閥-1", 9, ValvePowerSource.Air, ValveSeatType.ButterFly);
            System.Console.WriteLine(airValve);
            airValve.Open();
            airValve.Close();


            var MV1 =  new ManualValve("特製手動閥", 5.5, ValveSeatType.ButterFly);
            System.Console.WriteLine(MV1);
            MV1.Open();
            MV1.Close();
            MV1.ChangeName($"手動閥{MV1.SerialNumber}號");


            var MV2 =  new ManualValve("特製手動閥", 5.5, ValveSeatType.ButterFly);
            System.Console.WriteLine(MV1);
            MV2.Open();
            MV2.Close();
            MV2.ChangeName($"手動閥{MV1.SerialNumber}號");
    

            var AOV1 = new AirOperatedValve("氣動閥2", 6);
            System.Console.WriteLine(AOV1);
            AOV1.PositionerCalibrate();
            AOV1.Open();
            AOV1.Close();


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
