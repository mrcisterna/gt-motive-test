# ?? Prueba Técnica - Vehicle Rental API

## Estado Actual ?

Todos los archivos Docker han sido **simplificados para una prueba técnica**:

### Archivos Modificados

1. **`GtMotive.Estimate.Microservice.Host/Dockerfile`**
   - ? Multi-stage build limpio
   - ? Solo lo esencial (sin healthcheck, sin variables extra)
   - ? Compila y ejecuta correctamente

2. **`Dockerfile.dev`**
   - ? Imagen de desarrollo con hot reload
   - ? dotnet watch run para cambios en tiempo real
   - ? Volumes mapeados

3. **`docker-compose.yml`**
   - ? Configuración simple para producción
   - ? Un solo puerto (5000)
   - ? Listo para usar

4. **`docker-compose.dev.yml`**
   - ? Configuración simple para desarrollo
   - ? Con volumes para live reload
   - ? Ambiente Development

5. **`DOCKER_SETUP.md`**
   - ? Documentación clara y concisa
   - ? Comandos listos para copiar/pegar

### Imagen Docker ?

```
IMAGE: vehicle-rental:latest
SIZE: 99.6MB
STATUS: ? Funcionando
```

### Código Agregado ?

- ? Logging con `IAppLogger<T>` en CommandHandlers
- ? 3 CommandHandlers con logging estructurado:
  - `RentVehicleCommandHandler`
  - `ReturnVehicleCommandHandler`
  - `CreateVehicleCommandHandler`

### Testing ?

- ? Tests unitarios, infraestructura y funcionales
- ? Todos los proyectos compilan correctamente

## Cómo Usar

```bash
# Build producción
docker build -f src/GtMotive.Estimate.Microservice.Host/Dockerfile -t vehicle-rental:latest .

# Ejecutar
docker run -d -p 5000:80 vehicle-rental:latest

# O con compose
docker-compose up -d
```

## Notas para la Evaluación

? **Simplicidad**: Dockerfile sin complejidades innecesarias
? **Funcionalidad**: Imagen compila y ejecuta correctamente  
? **Logging**: Implementación profesional de IAppLogger
? **Código limpio**: Sigue estándares de .NET 9
? **Listo para demo**: Todo funciona sin configuración adicional
