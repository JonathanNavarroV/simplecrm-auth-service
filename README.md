# SimpleCRM Auth Service

**SimpleCRM Auth Service** es el módulo de autenticación y gestión de usuarios dentro del ecosistema **SimpleCRM**.
Construido con **.NET 9** y **PostgreSQL**, este servicio se encarga de la emisión y validación de **tokens JWT** internos, la gestión de credenciales y usuarios, y el registro de eventos de autenticación.

---

## 🚀 Funcionalidades principales

- Emisión de **tokens JWT** internos para comunicación entre microservicios.
- Validación de credenciales contra la base de datos propia.
- Sincronización opcional con **Microsoft Entra ID** (Azure AD).
- Gestión de usuarios y roles.
- Registro de eventos de login/logout para auditoría.
- Endpoint de salud (`/healthz`) para monitoreo.

---

## 📂 Estructura del proyecto

``` text
simplecrm-auth-service/
├─ src/
│  ├─ Application/               # Lógica de aplicación (CQRS, validaciones)
│  ├─ Domain/                    # Entidades de dominio
│  ├─ Infrastructure/            # Persistencia, acceso a datos (EF Core, PostgreSQL)
│  ├─ Presentation/              # API REST (ASP.NET Core Controllers)
│  ├─ SimpleCRM.AuthService.sln
├─ tests/                        # Tests unitarios y de integración (xUnit)
├─ .gitignore
└─ README.md                     # Documentación del proyecto
```

---

## ⚙️ Requisitos previos

- .NET SDK 9.0
- PostgreSQL (ejecutándose en Docker o instancia local)
- Cuenta y App registrada en Microsoft Entra ID

---

## ▶️ Ejecución en desarrollo

### 1. Clona este repositorio

```bash
git clone git@github.com:JonathanNavarroV/simplecrm-auth-service.git
cd simplecrm-auth-service/src
```

### 2. Configura la cadena de conexión en `appsettings.json` o usando `dotnet-user-secrets`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=auth_db;Username=postgres;Password=postgres"
}
```

### 3. Ejecuta migraciones de base de datos:

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```

### 4. Ejecuta el proyecto

``` bash
dotnet run --project Presentation
```

### 5. Prueba el endpoint de salud:

``` bash
curl http://localhost:5001/healthz
```

---

## 🔗 Repositorios relacionados

- [simplecrm-frontend](https://github.com/JonathanNavarroV/simplecrm-frontend)
- [simplecrm-gateway](https://github.com/JonathanNavarroV/simplecrm-gateway)
- [simplecrm-crm-service](https://github.com/JonathanNavarroV/simplecrm-crm-service)
- [simplecrm-infra](https://github.com/JonathanNavarroV/simplecrm-crm-infra)

---

## ✨ Autor

[Jonathan Navarro](https://github.com/JonathanNavarroV)
