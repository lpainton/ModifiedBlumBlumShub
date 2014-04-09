using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModifiedBlumBlumShub
{
    public class BBSGenerator2
    {
        uint _xVal, _p, _q;
        uint[] _parityBytes;
        ParityMode _mode;

        private uint M { get { return P * Q; } }
        private uint P { get { return _p; }  }
        private uint Q { get { return _q; }  }

        public BBSGenerator2(uint[] parityBytes, uint p, uint q, uint initSeed)
        {
            _parityBytes = parityBytes;
            _p = p;
            _q = q;
            _xVal = initSeed;

            //Check for even or odd seed
            _mode = (initSeed % 2 == 0) ? ParityMode.Even : ParityMode.Odd;
        }

        /// <summary>
        /// Calculates the next bit to be generated based on the formula X[n+1] = X[n]^2 mode M.
        /// </summary>
        /// <returns>True if 1, False if 0</returns>
        public bool GetNextBit()
        {
            uint Xnext = _xVal * _xVal;
            uint temp = Xnext % M;

            bool pBit = CalculateParityBit(temp, _parityBytes, _mode);

            _xVal = (temp > 2) ? temp : _xVal + 2;
            Console.WriteLine("x={0}", _xVal);
            return pBit;
        }

        /// <summary>
        /// Calculates the parity bit of n.  Setting isEven to true calculates even parity.  Setting odd to true calculates odd.
        /// </summary>
        /// <param name="n">The unsigned integer to calculate for.</param>
        /// <param name="isEvenParity">True if looking for even parity, otherwise false.</param>
        /// <returns>True if parity is 1, or false if 0.</returns>
        private bool CalculateParityBit(uint n, uint[] parityBits, ParityMode mode)
        {
            int bitCounter = 0;
            for (int i = parityBits.Length - 1; i >= 0; i--)
            {
                if (n >= parityBits[i])
                {
                    n -= parityBits[i];
                    bitCounter++;
                }
            }

            //If even and looking for even then we want to return 0, else 1
            //If even and looking for odd then we want to return 1, else 0
            return ((bitCounter % 2) == 0) ^ (mode.Equals(ParityMode.Even));
        }
    }
}
