# FocusMate

Trabajo de Diploma desarrollado para la asignatura Ingeniería de Software.

## Descripción

FocusMate es una aplicación de escritorio orientada a la gestión, priorización y planificación de tareas personales y laborales. El sistema permite modelar tareas con criterios multidimensionales, evaluar su prioridad mediante algoritmos determinísticos, gestionar la disponibilidad horaria semanal del usuario y estructurar agendas de trabajo integradas con flujos formales de revisión y supervisión.

## Características principales

- **Gestión de tareas**: Registro, edición, baja lógica y control de estados (Pendiente, En Curso, Pausada, Completada, Cancelada).
- **Cálculo de prioridad**: Ponderación automática en función de importancia, urgencia por fecha límite, nivel de energía requerido y duración estimada.
- **Visualizador TOP 10**: Vista focalizada de las diez tareas de mayor prioridad para agilizar la ejecución y actualización de estados.
- **Jornada laboral y bloques de tiempo**: Configuración de franjas horarias de trabajo por día de la semana y reserva de bloques fijos con validación de solapamientos.
- **Generación de planificaciones**: Algoritmo de asignación de tareas priorizadas dentro de los intervalos disponibles en la agenda.
- **Circuito de supervisión**: Estados formales de planificación (Generada, Revisada, Observada, Aprobada) con registro de observaciones.
- **Historial y auditoría de tareas**: Trazabilidad completa de cambios con capacidad de reversión a estados previos.
- **Control de acceso y usuarios**: Autenticación con políticas de bloqueo, gestión de cuentas y asignación jerárquica de permisos.
- **Bitácora del sistema**: Registro transaccional centralizado de eventos y operaciones.
- **Integridad de datos (DVH / DVV)**: Mecanismo de Dígito Verificador Horizontal y Vertical para detección de manipulaciones externas y auto-reparación.
- **Copias de seguridad**: Módulo administrativo de Backup y Restore nativo de base de datos.
- **Internacionalización (i18n)**: Soporte multiidioma con cambio dinámico de textos en tiempo de ejecución y administración de etiquetas.
- **Exportación multiformato**: Generación de reportes de agenda en formato PDF y exportación/importación en formato JSON.

## Arquitectura

El proyecto implementa una arquitectura por capas desacoplada:

- **Abstracciones**: Contratos e interfaces base del sistema.
- **BE (Business Entities)**: Entidades del dominio y estructuras del modelo jerárquico.
- **BL (Business Logic)**: Reglas de negocio, algoritmos de cálculo, validaciones y orquestación de flujos.
- **DAL (Data Access Layer)**: Capa de persistencia que interactúa con SQL Server mediante Stored Procedures.
- **Servicioss**: Servicios transversales independientes del dominio (criptografía, sesión e internacionalización).
- **UI (User Interface)**: Capa de presentación desarrollada en Windows Forms (arquitectura MDI).

### Esquema de dependencias

```
UI ----------> BL ----------> DAL ----------> Base de Datos (SQL Server)
|              |               |
+---> Servicioss <-------------+
|              |
+---> BE <-----+
      |
      v
Abstracciones
```

## Patrones de diseño

- **Composite**: Utilizado en `BE.Componente`, `BE.Patente` y `BE.Familia` para gestionar la jerarquía y evaluación recursiva de permisos y roles (RBAC).
- **Observer**: Utilizado en `Servicioss.IdiomaService` e `IIdiomaObserver` para notificar y actualizar en tiempo real los textos de los formularios abiertos ante un cambio de idioma.
- **Singleton**: Utilizado mediante `Lazy<T>` en `Servicioss.SesionActual` y `Servicioss.IdiomaService` para asegurar una única instancia compartida durante el ciclo de vida de la aplicación.

## Stack tecnológico

- **Lenguaje**: C#
- **Framework**: .NET Framework 4.7.2
- **Interfaz de usuario**: Windows Forms
- **Base de datos**: Microsoft SQL Server (persistencia mediante Stored Procedures)
- **Gestión de dependencias**: NuGet
- **Librerías externas**:
  - `iTextSharp` (5.5.13.5) — Generación y exportación de reportes PDF.
  - `Newtonsoft.Json` (13.0.4) — Serialización y persistencia de agendas en JSON.
  - `BouncyCastle.Cryptography` (2.6.2) — Algoritmos criptográficos auxiliares.

## Modelo de priorización

El cálculo del score de prioridad de cada tarea se evalúa en `BL.TareaBL` mediante la siguiente fórmula:

$$\text{Score} = (\text{BaseImportancia} + \text{Urgencia}) \times \text{FactorEnergia} \times \text{FactorDuracion}$$

Donde:
- **BaseImportancia**: Ponderación numérica fija según el nivel asignado ($\text{Importancia} \times 2.0$).
- **Urgencia**: Valor incremental determinado por los días restantes hasta la fecha límite ($0 \text{ a } 4 \text{ puntos}$).
- **FactorEnergia**: Coeficiente según el esfuerzo requerido (Baja: $1.0$, Media: $0.9$, Alta: $0.8$).
- **FactorDuracion**: Coeficiente según la duración estimada ($\le 30\text{ min}: 1.0$, $\le 60\text{ min}: 0.9$, $\le 120\text{ min}: 0.7$, $> 120\text{ min}: 0.5$).
- Las tareas en estado *Completada* o *Cancelada* tienen score $0$.

## Seguridad e integridad

- **Criptografía**: Almacenamiento de contraseñas con hash PBKDF2-SHA256 (100.000 iteraciones) y salt criptográfico aleatorio de 32 bytes.
- **Políticas de acceso**: Bloqueo progresivo de cuentas ante reiterados intentos fallidos de autenticación y validación de complejidad de clave.
- **Permisos jerárquicos**: Control de acceso basado en roles con árbol de patentes y familias.
- **Integridad (DVH / DVV)**: Dígito Verificador Horizontal por registro en la entidad `Tarea` y Dígito Verificador Vertical por tabla, impidiendo la manipulación no autorizada en la base de datos.
- **Auditoría**: Bitácora centralizada para seguimiento de operaciones críticas del sistema.

## Base de datos

El script completo para crear la base de datos, tipos definidos por el usuario, Stored Procedures y datos semilla se encuentra en:

- `Database/FocusMate_Database.sql`

El sistema opera exclusivamente mediante Stored Procedures parametrizados en Microsoft SQL Server.

## Instalación y ejecución

### Requisitos previos
- Windows con .NET Framework 4.7.2 Runtime / SDK.
- Microsoft SQL Server 2016 o superior.
- Visual Studio 2022 (o herramienta compatible con MSBuild).

### Pasos de configuración

1. **Clonar el repositorio**:
   ```bash
   git clone https://github.com/carlosmassaa/FocusMate.git
   cd FocusMate
   ```

2. **Instalar la base de datos**:
   - Abrir SQL Server Management Studio (SSMS).
   - Conectarse a la instancia local de SQL Server.
   - Abrir y ejecutar el script `Database/FocusMate_Database.sql`.
   - Esto creará la base de datos `FocusMateTDFinalFinal` con su esquema, procedimientos y datos iniciales.

3. **Restaurar paquetes NuGet**:
   - Desde Visual Studio: Al compilar la solución, o mediante la consola del Administrador de Paquetes.
   - O desde línea de comandos:
     ```bash
     msbuild FocusMateV4.sln /t:Restore /p:RestorePackagesConfig=true
     ```

4. **Compilar y ejecutar**:
   - Abrir `FocusMateV4.sln` en Visual Studio.
   - Compilar la solución en configuración `Debug` o `Release`.
   - Ejecutar el proyecto `UI`.

> **Nota sobre la conexión**: La capa de acceso a datos (`DAL/AccesoBD.cs`) está configurada por defecto para conectarse a una instancia local de SQL Server (`Data Source=.`) utilizando autenticación integrada de Windows (`Integrated Security=True`).

## Documentación

La documentación técnica y funcional completa del Trabajo de Diploma está disponible como documento único:

- [Documento de Trabajo de Diploma (PDF)](docs/FocusMate_Documentacion_Diploma.pdf)
- [Documento de Trabajo de Diploma (Google Docs)](https://docs.google.com/document/d/18GcswtrGCYmu6kLq2ZgmoFpKvgrvWNF1RiiG6CG-ads/edit?usp=sharing)

## Autor

**Carlos Demián Massa Beloso**

## Estado del proyecto

Versión final del Trabajo de Diploma.
