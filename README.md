# IdentityDataProtectionApi

Basit ve öğretici bir **.NET 8 Web API** projesi. Kullanıcı kayıt/giriş işlemleri için **Entity Framework Core (Code‑First)**, parolaları güvenle saklamak için **ASP.NET Core Identity’nin PasswordHasher’ı (PBKDF2)** ve **Data Protection** kullanır.

> Bu repo, *Pratik - Identity and Data Protection* ödevi içindir. Amaç: `User` tablosu, Model Validation, parola hashleme ve temel CRUD/kimlik işlemleri.

---

## 🚀 Özellikler

* **User** tablosu: `Id`, `Email` (unique), `PasswordHash` (düz şifre tutulmaz)
* **Kayıt / Giriş** endpoint’leri (validation dâhil)
* Parola hashleme: `PasswordHasher<User>` → PBKDF2, versiyonlu format (DB’de `AQAAAA...` gibi görünür)
* **Data Protection** servisleri hazır (token/şifreleme senaryoları için)
* **Swagger/OpenAPI** ile test
* İlk çalıştırmada **seed** 2 kullanıcı (yalnızca demo):

  * `user@example.com` / `P@ssw0rd!`
  * `admin@example.com` / `Admin123!`

> Seed edilen parolalar yalnızca geliştirme/demodur ve DB’de **hash** olarak saklanır.

---

## 🧱 Teknolojiler

* .NET 8
* **EF Core 9.0.8** (SqlServer, Design, Tools)
* ASP.NET Core Identity (yalnızca PasswordHasher)
* Swagger (Swashbuckle)

> Önemli: EF **Tools** sürümü ile NuGet EF sürümleriniz **aynı** olsun (ör: `9.0.8`).

---

## 📁 Proje yapısı

```
IdentityDataProtectionApi/
├─ Controllers/
│  └─ AuthController.cs      # register, login, getUser
├─ Data/
│  └─ AppDbContext.cs        # DbContext (Users)
├─ DTOs/
│  └─ AuthDtos.cs            # RegisterRequest, LoginRequest, UserResponse
├─ Models/
│  └─ User.cs                # User entity (Email unique, PasswordHash)
├─ appsettings.json          # Connection string
└─ Program.cs                # DI, EF, DataProtection, DB migrate & seed
```

---

## 🛠️ Gereksinimler

* .NET SDK **8.x**
* SQL Server (LocalDB/Express/Server)
* **dotnet-ef** CLI `9.0.8`
* Visual Studio 2022 17.10+ (veya CLI)

---

## ⚙️ Kurulum (Hızlı Başlangıç)

1. **Bağlantı dizesini** `appsettings.json` içinde ayarla:

   ```json
   {
     "ConnectionStrings": {
       "SqlServer": "Server=(localdb)\\MSSQLLocalDB;Database=IdentityDataProtectionDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
     }
   }
   ```
2. EF CLI aracını eşitle:

   ```bash
   dotnet tool uninstall -g dotnet-ef
   dotnet tool install    -g dotnet-ef --version 9.0.8
   ```
3. Paketleri geri yükle ve derle:

   ```bash
   dotnet restore
   dotnet build
   ```
4. **Migration** oluştur & veritabanını güncelle (proje klasöründe):

   ```bash
   dotnet ef migrations add InitialCreate --project . --startup-project .
   dotnet ef database update               --project . --startup-project .
   ```

   > Visual Studio PMC için:
   >
   > ```powershell
   > Add-Migration InitialCreate -Context AppDbContext
   > Update-Database
   > ```
5. Çalıştır:

   ```bash
   dotnet run
   ```
6. Swagger: `https://localhost:{port}/swagger`

---

## 🔌 API Uçları

### POST `/api/auth/register`

**Body**

```json
{
  "email": "newuser@example.com",
  "password": "Strong123!"
}
```

**201** →

```json
{ "id": 3, "email": "newuser@example.com" }
```

### POST `/api/auth/login`

**Body**

```json
{
  "email": "newuser@example.com",
  "password": "Strong123!"
}
```

**200** →

```json
{ "id": 3, "email": "newuser@example.com" }
```

### GET `/api/auth/{id}`

**200** →

```json
{ "id": 1, "email": "user@example.com" }
```

> **Validation**: `[Required]`, `[EmailAddress]`, `[MinLength]` vb. `[ApiController]` özelliği sayesinde otomatik 400 doğrulama hataları döner.

---

## 🔐 Güvenlik Notları

* Parolalar **asla düz metin** saklanmaz → yalnızca `PasswordHash` alanı tutulur.
* `PasswordHasher<T>` PBKDF2 kullanır; salt/iteration ve format sürümlüdür.
* `DataProtection` eklendi; token/şifreleme senaryolarında `IDataProtectionProvider` enjekte edilerek kullanılabilir.
* Örnek amaçlı seed kullanıcılar prod’da kaldırılmalıdır.

---

