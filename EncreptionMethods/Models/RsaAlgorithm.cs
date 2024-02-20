using System;

public class RsaAlgorithm
{
    public int P { get; private set; }
    public int Q { get; private set; }
    public int E { get; set; }
    public int N { get; private set; }
    public int Phi { get; private set; }
    public int D { get; private set; }

    public RsaAlgorithm()
    {
    }

    // Constructor with parameters
    public RsaAlgorithm(int p, int q, int e) : this()
    {
        Initialize(p, q, e);
    }

    public void Initialize(int p, int q, int e)
    {
        if (!IsPrime(p) || !IsPrime(q))
            throw new ArgumentException("P and Q must be prime.");
        if (e <= 1)
            throw new ArgumentException("E must be greater than 1.");

        P = p;
        Q = q;
        E = e;

        N = P * Q;
        Phi = (P - 1) * (Q - 1);

        if (!IsRelativelyPrime(E, Phi))
            throw new ArgumentException("E must be relatively prime to Phi.");

        D = CalculateModularMultiplicativeInverse(E, Phi);
    }

    public int Encrypt(int m)
    {
        if (m >= N) throw new ArgumentException($"Message must be less than N. N={N}");
        return ModularExponentiation(m, E, N);
    }

    public int Decrypt(int c)
    {
        return ModularExponentiation(c, D, N);
    }

    private int ModularExponentiation(int baseValue, int exponent, int modulus)
    {
        int result = 1;
        baseValue %= modulus;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
                result = (result * baseValue) % modulus;

            exponent >>= 1;
            baseValue = (baseValue * baseValue) % modulus;
        }
        return result;
    }
    private bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i * i <= number; i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }

    private bool IsRelativelyPrime(int a, int b)
    {
        return Gcd(a, b) == 1;
    }

    private int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private int CalculateModularMultiplicativeInverse(int a, int m)
    {
        int m0 = m, y = 0, x = 1;

        if (m == 1) return 0;

        while (a > 1)
        {
            int q = a / m;
            int t = m;

            // m is remainder now, process same as Euclid's algo
            m = a % m; a = t;
            t = y;

            // Update y and x
            y = x - q * y;
            x = t;
        }

        // Make x positive
        if (x < 0) x += m0;

        return x;
    }

    /*private int ModInverse(int a, int m)
    {
        int m0 = m;
        int y = 0;
        int x = 1;

        if (m == 1) return 0;

        while (a > 1)
        {
            // q is quotient
            int q = a / m;
            int t = m;

            // m is remainder
            m = a % m;
            a = t;

            // update y and x
            t = y;
            y = x - q * y;
            x = t;
        }

        if (x < 0)
        {
            x += m0;
        }

        return x;
    }
*/
    private string ConvertToInt(string message)
    {
        string result = "";
        for (int i = 0; i < message.Length; i++)
        {
            if (char.IsNumber(message[i]))
            {
                result += message[i];
            }
            else
            {
                result += ((int)message[i]).ToString();
            }
        }

        return result;
    }

    
    private string ConvertToString(int message)
    {
        string result = "";
        while (message > 0)
        {
            int digit = message % 10;
            result = (char)(digit + '0') + result;
            message /= 10;
        }

        return result;
    }
}
