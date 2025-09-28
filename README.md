# StajyerTakip Uygulaması

**Repo:** [https://github.com/zeynepozdemir01/StajyerTakip](https://github.com/zeynepozdemir01/StajyerTakip)

Bu proje, stajyer yönetimini kolaylaştırmak için geliştirilmiş **full-stack** bir uygulamadır.  
Stajyerlerin kaydını tutar, listeleme / filtreleme / sıralama yapar, düzenleme ve silme işlemlerini sağlar.  

---

## Teknik Mimari

### Frontend (stajyer-takip-ui)
- **React + Vite**
- React Router DOM (sayfa yönlendirme)
- Axios (API istekleri)
- Responsive tasarım (basit grid yapısı)
- JWT ile kimlik doğrulama (Bearer Token)
- Yetkisiz kullanıcıları login sayfasına yönlendiren `ProtectedRoute` bileşeni

### Backend (StajyerTakip.Api)
- **ASP.NET Core 8.0 Web API**
- CQRS + MediatR kullanımı
- JWT tabanlı Authentication / Authorization
- Swagger/OpenAPI 3.0 ile API dokümantasyonu
- Katmanlı Mimari (Domain, Application, Infrastructure)

### Veritabanı
- **Microsoft SQL Server**
- Entity Framework Core (Code-First + Migrations)
- Unique index: `NationalId`, `Email`
- Validation: `[Required]`, `[StringLength(11)]`, `[RegularExpression]` ile TC Kimlik kontrolü

---

## Özellikler

**JWT Login / Logout**  
**CRUD** – Ekleme, Listeleme, Güncelleme, Silme  
**Arama, filtreleme, sayfalama ve sıralama**  
**Kullanıcı koruması** – Token olmadan listeye erişim engellenir  
**Swagger UI** – API testleri kolayca yapılabilir  
**React UI** – Modern, SPA tabanlı frontend  

---

## Proje Yapısı

StajyerTakip/
├── StajyerTakip.Api/ # Web API katmanı
├── StajyerTakip.Application/ # CQRS + business logic
├── StajyerTakip.Domain/ # Entity ve Validation
├── StajyerTakip.Infrastructure/# EF Core, DbContext, Repository
├── stajyer-takip-ui/ # React uygulaması
└── README.md

---

## Backend Kurulum

1. Gereklilikler  
   - .NET 8 SDK  
   - SQL Server (Express, LocalDB veya kurumsal)  
   - Node.js (16+ sürüm) ve npm
   - Tarayıcı: Chrome / Edge (React dev server için önerilir)

2. Bağımlılıkların yüklenmesi  
```bash
dotnet restore
```

3. Veritabanı migration işlemi  
```bash
dotnet ef database update -p StajyerTakip.Infrastructure -s StajyerTakip.Api
```

4. Konfigürasyon (`appsettings.json`)  
```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=StajyerTakipDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "StajyerTakip",
    "Audience": "StajyerTakipClient",
    "Secret": "supersecretkey123456789"
  }
}
```

5. Çalıştırma  
```bash
dotnet run 
```
Uygulama çalıştırıldıktan sonra tarayıcıda https://localhost:{port} adresi üzerinden erişilebilir.
({port} bilgisayarda rastgele atanabilir, örn: 7281 veya 5281)

```bash
dotnet run --project StajyerTakip.Api --launch-profile https
```
Swagger’a şu adresten eriş:
https://localhost:7007/swagger


6. Giriş  
- Kullanıcı adı: `admin`  
- Şifre: `12345` 

---

## Frontend (React UI) Kurulum

1. React projesi dizinine geç
cd stajyer-takip-ui

2. Bağımlılıkları yükle
npm install

3. Çevre değişkeni ayarla (.env dosyası oluştur)
VITE_API_URL=https://localhost:7007/api

4. Çalıştır
npm run dev
Tarayıcıda şu adrese git:
http://localhost:5173

---

## Giriş Bilgileri

Email: demo@stajyer.local

Şifre: Password123!

Başarılı girişten sonra token localStorage’a kaydedilir.
API çağrıları otomatik olarak Authorization: Bearer <token> başlığı ile yapılır.

---

**Geliştirici:** İrem Zeynep Özdemir  
**Tarih:** 29.09.2025
