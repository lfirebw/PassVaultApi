Application for storing personal password with hight security and easy use , principal goal is make system to seek any password fasted.

## Folder Structure
src/
├── PasswordVault.API/              → Controllers, Auth, Middlewares
├── PasswordVault.Application/      → UseCases, Interfaces
├── PasswordVault.Domain/           → Entities, Enums, ValueObjects
├── PasswordVault.Infrastructure/  → EF, Crypto, Services, JWT
└── PasswordVault.Tests/            → Tests de dominio y aplicación


### Commands

docker run -d \
  -e Jwt__Key="otra-clave-segura" \
  -e Jwt__Issuer="https://prod-api.com" \
  -e Jwt__Audience="https://prod-api.com" \
  -p 5063:5063 \
  passwordlisting-api