# Distributed Cache in .NET: How to Configure and Use Redis

> **Article:** [Distributed Cache in .NET: How to Configure and Use Redis](https://dev.to/joaooliveiratech/distributed-cache-in-net-how-to-configure-and-use-redis-3ehj)

This repository provides a hands-on guide and reference implementation for integrating Redis as a Distributed Cache layer into a .NET Web API.

The project demonstrates how to achieve near-instantaneous response times by moving the workload of fetching heavy datasets (1,000+ records) from a simulated database to an in-memory cache, highlighting the significant performance gap between a Cache Miss and a Cache Hit.

---

## What This Project Covers

* Generic Cache Repository Pattern: Implementing `ICacheRepository<T>` to decouple business logic from the caching provider.
* Double Invalidation Strategy: Maintaining data consistency by clearing both individual keys and collection keys on data updates.
* Performance Benchmarking: Comparison of response times between database fetching and Redis retrieval.
* Serialization: Handling complex objects using JSON serialization for Redis storage.

---

## Concepts Overview

### Cache-Aside Pattern

The application first checks the cache. If the data is missing (Cache Miss), it fetches it from the database and stores it in the cache for future requests.

### Double Invalidation

To prevent "stale data" (showing old info after an update), this project implements a strategy where any write operation (`POST`, `PUT`, `DELETE`) automatically removes:

1. The specific item key (e.g., `user:123`)
2. The collection key (e.g., all_users_list)

This ensures that the next `GET` request is forced to fetch fresh data from the database.

---

## Tech Stack

* .NET 9 (Web API)
* Redis (Distributed Cache)
* StackExchange.Redis (Client library)
* Docker (Local environment)

---

## Environment Setup

### Prerequisites

* .NET 9 SDK
* Docker Desktop

### Step 1: Start Redis

Run the following command to start a local Redis instance:

```bash
docker run --name my-redis -d -p 6379:6379 redis
```

### Step 2: Run the API

Navigate to the project directory and execute:

```bash
dotnet run --project src/RedisCache.Api/RedisCache.Api.csproj
```

The API will be available (typically) at: http://localhost:5000 (check your launchSettings.json).

## About

This repository is part of my technical writing and learning notes.  
If you found it useful, consider starring the repo and sharing feedback.

* Author: Joao Oliveira
* Blog: <https://joaooliveira.net>
* Topics: .NET, Redis, Distributed Systems, Caching Patterns

## Contributing

Issues and pull requests are welcome.  
If you plan a larger change, please open an issue first so we can align on scope.

## License

Licensed under the **MIT License**. See the `LICENSE` file for details.
