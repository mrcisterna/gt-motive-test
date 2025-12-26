# Docker Setup - Vehicle Rental API

## Producción (Release)

```bash
# Desde la raíz del repositorio (C:\_workspace\gt-motive-test)
docker build -f src/GtMotive.Estimate.Microservice.Host/Dockerfile -t vehicle-rental:latest .

# Ejecutar contenedor
docker run -d -p 5000:80 --name vehicle-rental-api vehicle-rental:latest

# Con docker-compose
docker-compose up -d
```

## Desarrollo (Con hot reload)

```bash
docker-compose -f docker-compose.dev.yml up

# O manualmente
docker build -f src/Dockerfile.dev -t vehicle-rental-dev:latest .
docker run -it -p 5000:5000 -v C:\_workspace\gt-motive-test:/src vehicle-rental-dev:latest
```

## Parar contenedores

```bash
# Parar todos
docker-compose down

# Específico
docker stop vehicle-rental-api
docker rm vehicle-rental-api
```

## URLs

- **Producción**: http://localhost:5000
- **Desarrollo**: http://localhost:5000
