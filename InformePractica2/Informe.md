
# 3. Code coverage tool
## 3.1 Conclusiones y elección final
## 3.2. Guía de instalación y uso Coverlet
### Prerrequisitos
- Tener instalado `dotnet`
- Tener creado un proyecto de testing
#### Instalación
1. Añadir `coverlet.msbuild` a cada proyecto de testing (`.csproj`) mediante el NuGet package manager o con el comando (correr el comando a nivel de `.csproj`).
	```cmd
	dotnet add package coverlet.msbuild
	```
	Esto modificara el archivo `.csproj`  instalando el paquete.
	![[Pasted image 20221011224354.png]]
	```xml
    <PackageReference Include="coverlet.msbuild" Version="3.1.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
	```
2. En un powershell con privilegios de administrador ejecutar
	```cmd
	dotnet tool install -g dotnet-reportgenerator-globaltool
	dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
	dotnet new tool-manifest
	dotnet tool install dotnet-reportgenerator-globaltool
	```
    Respuesta esperada
    ```cmd
    C:\WINDOWS\system32> dotnet tool install -g dotnet-reportgenerator-globaltool
    Puede invocar la herramienta con el comando siguiente: reportgenerator
    La herramienta "dotnet-reportgenerator-globaltool" (versión '5.1.10') se instaló correctamente.
    C:\WINDOWS\system32> dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
    Puede invocar la herramienta con el comando siguiente: reportgenerator
    La herramienta "dotnet-reportgenerator-globaltool" (versión '5.1.10') se instaló correctamente.
    C:\WINDOWS\system32> dotnet new tool-manifest
    La plantilla "Archivo de manifiesto de la herramienta local de dotnet" se creó correctamente.

    C:\WINDOWS\system32> dotnet tool install dotnet-reportgenerator-globaltool
    Puede invocar la herramienta desde este directorio con los comandos siguientes: "dotnet tool run reportgenerator" o "dotnet reportgenerator".
    La herramienta "dotnet-reportgenerator-globaltool" (versión "5.1.10") se instaló correctamente. Se ha agregado la entrada al archivo de manifiesto C:\WINDOWS\system32\.config\dotnet-tools.json.
    C:\WINDOWS\system32>
    ```
3. Instalar la extensión `Run Coverlet Report` en Visual Studio, reiniciar el IDE para que los cambios surjan efecto.
    ![[Pasted image 20221011224717.png]]
    Ingresar a `Herramientas > Opciones` y modificar el valor de `Integration type`
    ![[Pasted image 20221011225133.png]]
    Instalación completa.
#### Uso y generación de reportes
Para obtener el reporte se deberá correr todos los test por lo menos una vez, para inicializar de manera correcta las referencias a los proyectos de test. (solo necesario la primera vez).
Para correr el análisis de cobertura ir a `Herramientas > Run Code Coverage`. Esto generara los files del reporte y los abrirá en Visual Studio.
![[Pasted image 20221011230520.png]]
Para ver las líneas resaltadas directamente en el IDE seleccionar `Herramientas > Toggle Code Coverage Highlighting` para activar o desactivar la opción.
![[Pasted image 20221011230438.png]]
guia extension 
