# FanStation WiFi Web API

REST API для управления системой вентиляции FanStationWifi.

## Технологии

- .NET 10
- Entity Framework Core 10.0.12
- PostgreSQL с Npgsql
- AutoMapper 16.2.0
- Swashbuckle.AspNetCore 10.2.3

## Структура проекта

```
FanStationWifi.WebApi/
├── Controllers/          # REST контроллеры
│   ├── ProfilesController.cs
│   ├── SettingsController.cs
│   ├── SensorsController.cs
│   ├── SensorTypesController.cs
│   ├── PeriodsController.cs
│   └── HistoryController.cs
├── Services/             # Сервисы бизнес-логики
│   ├── IProfileService.cs
│   ├── ProfileService.cs
│   ├── ISettingsService.cs
│   ├── SettingsService.cs
│   ├── ISensorService.cs
│   ├── SensorService.cs
│   ├── ISensorTypeService.cs
│   ├── SensorTypeService.cs
│   ├── IPeriodService.cs
│   ├── PeriodService.cs
│   └── ISensorHistoryService.cs
├── DTOs/                 # Объекты для передачи данных
│   ├── ProfileDTO.cs
│   ├── SettingsDTO.cs
│   ├── SensorDTO.cs
│   ├── SensorTypeDTO.cs
│   ├── PeriodDTO.cs
│   ├── SensorHistoryDTO.cs
│   └── Create DTOs
├── Extensions/           # Расширения
│   ├── AutoMapperExtensions.cs
│   ├── AuthExtensions.cs
│   ├── ApiKeyAuthenticationHandler.cs
├── Program.cs
├── appsettings.json
└── FanStationWifi.WebApi.csproj
```

## Конфигурация

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost:5432;Database=Climat;Username=postgres;Password=yourpassword"
  },
  "ApiSettings": {
    "ApiKey": "fanstation-secure-api-key-2024"
  }
}
```

### Старт

```bash
dotnet run --project FanStationWifi.WebApi
```

API будет доступен по адресу `http://localhost:5000`

## Авторизация

API использует API Key для аутентификации. Добавьте заголовок `X-API-KEY` со значением ключа из конфигурации.

**Пример:**

```bash
curl http://localhost:5000/api/profiles -H "X-API-KEY: fanstation-secure-api-key-2024"
```

## API Endpoints

### ProfilesController

| Endpoint | Method | Описание | Авторизация |
|----------|--------|----------|-------------|
| `/api/profiles` | GET | Получить все профили | ✓ |
| `/api/profiles/{id}` | GET | Получить профиль по ID | ✓ |
| `/api/profiles` | POST | Создать новый профиль | ✓ |
| `/api/profiles/{id}` | PUT | Обновить профиль | ✓ |
| `/api/profiles/{id}` | DELETE | Удалить профиль | ✓ |

### SettingsController

| Endpoint | Method | Описание | Авторизация |
|----------|--------|----------|-------------|
| `/api/settings/{profileId}` | GET | Получить настройки профиля | ✓ |
| `/api/settings/{profileId}` | POST | Создать настройки | ✓ |
| `/api/settings/{profileId}` | PUT | Обновить настройки | ✓ |

### SensorsController

| Endpoint | Method | Описание | Авторизация |
|----------|--------|----------|-------------|
| `/api/sensors` | GET | Получить все сенсоры | ✓ |
| `/api/sensors/{id}` | GET | Получить сенсор по ID | ✓ |
| `/api/sensors` | POST | Создать сенсор | ✓ |
| `/api/sensors/{id}` | PUT | Обновить сенсор | ✓ |
| `/api/sensors/{id}` | DELETE | Удалить сенсор | ✓ |

### SensorTypesController

| Endpoint | Method | Описание | Авторизация |
|----------|--------|----------|-------------|
| `/api/sensortypes` | GET | Получить все типы сенсоров | ✓ |
| `/api/sensortypes/{id}` | GET | Получить тип сенсора по ID | ✓ |
| `/api/sensortypes` | POST | Создать тип сенсора | ✓ |
| `/api/sensortypes/{id}` | PUT | Обновить тип сенсора | ✓ |
| `/api/sensortypes/{id}` | DELETE | Удалить тип сенсора | ✓ |

### PeriodsController

| Endpoint | Method | Описание | Авторизация |
|----------|--------|----------|-------------|
| `/api/periods/{profileId}` | GET | Получить все периоды профиля | ✓ |
| `/api/periods/id/{id}` | GET | Получить период по ID | ✓ |
| `/api/periods/{profileId}` | POST | Создать период | ✓ |
| `/api/periods/id/{id}` | PUT | Обновить период | ✓ |
| `/api/periods/id/{id}` | DELETE | Удалить период | ✓ |

### HistoryController

| Endpoint | Method | Описание | Авторизация |
|----------|--------|----------|-------------|
| `/api/history/{sensorId}` | GET | Получить историю сенсора | ✓ |
| `/api/history/{sensorId}/range` | GET | Получить историю за диапазон дат | ✓ |

## Примеры запросов

### GET все профили

```bash
curl http://localhost:5000/api/profiles -H "X-API-KEY: fanstation-secure-api-key-2024"
```

**Ответ:**

```json
{
  "id": 1,
  "name": "Обычный",
  "isActive": true,
  "order": 0,
  "createAt": "2026-09-27T17:29:00",
  "updateAt": "2026-09-27T17:30:00"
}
```

### POST создать профиль

```bash
curl -X POST http://localhost:5000/api/profiles \
  -H "X-API-KEY: fanstation-secure-api-key-2024" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Специальный",
    "isActive": true,
    "order": 1
  }'
```

### PUT обновить профиль

```bash
curl -X PUT http://localhost:5000/api/profiles/1 \
  -H "X-API-KEY: fanstation-secure-api-key-2024" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Обычный",
    "isActive": true,
    "order": 0
  }'
```

### DELETE профиль

```bash
curl -X DELETE http://localhost:5000/api/profiles/1 \
  -H "X-API-KEY: fanstation-secure-api-key-2024"
```

### GET история сенсора

```bash
curl http://localhost:5000/api/history/1 -H "X-API-KEY: fanstation-secure-api-key-2024"
```

**Ответ:**

```json
{
  "id": 1,
  "sensorId": 1,
  "date": "2026-09-27T17:35:00",
  "value": 23.5
}
```

## Swagger UI

API автоматически генерирует документацию Swagger. Откройте:

```
http://localhost:5000/swagger
```

## Миграция базы данных

API автоматически выполняет миграции при старте. Если нужно выполнить миграции вручную:

```bash
dotnet ef database update --project FanModelDB --startup-project FanStationWifi.WebApi
```

## Зависимости

- **Microsoft.EntityFrameworkCore** 10.0.12
- **Npgsql.EntityFrameworkCore.PostgreSQL** 10.0.3
- **AutoMapper** 16.2.0
- **AutoMapper.Extensions.Microsoft.DependencyInjection** 12.0.1
- **Swashbuckle.AspNetCore** 10.2.3
- **Microsoft.Extensions.Configuration.Json** 10.0.12

## Статус сборки

✓ Сборка успешно завершена

Предупреждения:
- AutoMapper.Extensions.Microsoft.DependencyInjection требует версию 12.0.1, но используется 16.2.0 (совместимо)
- Microsoft.Extensions.Configuration.Json добавлен вручную, но является частью .NET SDK (не влияет)