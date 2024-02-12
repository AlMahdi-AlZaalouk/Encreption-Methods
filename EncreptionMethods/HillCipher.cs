using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncreptionMethods
{
    internal class HillCipher
    {

            int[,] keyMatrix;
            int matrixSize;

            public HillCipher(string key)
            {
                SetKey(key);
            }

            private void SetKey(string key)
            {
                matrixSize = (int)Math.Sqrt(key.Length);
                keyMatrix = new int[matrixSize, matrixSize];

                if (matrixSize * matrixSize != key.Length)
                {
                    throw new ArgumentException("Invalid key length. Key length must be a perfect square.");
                }

                for (int i = 0, k = 0; i < matrixSize; i++)
                {
                    for (int j = 0; j < matrixSize; j++, k++)
                    {
                        keyMatrix[i, j] = CharToNumber(key[k]);
                    }
                }
            }

            public string Encrypt(string plaintext)
            {
                plaintext = plaintext.ToUpper().Replace(" ", "");
                string ciphertext = "";

                while (plaintext.Length % matrixSize != 0)
                {
                    plaintext += "X";
                }

                for (int i = 0; i < plaintext.Length; i += matrixSize)
                {
                    int[] vector = new int[matrixSize];
                    for (int j = 0; j < matrixSize; j++)
                    {
                        vector[j] = CharToNumber(plaintext[i + j]);
                    }

                    int[] resultVector = MultiplyMatrix(keyMatrix, vector);

                    for (int j = 0; j < matrixSize; j++)
                    {
                        ciphertext += NumberToChar(resultVector[j]);
                    }
                }

                return ciphertext;
            }

            private int[] MultiplyMatrix(int[,] matrix, int[] vector)
            {
                int[] result = new int[matrixSize];
                for (int i = 0; i < matrixSize; i++)
                {
                    for (int j = 0; j < matrixSize; j++)
                    {
                        result[i] += matrix[i, j] * vector[j];
                    }
                    result[i] %= 26;
                }
                return result;
            }

            private int CharToNumber(char c)
            {
                return c - 'A';
            }

            private char NumberToChar(int n)
            {
                return (char)((n + 26) % 26 + 'A');
            }

        public string Decrypt(string ciphertext)
        {
            string plaintext = "";
            int[,] inverseMatrix = InverseMatrix(keyMatrix);

            for (int i = 0; i < ciphertext.Length; i += matrixSize)
            {
                int[] vector = new int[matrixSize];
                for (int j = 0; j < matrixSize; j++)
                {
                    vector[j] = CharToNumber(ciphertext[i + j]);
                }

                int[] resultVector = MultiplyMatrix(inverseMatrix, vector);

                for (int j = 0; j < matrixSize; j++)
                {
                    plaintext += NumberToChar(resultVector[j]);
                }
            }

            return plaintext;
        }

        private int[,] InverseMatrix(int[,] matrix)
        {
            int determinant = ModularInverse(Determinant(matrix, matrixSize), 26);
            int[,] adjugate = Adjugate(matrix);

            int[,] inverse = new int[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    inverse[i, j] = (adjugate[i, j] * determinant) % 26;
                    if (inverse[i, j] < 0) inverse[i, j] += 26;
                }
            }
            return inverse;
        }

        private int Determinant(int[,] matrix, int n)
        {
            int det = 0;
            if (n == 1)
            {
                return matrix[0, 0];
            }

            int[,] temp = new int[matrixSize, matrixSize];
            int sign = 1;

            for (int f = 0; f < n; f++)
            {
                GetCofactor(matrix, temp, 0, f, n);
                det += sign * matrix[0, f] * Determinant(temp, n - 1);
                sign = -sign;
            }

            return det;
        }

        private void GetCofactor(int[,] matrix, int[,] temp, int p, int q, int n)
        {
            int i = 0, j = 0;
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    if (row != p && col != q)
                    {
                        temp[i, j++] = matrix[row, col];
                        if (j == n - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
        }

        private int[,] Adjugate(int[,] matrix)
        {
            if (matrixSize == 1)
            {
                return new int[,] { { 1 } };
            }

            int sign = 1;
            int[,] adj = new int[matrixSize, matrixSize];
            int[,] temp = new int[matrixSize, matrixSize];

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    GetCofactor(matrix, temp, i, j, matrixSize);
                    sign = ((i + j) % 2 == 0) ? 1 : -1;
                    adj[j, i] = (sign * Determinant(temp, matrixSize - 1)) % 26;
                    if (adj[j, i] < 0) adj[j, i] += 26;
                }
            }

            return adj;
        }

        private int ModularInverse(int a, int m)
        {
            a %= m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                    return x;
            }
            throw new ArgumentException("No modular inverse found. The key might be invalid.");
        }

    }
        /*public class Program
        {
            public static void Main(string[] args)
            {
                Console.Write("Enter the plaintext: ");
                string plaintext = Console.ReadLine().ToUpper();

                Console.Write("Enter the key (length must be a perfect square): ");
                string key = Console.ReadLine().ToUpper();

                try
                {
                    HillCipher cipher = new HillCipher(key);
                    string encrypted = cipher.Encrypt(plaintext);

                    Console.WriteLine("Encrypted Text: " + encrypted);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

    }*/
}
