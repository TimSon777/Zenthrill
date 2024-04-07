## Миграции БД auth

1. cd src/backend/Zenthrill.Auth.Model
2. dotnet ef migrations add Init -o Infrastructure/EntityFrameworkCore/Migrations --startup-project ../Zenthrill.Auth.WebAPI
