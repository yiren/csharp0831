using System;

namespace PrimeService
{
    public class PrimeService
    {
        public Boolean IsPrime(int n){
            Boolean NotPrime =  false;
            if(n==0 || n==1 ){
                return NotPrime;
            }
            
            if(n==2 || n==3 || n==5 || n==7){
                return !NotPrime;
            }

            else {
                for (int i = 2; i <= n / 2; ++i) {
                    if (n % i == 0) {
                        NotPrime = true;
                        break;
                    }
                }
            }
            return NotPrime;
        }
        
    }
}
