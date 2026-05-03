---
title: "RentalApp readme"
parent: RentalApp
grand_parent: C# practice
nav_order: 5
mermaid: true
---

# RentalApp

The purpose of this app is to provide a .NET MAUI rental marketplace client. It has grown from the
StarterApp template into an Android-focused app for browsing items, creating listings, requesting
rentals, managing rental status, and reviewing completed rentals. It provides features including:

* Login and registration through the SET09102 API
* Item browsing, item creation, item editing, categories, reviews, and nearby-item search
* Rental request creation plus incoming and outgoing rental management
* Rental status rules implemented in `StarterApp.Core`
* PostgreSQL / Entity Framework Core migration support for the optional local database project
* xUnit tests for item validation, review validation, rental pricing, overdue rules, and rental state transitions

The mobile app currently targets Android using .NET MAUI and .NET 10. The main rental, item, review,
category, nearby-search, and authentication workflows call the remote SET09102 API at
`https://set09102-api.b-davison.workers.dev`. The solution also contains a PostgreSQL-backed Entity
Framework Core database project and a separate migrations runner for local development, schema practice,
and coursework architecture evidence. The hosted API should be treated as the primary backend for the
implemented mobile app features.

To fully understand how it works, you should follow an appropriate set of tutorials such as
[this one](https://edinburgh-napier.github.io/SET09102/tutorials/csharp/) which covers the main
concepts and techniques used here. However, if you want to jump straight in and work out any problems
as you go along, that will also work. The main app follows the MAUI + MVVM pattern, with services and
repositories handling API access and `StarterApp.Core` holding the domain rules that are covered by
unit tests.

You can use any development environment with this project including:

* [Rider](https://www.jetbrains.com/rider/)
* [Visual Studio](https://visualstudio.microsoft.com/)
* [Visual Studio Code](https://code.visualstudio.com/)

The instructions assume you will be using VS Code since that is the lowest-common-denominator choice.

## Project Reality

This project contains both remote API-backed app functionality and local database/migration support.

The implemented mobile marketplace features primarily use the hosted SET09102 API. This means the app
does not require a locally hosted API server for normal mobile development. The local PostgreSQL database
exists as part of the StarterApp/coursework structure and is useful for migrations, schema practice,
database evidence, and local development work, but it is not the main runtime data source for the
implemented app workflows.

In practical terms:

* Use the hosted API for app features.
* Use the local database project for EF Core migration/schema work.
* Do not debug PostgreSQL first when an API-backed app feature fails.
* Check API requests, JWT tokens, endpoint URLs, and request bodies first.
* Keep the local database configuration consistent with `docker-compose.yml`.

## Compatibility

This app is built using the following tool versions.

| Name | Version |
|---|---:|
| [.NET](https://dotnet.microsoft.com/en-us/) | 10.0 |
| [.NET MAUI workload](https://learn.microsoft.com/dotnet/maui/) | Android |
| [PostgreSQL Docker image](https://hub.docker.com/_/postgres) | 16 |

## Architecture Overview

The app follows an MVVM-style structure with service and repository/API-client abstractions.

The main runtime flow is:

```text
Views
↓
ViewModels
↓
Services
↓
Repositories / API Client
↓
Hosted SET09102 API