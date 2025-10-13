# Redis Cache in .NET API

## Project Overview

This repository provides a hands-on guide and reference implementation for integrating **Redis** as a **Distributed Cache** layer into a **.NET Web API**. 

The core goal is to showcase how to achieve near-instantaneous API response times by implementing the **Generic Cache Repository Pattern** and the **Double Invalidation** strategy. The solution successfully demonstrates moving the workload of fetching **1000 records** from a simulated database to an in-memory cache, proving the performance benefit of a Cache Hit.

### Key Architectural Highlights:
* **Generic Cache Abstraction:** Uses `ICacheRepository<T>` to isolate the `UserService` from Redis technology.
* **Double Invalidation:** Ensures data consistency by invalidating both the single item key and the full collection key (`all_users_list`) on any write operation (`POST`, `PUT`, `DELETE`).

---

## 📚 Read the Full Guide

For a detailed, step-by-step tutorial on how this project was built, including the Docker setup, code explanations, and the analysis of the performance gains (Cache Miss vs. Cache Hit), check out the full article:

> **[Article](https://dev.to/joaooliveiratech/distributed-cache-in-net-how-to-configure-and-use-redis-3ehj)**

---

## Prerequisites

To run this project, you need to have installed:

1.  **[.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)**
2.  **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** (Required to run the Redis container)

---

## How to Run the Project Locally

Follow these steps in your terminal:

### Step 1: Start the Redis Container

```bash
docker run --name my-redis -d -p 6379:6379 redis
```

### Step 2: Start the .NET API

Navigate to the solution root (RedisCacheApi) and start the API project.

```bash
# Example command from the solution root
dotnet run --project src/RedisCache.Api/RedisCache.Api.csproj
```


