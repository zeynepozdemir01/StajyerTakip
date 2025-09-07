# StajyerTakip Uygulaması

**Repo Linki:** [https://github.com/zeynepozdemir01/StajyerTakip](https://github.com/zeynepozdemir01/StajyerTakip)

Bu proje, stajyer yönetimini kolaylaştırmak amacıyla geliştirilmiş bir **ASP.NET Core MVC** uygulamasıdır.  
Stajyerlerin bilgilerini kayıt altına almak, listelemek, düzenlemek ve dışa/içe aktarmak için kullanılabilir.  


---

## Teknik Mimarisi

**Frontend**
- ASP.NET MVC View (Razor)
- HTML, CSS, Bootstrap 5
- Bootstrap Icons
- jQuery Unobtrusive Validation (istemci tarafı doğrulama)

**Backend**
- ASP.NET Core 8.0 (MVC pattern)
- Katmanlı yapı (Controller + Model + Service + DbContext)
- Cookie tabanlı Authentication / Authorization

**Veritabanı**
- Microsoft SQL Server
- Entity Framework Core (Code-First, Migrations)
- Unique index: `NationalId`, `Email`

---

## Özellikler

- CRUD (Ekleme, Listeleme, Güncelleme, Silme)  
- Form validasyonları (zorunlu alan, e-posta formatı, TC kimlik kontrolü, tarih kuralları)  
- Listeleme sayfasında:
  - Arama (Ad, Soyad, Email, Telefon, Okul, Bölüm)  
  - Durum filtresi (Aktif / Pasif)  
  - Sayfalama ve sıralama  
- Dışa aktarma:
  - CSV formatında indirme  
  - Excel (.xlsx) formatında indirme  
- İçe aktarma:
  - CSV dosyasından toplu stajyer yükleme  
  - CSV şablon indirilebilme  
- Kullanıcı girişi:
  - Login / Logout  
  - Cookie tabanlı kimlik doğrulama  
  - Basit konfigürasyon (`appsettings.json` → `Auth:Username` & `Auth:Password`)  

---

## Proje Yapısı

StajyerTakip/
├── Controllers/
│   ├── AccountController.cs
│   └── InternsController.cs
├── Data/
│   ├── AppDbContext.cs
│   └── DbSeeder.cs
├── Models/
│   ├── Intern.cs
│   └── ViewModels/
│       ├── InternListVm.cs
│       ├── LoginVm.cs
│       └── ImportResultVm.cs
├── Services/
│   ├── IInternService.cs
│   └── InternService.cs
├── Views/
│   ├── Interns/ (Index, Create, Edit, Details, Delete, UploadCsv, …)
│   ├── Account/ (Login, AccessDenied)
│   └── Shared/ (_Layout.cshtml, _ValidationScriptsPartial.cshtml)
└── wwwroot/ (css, js, bootstrap, icons)

---

## Kurulum

1. Gereklilikler  
   - .NET 8 SDK  
   - SQL Server (Express, LocalDB veya kurumsal)  

2. Bağımlılıkların yüklenmesi  
```bash
dotnet restore
```

3. Veritabanı migration işlemi  
```bash
dotnet ef database update
```

4. Konfigürasyon (`appsettings.json`)  
```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=StajyerTakipDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Auth": {
    "Username": "admin",
    "Password": "12345",
    "DisplayName": "Admin Kullanıcı"
  }
}
```

5. Çalıştırma  
```bash
dotnet run
```
Uygulama çalıştırıldıktan sonra tarayıcıda https://localhost:{port} adresi üzerinden erişilebilir.
({port} bilgisayarda rastgele atanabilir, örn: 7281 veya 5281)


6. Giriş  
- Kullanıcı adı: `admin`  
- Şifre: `12345`  

---

## Notlar

- CSV içe aktarma için şablon dosya indirilebilir: `/Interns/DownloadCsvTemplate`.  
- Proje, temel işlevler için hazırdır. Geliştirmeler (grafikler, dashboard, roller bazlı yetkilendirme vb.) isteğe bağlı olarak eklenebilir.  
- Kod yapısı MVC prensiplerine uygun olup okunabilirlik gözetilmiştir.  

---

**Geliştirici:** İrem Zeynep Özdemir  
**Tarih:** 05.09.2025
