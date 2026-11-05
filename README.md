# Bioreactor Sensor Readings API

I built this project to learn C# and ASP.NET Core. 

It's a small REST API that records sensor readings from bioreactors (temperature, pH
and dissolved oxygen). I chose this example because it links to my earlier work at DRDO,
where I built a Python system to collect and validate hardware sensor data. The same
idea applies here where if the data coming in is wrong, everything built on top of it is
wrong too, so validation comes first.

## What it does

- Adds a reading for a bioreactor and rejects it if the value is out of range or not a
  valid number
- Lists all readings for a bioreactor, oldest first
- Gives a summary for one sensor type: count, minimum, maximum and average

## How it's built

- **ASP.NET Core Web API** with controllers
- **Layered design:** the controller handles HTTP, the service holds the validation
  rules, and the repository handles storage. Each layer depends on an interface, so I
  can swap the storage for a database without changing the service
- **Dependency injection** set up in `Program.cs`
- **xUnit tests** for the service, including a parameterised test that checks several
  out-of-range values

## Endpoints

| Method | Route | What it does |
|---|---|---|
| POST | `/api/bioreactors/{id}/readings` | Add a reading |
| GET | `/api/bioreactors/{id}/readings` | List readings |
| GET | `/api/bioreactors/{id}/readings/summary?type=Ph` | Get the summary for one sensor type |

Example request body:

```json
{ "type": "Ph", "value": 7.1 }
```

An invalid reading returns a `400` with a clear message, for example:

```json
{ "error": "Ph value 9.5 is outside the allowed range 6 to 8." }
```

## Running it

You need the .NET 10 SDK.

```
dotnet run --project src/SensorApi --launch-profile http
```

To run the tests:

```
dotnet test
```

## What I'd add next

- **A real database** using Entity Framework Core. Right now readings are stored in
  memory, so they're lost when the app restarts. I kept it simple for the first version
- **Integration tests** for the endpoints, not just the service
- **Configurable limits.** The allowed ranges are fixed in the code, but in a real
  process they would depend on the process and should come from configuration
- **An audit trail** recording who added each reading and when, which matters in
  regulated environments

## What I learned

Most of the concepts carried over directly from Spring Boot like layers, dependency
injection, REST design and testing. The main differences were in syntax, such as
properties instead of getters and setters, LINQ instead of Java Streams, and attributes
instead of annotations.