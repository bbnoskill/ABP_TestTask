# Conference Booking API

API для управління бронюванням та орендою конференц-залів.

## Технології

- .NET 8, ASP.NET Core Web API
- Entity Framework Core + SQLite
- AutoMapper, FluentValidation
- xUnit, Moq, FluentAssertions
- Swagger / OpenAPI

## Архітектура

Clean Architecture з 4 шарами:

```
API → Application → Domain
       ↑
Infrastructure
```

- **Domain** — сутності, інтерфейси, виключення (нульові залежності)
- **Application** — бізнес-логіка, DTOs, валідація, маппінг
- **Infrastructure** — EF Core, репозиторії, SQLite
- **API** — контролери, middleware, Swagger

## Запуск

```bash
dotnet restore
dotnet build
dotnet run --project src/ConferenceBooking.API
```

Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

### Конференц-зали

| Метод | URL | Опис |
|-------|-----|------|
| GET | `/api/conferencerooms` | Всі зали |
| GET | `/api/conferencerooms/{id}` | Зал за ID |
| GET | `/api/conferencerooms/available` | Пошук доступних (query: date, startTime, endTime, capacity) |
| POST | `/api/conferencerooms` | Створити зал |
| PUT | `/api/conferencerooms/{id}` | Оновити зал |
| DELETE | `/api/conferencerooms/{id}` | Видалити зал (soft delete) |

### Бронювання

| Метод | URL | Опис |
|-------|-----|------|
| GET | `/api/bookings/{id}` | Бронювання за ID |
| POST | `/api/bookings` | Створити бронювання (автоматичний розрахунок вартості) |
| DELETE | `/api/bookings/{id}` | Скасувати бронювання |

### Звіти

| Метод | URL | Опис |
|-------|-----|------|
| GET | `/api/reports/room-usage` | Використання залів за період |
| GET | `/api/reports/revenue` | Доходи за період |
| GET | `/api/reports/popular-services` | Популярні послуги |

## Розрахунок вартості

Ціна = Σ(БазоваСтавка × Коефіцієнт × Годин) + Σ(ВартістьПослуг)

| Часовий слот | Години | Коефіцієнт |
|-------------|--------|-----------|
| Ранок | 06:00–09:00 | 0.9 |
| Стандарт | 09:00–18:00 | 1.0 |
| Пік | 12:00–14:00 | 1.15 |
| Вечір | 18:00–23:00 | 0.8 |

> Пікові години мають пріоритет над стандартними.

## Початкові дані

| Зал | Місткість | Ставка |
|-----|-----------|--------|
| Зал А | 50 | 2000 грн/год |
| Зал B | 100 | 3500 грн/год |
| Зал C | 30 | 1500 грн/год |

| Послуга | Ціна |
|---------|------|
| Проєктор | 500 грн |
| Wi-Fi | 300 грн |
| Звук | 700 грн |

## Тестування

```bash
dotnet test
```
