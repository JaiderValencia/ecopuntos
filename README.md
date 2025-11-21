# 🌱 Gestor Ecopuntos - ECO Medellín S.A.S.

Sistema integral de gestión de puntos ecológicos para el reciclaje de materiales no orgánicos en la ciudad de Medellín.

## 📋 Descripción del Proyecto

En la empresa ECO Medellín S.A.S, se cuenta con una serie de puntos ecológicos distribuidos por la ciudad de Medellín de forma estratégica en zonas públicas fácilmente reconocibles, estos están destinados a que las personas puedan entregar residuos no orgánicos reciclables a los responsables del ecopunto.

La empresa ECO Medellín S.A.S, indicó sobre la necesidad de poder registrar fácilmente nuevas ubicaciones de ecopuntos, así como también definir el tipo de material reciclable que será aceptado en cada uno de ellos, también debe permitir la gestión del personal encargado de cada ecopunto, asegurando el correcto control de asignaciones y responsabilidades.

Los empleados deben contar con acceso a una herramienta que le permita a los responsables del ecopunto clasificar rápidamente los residuos reciclables según el tipo de material llevado por el cliente a la ubicación del ecopunto, esta herramienta debe ser accesible e intuitiva, garantizando un manejo sencillo para los empleados.

Los clientes de la empresa necesitan una herramienta segura, accesible y disponible en todo momento que permita conocer, ya sea por medio de direcciones o coordenadas en un mapa, la ubicación de los ecopuntos distribuidos por la ciudad, con el fin de facilitar su uso.

### 👥 Actores del Sistema

- **Administración**: Encargados de gestionar el sistema, supervisar los ecopuntos, registrar nuevas ubicaciones, materiales aceptados y asignar responsables a cada punto.
- **Clientes**: Usuarios que entregan materiales reciclables en los ecopuntos y utilizan el sistema para consultar ubicaciones, ver sus reportes y acumular puntos por sus entregas.
- **Trabajadores (Empleados)**: Responsables de recibir los materiales reciclables, registrar las entregas, clasificar los residuos y generar los reportes correspondientes.

## 🚀 Tecnologías Utilizadas

### Backend
- **ASP.NET Core** 9.0 - Framework web para la API REST
- **C#** - Lenguaje de programación
- **SQL Server** - Sistema de gestión de base de datos relacional
- **Microsoft.Data.SqlClient** - Proveedor de datos para SQL Server
- **BCrypt.Net-Next** 4.0.3 - Hashing de contraseñas
- **JWT (JSON Web Tokens)** - Autenticación y autorización
- **Swashbuckle.AspNetCore** 6.6.2 - Documentación de API con Swagger

### Frontend
- **React** 18.3.1 - Biblioteca de JavaScript para interfaces de usuario
- **TypeScript** 5.6.2 - Superset tipado de JavaScript
- **Vite** 5.4.10 - Herramienta de construcción y desarrollo
- **React Router DOM** 6.28.0 - Enrutamiento para aplicaciones React
- **React Hook Form** 7.53.2 - Manejo de formularios
- **Axios** 1.7.7 - Cliente HTTP para peticiones a la API
- **Leaflet** 1.9.4 - Biblioteca de mapas interactivos
- **Tailwind CSS** 3.4.14 - Framework CSS utility-first

## 🏗️ Arquitectura y Patrones de Diseño

### Patrones Implementados

#### Backend

1. **Repository Pattern**
   - Abstracción de la capa de acceso a datos
   - Interfaces: `IRepositorioEscrituraTabla`, `IRepositorioLecturaTabla`, `IRepositorioActualizarTabla`, `IRepositorioEliminarTabla`, `IRepositorioBusquedaPorCampoTabla`, `IRepositorioSubconsulta`, `IRepositorioJoin`, `IRepositorioConsultaPersonalizada`
   - Implementaciones específicas para SQL Server

2. **Service Layer Pattern**
   - Separación de la lógica de negocio de los controladores
   - Servicios: `ServicioCliente`, `ServicioTrabajador`, `ServicioAdministrador`, `ServicioEcoPunto`, `ServicioMaterial`, `ServicioEntrega`, `ServicioReporte`, `ServicioJwt`

3. **Dependency Injection**
   - Inyección de dependencias nativa de ASP.NET Core
   - Configuración en `Program.cs`
   - Gestión del ciclo de vida de servicios (Scoped, Singleton)

4. **Proxy Pattern**
   - `ServicioJwtProxy` para caching de tokens JWT
   - Implementa caché en memoria para mejorar rendimiento

5. **DTO (Data Transfer Object) Pattern**
   - Separación entre modelos de dominio y objetos de transferencia
   - DTOs: `ClienteDto`, `TrabajadorDto`, `UsuarioDto`, etc.

6. **Provider Pattern**
   - `ProveedorConexion` para gestión centralizada de cadenas de conexión

#### Frontend

1. **Context API Pattern**
   - Gestión global del estado del usuario (`UserContext`)
   - Gestión del estado del mapa (`MapContext`)

2. **Custom Hooks**
   - `useUserContext` - Acceso al contexto de usuario
   - `useLogout` - Lógica reutilizable de cierre de sesión
   - `hasSession` - Validación de sesión activa

3. **Component Composition**
   - Componentes reutilizables: `Button`, `InputComponent`, `Table`, `Card`
   - Separación de responsabilidades por componente

4. **Protected Routes Pattern**
   - `PrivateRoute` - Rutas que requieren autenticación
   - `PublicRoute` - Rutas solo para usuarios no autenticados

5. **API Client Pattern**
   - Instancia centralizada de Axios con interceptores
   - Módulos de API separados por dominio (clientes, trabajadores, ecopuntos, etc.)

## 📐 Principios SOLID Aplicados

### Single Responsibility Principle (SRP)
- **Backend**: Cada servicio tiene una única responsabilidad (ej: `ServicioCliente` solo gestiona operaciones de clientes)
- **Frontend**: Cada componente tiene una función específica (ej: `Button` solo renderiza botones, `Table` solo muestra tablas)

### Open/Closed Principle (OCP)
- **Backend**: Repositorios abiertos a extensión mediante interfaces, cerrados a modificación
- **Frontend**: Componentes configurables mediante props sin necesidad de modificar su código interno

### Liskov Substitution Principle (LSP)
- **Backend**: Las implementaciones de repositorios (`RepositorioEscrituraSqlServer`, `RepositorioLecturaSqlServer`) son intercambiables donde se espera la interfaz base
- **Frontend**: Componentes pueden ser reemplazados por variantes sin afectar el funcionamiento

### Interface Segregation Principle (ISP)
- **Backend**: Múltiples interfaces específicas (`IRepositorioEscrituraTabla`, `IRepositorioLecturaTabla`) en lugar de una interfaz grande
- Los servicios solo dependen de las interfaces que realmente necesitan

### Dependency Inversion Principle (DIP)
- **Backend**: Los servicios dependen de abstracciones (interfaces) no de implementaciones concretas
- **Frontend**: Componentes dependen de props y contextos, no de implementaciones específicas

## 🗂️ Estructura del Proyecto

```
ecopuntos/
├── backend/
│   └── webapicsharp/
│       ├── Controllers/        # Controladores de la API REST
│       ├── Servicios/          # Lógica de negocio
│       ├── Repositorios/       # Acceso a datos
│       ├── Modelos/            # Modelos de dominio y DTOs
│       └── Interface/          # Contratos e interfaces
│
└── frontend/
    └── src/
        ├── api/                # Clientes HTTP para la API
        ├── components/         # Componentes reutilizables
        ├── contex/            # Gestión de estado global
        ├── interfaces/         # Definiciones de tipos TypeScript
        ├── pages/             # Páginas de la aplicación
        └── utils/             # Funciones auxiliares
```

## ✨ Funcionalidades Principales

### 1. Gestión de Ecopuntos
- ✅ Registro de nuevos ecopuntos con ubicación (dirección y coordenadas)
- ✅ Actualización de información de ecopuntos
- ✅ Visualización de ecopuntos en mapa interactivo
- ✅ Asignación de materiales aceptados por ecopunto
- ✅ Asignación de responsables (trabajadores)

### 2. Gestión de Usuarios
- ✅ **Perfiles diferenciados**:
  - **Administrador**: Gestión completa del sistema
  - **Trabajador**: Gestión de entregas y reportes
  - **Cliente**: Consulta de ecopuntos y visualización de reportes
- ✅ Autenticación con JWT
- ✅ Registro de nuevos clientes
- ✅ Visualización de perfil con estadísticas

### 3. Registro de Entregas
- ✅ Registro de entregas por material
- ✅ Clasificación de materiales (aceptados/rechazados)
- ✅ Cálculo automático de puntos de recompensa
- ✅ Asignación de entregas a clientes registrados

### 4. Sistema de Reportes
- ✅ Generación automática de reportes por entrega
- ✅ Información detallada de cliente y trabajador
- ✅ Listado de materiales con cantidades y estados
- ✅ Top 3 de materiales más entregados
- ✅ Resumen de totales (entregado, aceptado, rechazado)
- ✅ Historial de reportes para clientes

### 5. Mapa de Ecopuntos
- ✅ Visualización interactiva con Leaflet
- ✅ Marcadores para cada ecopunto
- ✅ Lista de ecopuntos con información completa
- ✅ Indicadores de estado (abierto/cerrado)

## 🔐 Seguridad

- ✅ Autenticación basada en JWT
- ✅ Hashing de contraseñas con BCrypt
- ✅ Rutas protegidas por rol de usuario
- ✅ Validación de datos en backend y frontend
- ✅ Manejo seguro de sesiones en cliente

## 📦 Instalación y Ejecución

### Backend
```bash
cd backend/webapicsharp
dotnet restore
dotnet run
```

### Frontend
```bash
cd frontend
pnpm install
pnpm dev
```

## 👨‍💻 Desarrolladores

Proyecto desarrollado para ECO Medellín S.A.S.