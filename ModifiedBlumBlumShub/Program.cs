using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace ModifiedBlumBlumShub
{
    class Program
    {


        static void Main(string[] args)
        {
            uint[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863 };
            uint mostSigBit = 20;
            uint[] parityBytes = new uint[mostSigBit];

            //Construct parity bit list
            for (int i = 0; i < mostSigBit; i++)
            {
                parityBytes[i] = 1;
                for (int j = 0; j < i; j++)
                    parityBytes[i] = parityBytes[i] * 2;
            }
            //foreach (uint b in parityBits) Console.WriteLine(b);

            string picPath = @"c:\documents and settings\lee.hal\my documents\visual studio 2010\Projects\ModifiedBlumBlumShub\ModifiedBlumBlumShub\SmallGrain.jpg";
            uint[] N;

            uint averageVal = ReadPicture(picPath, out N);

            //Get number to produce
            Console.Write("Enter the quantity of pseudorandom numbers to produce: ");
            int quant = Convert.ToInt32(Console.ReadLine());

            List<uint> NPrime;
            ReduceMatrixToPrimes(N, primes, out NPrime);

            BBSGenerator myGenerator = new BBSGenerator(parityBytes, NPrime.ToArray(), averageVal);

            /* Test Code Makes 8-bit numbers
            for (int i = 0; i < quant; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    sb.Append(Convert.ToInt16(myGenerator.GetNextBit()));
                }
                Console.WriteLine(Convert.ToInt32(sb.ToString(),2));
            }*/

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < quant; i++)
            {
                sb.Append(Convert.ToInt16(myGenerator.GetNextBit()));
            }
            System.IO.File.WriteAllText(@"C:\Documents and Settings\Lee.HAL\My Documents\Visual Studio 2010\Projects\ModifiedBlumBlumShub\ModifiedBlumBlumShub\TestResults.txt", sb.ToString());
        }

        /// <summary>
        /// Calculates the GCD of two positive integers.
        /// </summary>
        /// <param name="a">A positive integer</param>
        /// <param name="b">Another positive integer</param>
        /// <returns>The largest positive integer which divides both numbers</returns>
        public static int GCD(int a, int b)
        {
            int smallNum = (a <= b) ? a : b;
            int bigNum = (smallNum == a) ? b : a;

            for (int i = smallNum; i > 1; i--)
            {
                //If both ints are congruent to 0 mod i then we have our GCD
                if ((smallNum % i) == 0 || (bigNum % i) == 0)
                    return i;
            }

            return 1;
        }

        /// <summary>
        /// Euler's Totient Function.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="primes"></param>
        /// <returns></returns>
        public static int Phi(int n, uint[] primes)
        {
            return 0;
        }

        /// <summary>
        /// Returns a collection of all members of a stack of primes which are factors of an integer n.
        /// </summary>
        /// <param name="n">A non-zero integer (ideally)</param>
        /// <param name="primes">The list of primes to check</param>
        /// <param name="primeFactors"></param>
        public static void GetPrimeFactors(uint n, uint[] primes, out Stack<uint> primeFactors)
        {
            primeFactors = new Stack<uint>();

            foreach (uint p in primes)
            {
                if (n % p == 0)
                    primeFactors.Push(p);
            }
        }

        /// <summary>
        /// Finds the largest prime factor from a list of prime factors of a given number.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="primes"></param>
        /// <returns></returns>
        public static uint GetLargestPrimeFactor(uint n, uint[] primes)
        {
            Stack<uint> factors;
            GetPrimeFactors(n, primes, out factors);

            if (factors.Count == 0)
                return 0;
            else
                return factors.Peek();
        }

        /// <summary>
        /// Reduces a matrix of unsigned ints to a list of unique prime factors -- unordered except in the order they are found.
        /// We are only looking for prime factors congruent to 3 mod 4.
        /// </summary>
        /// <param name="matrix">The matrix of unsigned integers.</param>
        /// <param name="primeMatrix">Our output matrix, containing prime factors congruent to 3 mod 4.</param>
        public static void ReduceMatrixToPrimes(uint[] matrix, uint[] primes, out List<uint> primeMatrix)
        {
            primeMatrix = new List<uint>();
            Queue<uint> congruentPrimes = new Queue<uint>(primes);

            //Filter out all primes not congruent to 3 mod 4.
            //Goes through each prime, dequeues, checks congruency and only requeues if acceptable.
            for (int i = 0; i < congruentPrimes.Count; i++)
            {
                uint p = congruentPrimes.Dequeue();
                if ((p % 4) == 3)
                    congruentPrimes.Enqueue(p);
            }

            //Reduce each member of 'matrix' to its largest congruent prime.
            //Store in primeMatrix only if unique.
            for (int i = 0; i < matrix.Length; i++)
            {
                uint p = GetLargestPrimeFactor(matrix[i], congruentPrimes.ToArray());
                if (p > 0)
                    if (!primeMatrix.Contains(p))
                        primeMatrix.Add(p);
            }

            //Cycle through all elements of the matrix.  Look for the largest prime factor in each.
            for (int i = 0; i < matrix.Length; i++)
            {

            }
        }

       
        /// <summary>
        /// Reads all pixels in a raster image for their RGB values and outputs an array of their RGB values summed.
        /// Also returns the floor of the average for the array.
        /// </summary>
        /// <param name="path">A string with the path to the picture to be read.</param>
        /// <param name="matrix">The uninitialized output array.</param>
        /// <returns>The floor of the average of the array.</returns>
        public static uint ReadPicture(string path, out uint[] matrix)
        {
            //Load image
            FileStream f = File.OpenRead(path);
            Bitmap b = new Bitmap(f);

            //Calculate dimensions
            uint height = (uint)b.PhysicalDimension.Height;
            uint width = (uint)b.PhysicalDimension.Width;
            uint maxSize = height * width;

            //Test code
            System.Console.WriteLine(b.PhysicalDimension);
            System.Console.WriteLine("Max size = " + maxSize);

            //Initialize Matrix
            matrix = new uint[maxSize];
            uint sum = 0;

            //Read loop, construct N
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    Color clr = b.GetPixel(j, i);
                    //Console.WriteLine(clr.ToString());
                    int[] c = { clr.R, clr.G, clr.B };
                    //Console.WriteLine(c[0]);

                    int mIndex = i * (int)width + j;

                    //Matrix N
                    matrix[mIndex] = Convert.ToUInt32(c.Sum());
                    sum += matrix[mIndex];
                    
                    //Console.WriteLine("mIndex:{0} N:{1}", mIndex, N[mIndex]);
                    //Console.WriteLine(Convert.ToString(N[mIndex]));
                }



            return sum / maxSize;
        }
    }
}
