## DisciplineService.GetDisciplinesAsync

### Código

```csharp
public async Task<IEnumerable<DisciplineModel>> GetDisciplinesAsync()
{
    var disciplineEntityList = await _athleteRepository.GetDisciplinesAsync();
    var disciplines = _mapper.Map<IList<DisciplineModel>>(disciplineEntityList);
    return disciplines;
}
```

### Grafo

```mermaid
graph TD
    I(I) --> 1(1)
    1 -->  F(F)
```

### Complejidad ciclo matica

Numero de regiones
$$
v(G) = R \\
v(G) = 1
$$

Numero de nodos y aristas
$$
v(G) = E - N + 2 \\
v(G) = 2 - 3 + 2
$$
  
Numero de decisiones
$$
v(G) = P + 1 \\
v(G) = 0 + 1
$$

### Casos de prueba

| | Camino   | Entrada   | TC | Salida  |
| --- | --- | --- | --- | --- |
| 1 | I-1-F | `athleteRepositoryMock.GetDisciplinesAsync()` return `Ilist<DisciplineEntity>(){new DisciplineEntity(){ Id = 1,Name = "100M"}, new DisciplineEntity(){Id = 2,Name = "200M"}}` | --- | `IList<DisciplineModel>(){new DisciplineModel(){ Id = 1,Name = "100M"}, new DisciplineModel(){Id = 2,Name = "200M"}}` |

TC1: Verificar que al recuperar las disciplinas de la bd `Ilist<DisciplineEntity>(){new DisciplineEntity(){ Id = 1,Name = "100M"}, new DisciplineEntity(){Id = 2,Name = "200M"}}` se recuperen correctamente (`IList<DisciplineModel>(){new DisciplineModel(){ Id = 1,Name = "100M"}, new DisciplineModel(){Id = 2,Name = "200M"}}`)

Camino 1
```mermaid
graph TD
    I(I):::c1 --> 1(1)
    1:::c1 -->  F(F):::c1
classDef c1 fill:#F2274C, stroke:#F2274C;
```
### Pruebas unitarias

```csharp
[Fact]
public async Task GetDisciplinesAsync_ReturnsListOfDisciplines()
{
	var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
	var mapper = config.CreateMapper();
	var disciplineEntity100M = new DisciplineEntity()
	{
		Id = 1,
		Name = "100M"              
	};
	var disciplineEntity200M = new DisciplineEntity()
	{
		Id = 2,
		Name = "200M"               
	};
	var disciplinesEnumerable = new List<DisciplineEntity>() { disciplineEntity100M, disciplineEntity200M } as IEnumerable<DisciplineEntity>;
	var repositoryMock = new Mock<IAthleteRepository>();
	repositoryMock.Setup(r => r.GetDisciplinesAsync()).ReturnsAsync(disciplinesEnumerable);

	var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
	var disciplinesList = await disciplinesService.GetDisciplinesAsync();
	Assert.NotNull(disciplinesList);
	Assert.NotEmpty(disciplinesList);
	Assert.Equal(2,disciplinesList.Count());
	Assert.Equal("100M", disciplinesList.First().Name);
	Assert.Equal("200M", disciplinesList.Last().Name);
}
```