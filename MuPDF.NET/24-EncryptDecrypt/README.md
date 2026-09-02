# 24-EncryptDecrypt

Save a PDF with AES-256 encryption, authenticate, then save without encryption.

## Sample method

`EncryptDecrypt()` in `Program.cs`.

## Package

- [MuPDF.NET](https://www.nuget.org/packages/MuPDF.NET)

## Input / output

| | Path |
|--|------|
| Input | `Input/MuPDF.NET/sample.pdf` |
| Output | `encrypted.pdf`, `decrypted.pdf` |
| Expected | `encrypt-decrypt.txt` + decrypted fingerprint |

## Run

```powershell
dotnet run --project MuPDF.NET\24-EncryptDecrypt
```

## APIs used

- `Document.Save` (`encryption`, `ownerPW`, `userPW`)
- `Document.Authenticate`
- `Constants.PDF_ENCRYPT_AES_256` / `PDF_ENCRYPT_NONE`
