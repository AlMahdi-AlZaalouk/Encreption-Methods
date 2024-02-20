using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EncreptionMethods
{
    internal class HillCipherAlgorithm
    {

        int[,] keyMatrix;
        int matrixSize;

        public HillCipherAlgorithm(string key)
        {
            SetKey(key.Trim());
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

            // Pad the plaintext with 'X' characters if needed
            int paddingLength = plaintext.Length % matrixSize;
            if (paddingLength > 0)
            {
                plaintext += new string('X', matrixSize - paddingLength);
            }

            string ciphertext = "";

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
                    result[i] += (matrix[i, j] * vector[j]);
                }
                result[i] = (result[i] % 26 + 26) % 26;
            }
            return result;
        }

        public string Decrypt(string ciphertext)
        {
            int determinant = Determinant(keyMatrix, matrixSize);
            if (determinant == 0)
            {
                return "Decryption failed: Key matrix is not invertible.";
            }

            string plaintext = "";
            int[,] inverseMatrix = InverseMatrix(keyMatrix);

            for (int i = 0; i < ciphertext.Length; i += matrixSize)
            {
                int[] vector = new int[matrixSize];
                for (int j = 0; j < matrixSize && i + j < ciphertext.Length; j++)
                {
                    vector[j] = CharToNumber(ciphertext[i + j]);
                }

                int[] resultVector = MultiplyMatrix(inverseMatrix, vector);

                // Remove padding characters from the decrypted plaintext
                for (int j = 0; j < matrixSize; j++)
                {
                    if (resultVector[j] == CharToNumber('X'))
                    {
                        break;
                    }
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

            int[,] temp = new int[n, n];
            int sign = 1;

            for (int f = 0; f < n; f++)
            {
                GetCofactor(matrix, temp, 0, f, n);
                det += sign * matrix[0, f] * Determinant(temp, n - 1);
                sign = -sign;
            }

            det %= 26;
            if (det < 0) det += 26;
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
            int[,] adj = new int[matrixSize, matrixSize];
            if (matrixSize == 1)
            {
                adj[0, 0] = 1;
                return adj;
            }

            int[,] temp = new int[matrixSize, matrixSize];
            int sign;

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    GetCofactor(matrix, temp, i, j, matrixSize);
                    sign = ((i + j) % 2 == 0) ? 1 : -1;
                    adj[j, i] = (sign * Determinant(temp, matrixSize - 1) + 26) % 26;
                }
            }

            return adj;
        }

        /*private int ModularInverse(int a, int m)
        {
            MessageBox.Show($"{a}, {m}");
            int m0 = m, t, q;
            int x0 = 0, x1 = 1;

            if (m == 1) return 0;

            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m; a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0) x1 += m0;
            return x1;
        }*/

        private int CharToNumber(char c)
        {
            return c - 'A';
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
        private char NumberToChar(int n)
        {
            return (char)((n + 26) % 26 + 'A');
        }
    }
}


