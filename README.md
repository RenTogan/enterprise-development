# Автомойка — Серверное приложение (Лабораторная работа №2)

REST API для автомойки на базе ASP.NET Core (.NET 10)

## Реализованный функционал
- Базовые CRUD-операции для сущностей:
  - `/api/clients` - клиенты и их автомобили
  - `/api/cars` - автомобили
  - `/api/services` - услуги мойки
  - `/api/orders` - заказы
- Аналитические запросы:
  - `GET /api/analytics/top-clients` - топ-5 клиентов по количеству посещений
  - `GET /api/analytics/current-cars` - автомобили на мойке прямо сейчас
  - `GET /api/analytics/popular-services` - топ-5 наиболее популярных услуг
  - `GET /api/analytics/box-free-time` - время освобождения выбранного бокса
  - `GET /api/analytics/service-revenues` - суммарная выручка по каждой услуге
- Интерактивная документация Scalar доступна по адресу `http://localhost:5000/scalar`