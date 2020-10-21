using CsharpBasics.Valve;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CsharpBasics
{
    class MyEnumerable : IEnumerable<int>
    {
        private readonly int[] arr;

        public MyEnumerable(int[] arr)
        {
            this.arr = arr;
        }
        public IEnumerator<int> GetEnumerator()
        {
            return new MyEnumerator(arr);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    class MyEnumerator : IEnumerator<int>
    {
        private readonly int[] arr;

        private int arrIndex=-1;
        public MyEnumerator(int[] arr)
        {
            this.arr = arr;
        }
        public int Current => arr[arrIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            arrIndex++;

            return arrIndex < arr.Length;
        }

        public void Reset()
        {
            arrIndex=0;
        }
    }
    class Program
    {
        
         // Delegate
         // 是特殊物件，基本精神把方法當參數使用(e.g. 傳遞)

        
        delegate double TempTransformation(int n);

        static double Temp_F_Cal(int x) => (x*9/5)+32;
        static double IceEffect(int x) => Math.Sqrt(x); 
        static void Main(string[] args)
        {

            TempTransformation tf= Temp_F_Cal;

            for (int i = 0; i <= 50; i+=10)
            {
                System.Console.WriteLine($"攝氏{i}");
                System.Console.WriteLine($"華式{tf(i)}");
                System.Console.WriteLine("------------");
                //System.Console.WriteLine((i*9/5)+32);
            }

            tf+=IceEffect;
            System.Console.WriteLine($"冰島效應加成{tf(30)}");
            //System.Console.WriteLine($"冰島效應加成{Math.Sqrt(30)}");
            tf-=IceEffect;
            System.Console.WriteLine($"室溫{tf(30)}F");
            

            // Lamdba Expression
            // 匿名函式(Anonymous Function)
            TempTransformation tfl = x => (x*9/5)+32;

            System.Console.WriteLine($"Lamdba室溫{tfl(30)}F"); 

            // Special Type of Delegate used extensively
            // Func & Action
            // 同一個運算或是功能重複使用
            
            Func<int, double> tff = x => (x*9/5)+32;
            System.Console.WriteLine($"Func室溫{tff(30)}F");

            Action<int> tfa = x=>{
                 double temp_F = (x*9/5)+32;
                 System.Console.WriteLine($"Action室溫{temp_F}F");

            };
            tfa(30);

            // Enumeration Patterns & Iterators
            /// GetEnumerator()
            /// bool MoveNext()
            /// var Current {get;}

            
            int[] numbers = {10,20,30};

            var infinite = new MyEnumerable(numbers);
            foreach(var i in infinite)
            {
                System.Console.WriteLine($"Manual Enumeration: {i}");
            }

            foreach (var i in numbers)
            {
                System.Console.WriteLine($"Array Built-in Enumeration: {i}");
            }

            string words = "Hello!World";
            CharEnumerator enumerator = words.GetEnumerator();
            while(enumerator.MoveNext()){
                System.Console.WriteLine(enumerator.Current);
            }


            // Collection (Data Structure)
            // IList, List<T>, Dictionary....


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


            // // class像是設計圖
            // // new class => 實體物件

            // var StAOV1 = new AirOperatedValve(); 
            // System.Console.WriteLine(StAOV1);
            // StAOV1.PositionerCalibrate();
            // StAOV1.Open();
            // StAOV1.Close();
            // StAOV1.RequestPosition(32);

            // var StAOV2 = new AirOperatedValve();
            // System.Console.WriteLine(StAOV2);
            // StAOV2.PositionerCalibrate();
            // StAOV2.RequestPosition(78);

            // StAOV2.Open();
            // StAOV2.Close();

            // var StAOV3 = new AirOperatedValve();
            // System.Console.WriteLine(StAOV2);
            // StAOV2.PositionerCalibrate();
            // StAOV2.RequestPosition(78);

            // StAOV2.Open();
            // StAOV2.Close();

            
            // int TotalNumber = AirOperatedValve.GetSerialNumber();
            // System.Console.WriteLine($"已經做出{TotalNumber}個標準氣動閥");  
            
           



        }
    }
}
