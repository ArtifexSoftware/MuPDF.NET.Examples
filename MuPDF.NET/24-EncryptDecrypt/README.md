# 24-EncryptDecrypt

Save a PDF with AES-256 encryption, authenticate, then save without encryption.

## Sample method

`EncryptDecrypt()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/encrypt-decrypt.pdf` |
| Output | `Output/MuPDF.NET/24-EncryptDecrypt/encrypted.pdf`, `decrypted.pdf`, `original.png`, `decrypted.png` |
| Expected | `encrypt-decrypt.txt` + decrypted fingerprint + PNG SHA-256 |

## Run

```powershell
dotnet run --project MuPDF.NET\24-EncryptDecrypt
```

Recreate the fixture: `python Test/create_encrypt_decrypt_pdf.py` from `D:\Artifex\Work`.

Passwords on `encrypted.pdf`: user `user-secret`, owner `owner-secret`.

## APIs used

- `Document.Save` (`encryption`, `ownerPW`, `userPW`)
- `Document.Authenticate`
- `Document.NeedsPass` / `IsEncrypted`
- `Constants.PDF_ENCRYPT_AES_256` / `PDF_ENCRYPT_NONE`
