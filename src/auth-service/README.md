# Auth Service

[![Auth Service Build](https://github.com/benukhanov/maple-fighters/actions/workflows/auth-service-build.yml/badge.svg)](https://github.com/benukhanov/maple-fighters/actions/workflows/auth-service-build.yml)

Auth service stores user data and verifies user.

## Project Structure

```
├── Authenticator.API
│   ├── Constants
│   ├── Controllers
│   ├── Converters
│   ├── Datas
│   ├── Properties
│   └── Validators
├── Authenticator.Domain
│   ├── Aggregates
│   │   └── User
│   │       └── Services
│   ├── Repository
├── Authenticator.Infrastructure
│   ├── InMemoryRepository
│   ├── MongoRepository
└── Authenticator.UnitTests
    ├── API
    │   └── Controllers
    └── Domain
        └── User
            └── Services
```

## Quickstart

### Docker

Follow these instructions to create an image and run a container:

1. Build a docker image:

```bash
make build
```

2. Running in a docker container:

```bash
make run
```

You should now be able to access it at `http://localhost:50050`.
