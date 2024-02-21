# Encryption Algorithms in [Encreption-Methods]

This repository explores various encryption algorithms, including:

* Hill Cipher
* Caesar Cipher
* RSA

These algorithms provide different levels of security and complexity, making them suitable for diverse use cases.

# Usage
Brief examples of how to use each algorithm:

# Hill Cipher
Encrypt: HillCipher.Encrypt("YourMessage", keyMatrix)
Decrypt: HillCipher.Decrypt("EncryptedMessage", keyMatrix)

# Caesar Cipher
Encrypt: CaesarCipher.Encrypt("YourMessage", shift)
Decrypt: CaesarCipher.Decrypt("EncryptedMessage", shift)

# RSA
Generate keys: See RsaModel for key generation logic.
Encrypt: RsaCipherViewModel.Encrypt("YourMessage", "p,q,e")
Decrypt: RsaCipherViewModel.Decrypt("EncryptedMessage", "p,q,e")

Replace "YourMessage", keyMatrix, shift, and "p,q,e" with your actual data and RSA parameters.

# Authors
AL Mahdi Al Zaalouk - AlMahdi-AlZaalouk
