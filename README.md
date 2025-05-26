# CV Parser API

API REST desarrollada en .NET 8 para procesar y extraer información de currículums vitae (CVs) en formato PDF o imagen.

## Características Principales

- Procesamiento de CVs en múltiples formatos (PDF, JPG, PNG)
- Extracción de texto mediante OCR (Optical Character Recognition)
- Análisis de contenido utilizando modelos de lenguaje (LLM)
- Estructura modular y extensible
- Alta cobertura de pruebas unitarias
- Logging estructurado con Serilog
- Documentación API con Swagger

## Arquitectura

El proyecto sigue una arquitectura limpia y modular:

- **Controllers**: Manejo de endpoints HTTP
- **Models**: Definición de estructuras de datos
- **Services**: Implementación de lógica de negocio
  - `IOcrService`: Servicio de extracción de texto
  - `ILlmService`: Servicio de procesamiento de lenguaje natural

## Observabilidad

Se implementó un sistema robusto de logging utilizando Serilog con:
- Logging a consola
- Logging a archivo
- Logging estructurado
- Contexto enriquecido

## Testing

El proyecto cuenta con una suite completa de pruebas unitarias utilizando:
- xUnit como framework de testing
- Moq para mocking
- FluentAssertions para aserciones expresivas
- Alta cobertura de código

## Requisitos

- .NET 8 SDK
- Visual Studio 2022 o VS Code con extensiones .NET

## Instalación

1. Clonar el repositorio
```bash
git clone [url-repositorio]
```

2. Restaurar dependencias
```bash
dotnet restore
```

3. Compilar el proyecto
```bash
dotnet build
```

## Ejecución

Para ejecutar la API:

```bash
dotnet run --project CVParserAPI
```

La API estará disponible en:
- http://localhost:5000
- https://localhost:5001

Swagger UI: https://localhost:5001/swagger

## Uso

La API expone un endpoint principal para procesar CVs:

```http
POST /api/resume
Content-Type: multipart/form-data

file: [archivo-cv]
```

## Pipeline de Procesamiento

1. **Validación de Archivo**
   - Verificación de formato
   - Validación de tamaño

2. **Extracción de Texto (OCR)**
   - Implementación mock para demostración
   - Preparado para integración con servicios reales

3. **Análisis de Contenido (LLM)**
   - Implementación mock para demostración
   - Preparado para integración con Gemini/GPT

## Contribución

1. Fork del repositorio
2. Crear rama feature (`git checkout -b feature/nueva-caracteristica`)
3. Commit cambios (`git commit -am 'Agrega nueva característica'`)
4. Push a la rama (`git push origin feature/nueva-caracteristica`)
5. Crear Pull Request

## Licencia

[MIT License](LICENSE) 