# Вступне слово


# Conference Room Booking API

REST API для керування конференц-залами, бронюваннями та додатковими послугами.

Проєкт реалізований на **ASP.NET Core / .NET 10** з використанням багаторівневої архітектури та Entity Framework Core.

## Features

* Створення конференц-залів
* Редагування конференц-залів
* Деактивація конференц-залів
* Пошук доступних залів за:

  * датою та часом;
  * необхідною місткістю
* Створення бронювання
* Перевірка конфліктів бронювання
* Додавання додаткових послуг до бронювання
* Автоматичний розрахунок загальної вартості
* Керування додатковими послугами
* Звіти та аналітика:

  * дохід;
  * завантаженість залів;
  * популярність додаткових послуг
* Swagger UI для тестування API
* Entity Framework Core migrations
* SQLite database
* Централізована обробка винятків

## Architecture

Проєкт побудований за принципом розділення відповідальностей:

```text
ConferenceRoomBooking
│
├── src
│   ├── ConferenceRoomBooking.Api
│   │   ├── Controllers
│   │   ├── Middleware
│   │   └── Extensions
│   │
│   ├── ConferenceRoomBooking.Application
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Services
│   │   └── Exceptions
│   │
│   ├── ConferenceRoomBooking.Domain
│   │   ├── Entities
│   │   └── Enums
│   │
│   └── ConferenceRoomBooking.Infrastructure
│       ├── Persistence
│       ├── Configurations
│       ├── Repositories
│       └── Migrations
│
└── tests
    └── ConferenceRoomBooking.Tests
```

### Layers

**API**

HTTP endpoints, controllers, middleware та конфігурація застосунку.

**Application**

Бізнес-логіка, DTO, application services та інтерфейси репозиторіїв.

**Domain**

Основні сутності

**Infrastructure**

Робота з базою даних, Entity Framework Core, repositories та migrations.

**Tests**

Вимоги на тести не було


## Technologies

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core 10
* SQLite
* Swagger / OpenAPI
* Dependency Injection
* Git

## Getting Started

### Requirements

Перед запуском необхідно встановити:

* .NET 10 SDK
* Visual Studio 2026 або інше IDE з підтримкою .NET 10

### Clone repository

```bash
git clone <repository-url>
cd ConferenceRoomBooking
```

### Restore dependencies

```bash
dotnet restore
```

### Build project

```bash
dotnet build
```

### Run tests

```bash
dotnet test
```

### Run API

```bash
dotnet run --project src/ConferenceRoomBooking.Api
```

Після запуску API буде доступний за адресою, яку покаже консоль застосунку.

## Swagger

Після запуску API Swagger UI доступний за адресою:

```text
https://localhost:7269/swagger
```

Swagger дозволяє переглядати та тестувати всі доступні API endpoints безпосередньо з браузера.

## Database

Для зберігання даних використовується SQLite.

Database schema створюється за допомогою Entity Framework Core migrations.

Database file:
`conference-room-booking.db`

Для оновлення бази даних:

```bash
dotnet ef database update \
  --project src/ConferenceRoomBooking.Infrastructure \
  --startup-project src/ConferenceRoomBooking.Api
```

При запуску застосунку також виконується перевірка та ініціалізація початкових даних.

## Initial Data

Система містить початкові конференц-зали:

| Room   | Capacity | Base hourly price |
| ------ | -------: | ----------------: |
| Hall A |       50 |          2000 UAH |
| Hall B |      100 |          3500 UAH |
| Hall C |       30 |          1500 UAH |

Доступні додаткові послуги:

| Service   |   Price |
| --------- | ------: |
| Projector | 500 UAH |
| Wi-Fi     | 300 UAH |
| Sound     | 700 UAH |

## Pricing Rules

Вартість бронювання розраховується залежно від часу бронювання.

| Period      |          Price |
| ----------- | -------------: |
| 06:00–09:00 |           -10% |
| 09:00–18:00 | Standard price |
| 18:00–23:00 |           -20% |
| 12:00–14:00 |           +15% |

Вартість вибраних додаткових послуг додається до вартості оренди залу.

Для бронювання, яке охоплює декілька часових періодів, вартість розраховується окремо для відповідних інтервалів.

## API Endpoints

### Rooms

```text
POST   /api/rooms
GET    /api/rooms/{id}
PUT    /api/rooms/{id}
DELETE /api/rooms/{id}
GET    /api/rooms/available
```

### Bookings

```text
POST /api/bookings
GET  /api/bookings/{id}
```

### Services

```text
POST   /api/services
PUT    /api/services/{id}
DELETE /api/services/{id}
```

### Reports

```text
GET /api/reports/revenue
GET /api/reports/occupancy
GET /api/reports/services
```

Повний опис request/response моделей та параметрів доступний у Swagger UI.

## Booking Conflict Detection

Перед створенням бронювання система перевіряє, чи не існує іншого підтвердженого бронювання цього самого залу з перетином часових інтервалів.

Бронювання вважаються конфліктними, якщо:

```text
existing.StartAt < requested.EndAt
AND
existing.EndAt > requested.StartAt
```

Це дозволяє, наприклад, створити нове бронювання одразу після завершення попереднього.

## Error Handling

Помилки обробляються централізовано через middleware.

Залежно від типу помилки API повертає відповідний HTTP status code, наприклад:

* `400 Bad Request` для помилок валідації;
* `404 Not Found`, якщо ресурс не знайдено;
* `409 Conflict`, якщо зал уже заброньований на вибраний період;
* `500 Internal Server Error` для неочікуваних помилок.

## Project Goals

Основна мета проєкту:

* чисте розділення відповідальностей;
* підтримуваність коду;
* можливість подальшого масштабування;
* тестованість бізнес-логіки;
* централізована обробка помилок;
* використання dependency injection;
* робота з базою даних через Entity Framework Core.

## License

This project was created as a technical assignment.
