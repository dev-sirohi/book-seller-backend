# 📚 Book Seller Backend (BSB)

A backend system for a book-selling platform built with **.NET (C#)** and **SQL Server**, focused on **custom data access abstractions, transactional workflows, and flexible query construction**.

This project explores building backend systems **without relying heavily on ORMs**, emphasizing control over database interaction and query execution.

---

## ⚙️ Tech Stack

- .NET / ASP.NET Core
- SQL Server
- Custom data access layer (no Entity Framework abstraction)
- ADO.NET-style connections + transactions

---

## 🧠 Core Ideas

This project is built around a few deliberate design choices:

- Explicit control over database queries
- Transaction-first service design
- Custom query builder abstraction
- Consistent API response handling

---

## 🏗️ Architecture Overview

Controllers  
    ↓  
Application Services  
    ↓  
Custom DB Layer (Command Builders)  
    ↓  
SQL Server

---

## 🔧 Key Components

### 1. Result Wrapper

Standardized API responses using a wrapper:

public sealed class ResultWrapper

Provides:

- `Success`
- `Message`
- `Data`

This ensures consistency across all endpoints.

---

### 2. Custom DB Command System

Instead of using an ORM, queries are constructed using a **Command Builder pattern**:

DBCommandFactory.CommandBuilder

Example capabilities:

- Dynamic column selection
- Parameterized queries
- Where clause construction
- Query sanitization

Reference:

---

### 3. Transaction Handling

All critical operations are wrapped in transactions:

using (IDBTransaction tx = _cn.BeginTransaction())

Ensures:

- atomic operations
- consistency across multi-step workflows

---

### 4. Utility Layer

Includes helpers for:

- Object transformation via JSON serialization
- Username resolution
- DataTable → object mapping
- SQL parameter conversion

---

### 5. Data Mapping

Instead of ORM mapping, this project uses:

DataTable → ExpandoObject → Typed Models

This provides flexibility while maintaining control over schema handling.

---

## 🔐 Authentication

Basic authentication flow implemented via service layer:

- Login endpoint with validation
- Token-based system (extensible)
- Transaction-backed authentication logic

---

## 🧪 Design Philosophy

This project intentionally avoids heavy abstractions like Entity Framework to:

- understand how queries are actually executed
- control performance characteristics
- experiment with custom query pipelines

---

## ⚠️ Trade-offs

This approach introduces:

- more boilerplate compared to ORMs
- manual query handling complexity
- higher responsibility for correctness

But enables:

- fine-grained control
- predictable query behavior
- deeper understanding of data access

---

## 🚀 Running the Project

dotnet run

Ensure:

- SQL Server is running
- Connection string is configured in `appsettings.json`

---

## 📌 Future Improvements

- Proper password hashing (currently placeholder)
- Token-based authentication (JWT)
- Repository abstraction (optional)
- Query optimization and indexing
- Input validation hardening

---

## One-line summary

> A backend system focused on understanding database interaction by building a custom query and transaction layer instead of relying on ORMs.
