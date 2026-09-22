# RecordDB.API

![.NET 10](https://img.shields.io/badge/.NET%2010-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![C#](https://img.shields.io/badge/C%23%2013-239120?logo=csharp&logoColor=white&style=for-the-badge)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white&style=for-the-badge)
![Scalar](https://img.shields.io/badge/Scalar-API%20Reference-E9573F?style=for-the-badge)
![ReDoc](https://img.shields.io/badge/ReDoc-API%20Docs-263238?logo=openapiinitiative&logoColor=white&style=for-the-badge)
![Swagger](https://img.shields.io/badge/Swagger%20%2F%20OpenAPI-85EA2D?logo=swagger&logoColor=black&style=for-the-badge)
![Dapper](https://img.shields.io/badge/Dapper-Micro--ORM-007ACC?style=for-the-badge)
![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC292B?logo=microsoftsqlserver&logoColor=white&style=for-the-badge)

A standalone, high-performance **.NET 10 Controller-based Web API** for the **RecordDB** music catalog system.

RecordDB.API exposes RESTful endpoints for managing artists, records (albums), discs, and tracks. It is backed by a self-contained data access layer built with **Dapper** and **Microsoft.Data.SqlClient**, executing optimized SQL Server stored procedures and table-valued parameters (TVPs).

---

## Features

- **.NET 10 & C# 13**: Modern runtime features including primary constructors and collection expressions.
- **Self-Contained Data Layer**: Completely independent of external DAL class libraries.
- **Dapper & Stored Procedures**: Direct, lightweight database execution with zero EF Core overhead.
- **Table-Valued Parameters (TVP)**: Bulk track insertion via SQL Server's `dbo.TrackTableType`.
- **DTOs & Data Integrity**: Dedicated request/response DTOs prevent over-posting and eliminate circular references.
- **Triple API Documentation (Scalar, ReDoc, & Swagger UI)**: Explore and test your endpoints through three interactive documentation UIs:
  - **Scalar** (`/scalar/v1`, with `/` root redirection)
  - **ReDoc** (`/redoc`)
  - **Swagger UI** (`/swagger`)

---

## Project Structure

```
RecordDB.API/
├── RecordDB.API.slnx                  # Solution file (.NET 10 format)
└── RecordDB.API/
    ├── Program.cs                     # DI configuration, pipeline, & Swagger setup
    ├── appsettings.json               # Database connection strings & logging config
    ├── Controllers/                   # ASP.NET Core API controllers
    │   ├── ArtistController.cs        # Artist CRUD, bios, and search endpoints
    │   ├── RecordController.cs        # Record CRUD, reviews, totals, and counts
    │   ├── DiscController.cs          # Disc CRUD and length patching
    │   └── TrackController.cs         # Track CRUD, search, and TVP bulk inserts
    ├── Repositories/                  # Data access abstractions & implementations
    │   ├── IArtistRepository.cs / ArtistRepository.cs
    │   ├── IRecordRepository.cs / RecordRepository.cs
    │   ├── IDiscRepository.cs   / DiscRepository.cs
    │   └── ITrackRepository.cs  / TrackRepository.cs
    ├── Data/                          # Core database execution engine
    │   ├── IDataAccess.cs             # Generic interface for queries, scalars, & commands
    │   └── DataAccess.cs              # Dapper SQL connection and command execution
    ├── Models/                        # Domain entities representing database tables
    │   ├── Artist.cs
    │   ├── Record.cs
    │   ├── Disc.cs
    │   ├── Track.cs
    │   └── Total.cs
    ├── DTOs/                          # Request bodies & projection view models
    │   ├── ArtistDto.cs / CreateArtistDto.cs / UpdateArtistDto.cs
    │   ├── CreateRecordDto.cs / UpdateRecordDto.cs
    │   ├── CreateDiscDto.cs / UpdateDiscDto.cs / UpdateDiscLengthDto.cs
    │   ├── CreateTrackDto.cs / UpdateTrackDto.cs
    │   ├── ArtistRecordDto.cs
    │   ├── ArtistRecordDiscDto.cs
    │   ├── ArtistRecordDiscTrackDto.cs
    │   ├── RecordReviewDto.cs
    │   └── MissingReviewDto.cs
    └── Extensions/
        └── DateTimeExtensions.cs      # Date formatting helper utilities
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- SQL Server instance with the `RecordDB` database schema and stored procedures installed.

### Configuration

Edit `RecordDB.API/appsettings.json` with your database credentials:

```json
{
  "ConnectionStrings": {
    "RecordDb": "Server=YOUR_SERVER;Initial Catalog=RecordDB;Persist Security Info=False;User ID=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
  }
}
```

> **Note on `TrustServerCertificate=True`**: When connecting to local development SQL Server instances with self-signed certificates, `TrustServerCertificate=True` is required by `Microsoft.Data.SqlClient` to bypass certificate chain validation.

### Running the API

From the terminal:

```powershell
# Restore dependencies and build
dotnet build D:\Projects\RecordDB.API\RecordDB.API.slnx

# Run the API project
dotnet run --project D:\Projects\RecordDB.API\RecordDB.API
```

Once running, launch your browser and navigate to:
- **Scalar API Reference**: `https://localhost:<port>/scalar/v1` (or root `https://localhost:<port>/`)
- **ReDoc**: `https://localhost:<port>/redoc`
- **Swagger UI**: `https://localhost:<port>/swagger`
- **OpenAPI Specification**: `https://localhost:<port>/openapi/v1.json`

---

## API Endpoints Reference

### 1. Artist (`/api/artist`)

| HTTP Method | Route | Description |
|---|---|---|
| `GET` | `/api/artist` | Retrieve all artists |
| `GET` | `/api/artist/{id}` | Retrieve a single artist by ID |
| `GET` | `/api/artist/search/{name}` | Search artists by partial name |
| `GET` | `/api/artist/no-biography` | Retrieve all artists with missing biography |
| `GET` | `/api/artist/no-biography/{name}` | Retrieve an artist with no biography by name |
| `GET` | `/api/artist/by-record/{recordId}` | Retrieve the artist for a specific record |
| `GET` | `/api/artist/biography/{recordId}` | Retrieve the biography text for a record's artist |
| `GET` | `/api/artist/artist-id?firstName=&lastName=` | Look up an `ArtistId` by first and last name |
| `POST` | `/api/artist` | Create a new artist (`CreateArtistDto`) |
| `PUT` | `/api/artist/{id}` | Update an existing artist (`UpdateArtistDto`) |
| `DELETE` | `/api/artist/{id}` | Delete an artist by ID |

---

### 2. Record (`/api/record`)

| HTTP Method | Route | Description |
|---|---|---|
| `GET` | `/api/record` | Retrieve all records with artist details |
| `GET` | `/api/record/{id}` | Retrieve a single record with artist details by ID |
| `GET` | `/api/record/show/{show}` | Filter records by show flag (e.g. `Y` / `N`) |
| `GET` | `/api/record/by-artist/{name}` | Retrieve records by artist name |
| `GET` | `/api/record/by-artist-id/{artistId}` | Retrieve records for an artist ID |
| `GET` | `/api/record/by-year/{year}` | Retrieve records by recording year |
| `GET` | `/api/record/reviews` | Retrieve all records having reviews |
| `GET` | `/api/record/no-reviews` | Retrieve all records missing reviews |
| `GET` | `/api/record/no-tracks` | Retrieve all records that have no tracks |
| `GET` | `/api/record/no-tracks/{name}` | Retrieve records with no tracks for a specific artist |
| `GET` | `/api/record/tracks/search/{name}` | Search tracks with record/disc context |
| `GET` | `/api/record/totals` | Retrieve per-artist disc count and cost totals |
| `GET` | `/api/record/disc-count/{show}` | Count total discs by show flag |
| `GET` | `/api/record/count/artist/{artistId}` | Count total records for an artist |
| `GET` | `/api/record/count/year/{year}` | Count total records for a recording year |
| `POST` | `/api/record` | Create a new record (`CreateRecordDto`) |
| `PUT` | `/api/record/{id}` | Update an existing record (`UpdateRecordDto`) |
| `DELETE` | `/api/record/{id}` | Delete a record by ID |

---

### 3. Disc (`/api/disc`)

| HTTP Method | Route | Description |
|---|---|---|
| `GET` | `/api/disc` | Retrieve all discs with artist and record context |
| `GET` | `/api/disc/{id}` | Retrieve a single disc by ID |
| `GET` | `/api/disc/search/{name}` | Search discs by record name |
| `POST` | `/api/disc` | Create a new disc (`CreateDiscDto`) |
| `PUT` | `/api/disc/{id}` | Update an existing disc (`UpdateDiscDto`) |
| `PATCH` | `/api/disc/{id}/length` | Update only the disc's length (`UpdateDiscLengthDto`) |
| `DELETE` | `/api/disc/{id}` | Delete a disc by ID |

---

### 4. Track (`/api/track`)

| HTTP Method | Route | Description |
|---|---|---|
| `GET` | `/api/track` | Retrieve all tracks with full context |
| `GET` | `/api/track/{id}` | Retrieve a single track by ID |
| `GET` | `/api/track/search/{name}` | Search tracks by partial name |
| `GET` | `/api/track/by-artist/{name}` | Retrieve tracks for records by artist name |
| `GET` | `/api/track/by-record/{name}` | Retrieve tracks by record name |
| `GET` | `/api/track/count/record/{recordId}` | Get total track count for a record ID |
| `GET` | `/api/track/check/disc/{discId}` | Check existing track count for a disc |
| `POST` | `/api/track` | Create a single track (`CreateTrackDto`) |
| `POST` | `/api/track/bulk` | Bulk-insert multiple tracks using SQL TVP (`IEnumerable<CreateTrackDto>`) |
| `PUT` | `/api/track/{id}` | Update an existing track (`UpdateTrackDto`) |
| `DELETE` | `/api/track/{id}` | Delete a track by ID |

---

## Technical Highlights

### Table-Valued Parameter (TVP) Bulk Insertion

For high-throughput track creation, `TrackRepository` uses SQL Server's Table-Valued Parameter `dbo.TrackTableType` mapped via a populated ADO.NET `DataTable`:

```csharp
var trackTable = new DataTable();
trackTable.Columns.Add("DiscId", typeof(int));
trackTable.Columns.Add("TrackNo", typeof(int));
trackTable.Columns.Add("Name", typeof(string));
trackTable.Columns.Add("TrackLength", typeof(int));
trackTable.Columns.Add("Extended", typeof(string));

foreach (var track in tracks)
{
    trackTable.Rows.Add(
        track.DiscId,
        track.TrackNo,
        track.Name ?? string.Empty,
        (object?)track.TrackLength ?? DBNull.Value,
        (object?)track.Extended ?? DBNull.Value
    );
}

var parameters = new
{
    Tracks = trackTable.AsTableValuedParameter("dbo.TrackTableType")
};

await _db.SaveData("up_InsertTracks", parameters);
```

### Clean DTO Separation

All controller endpoints receive and return dedicated DTOs rather than raw database entities with circular object graphs. This ensures clean JSON serialization, consistent contract versioning, and defense against mass assignment vulnerabilities.
