## DisciplineService.CheckSeasonBest

### Código

```csharp
public bool CheckSeasonBest(AthleteModel athlete, Decimal mark, string discipline)
    {
        bool seasonBest = _markComparer[discipline](athlete.SeasonBest, mark);//1
        if (athlete.SeasonBest == null)//2
        {
            seasonBest = true;//3
        }
        if (seasonBest)//4
        {
            athlete.SeasonBest = mark;//5
        }
        return seasonBest;//6
    }
  
```

### Grafo

```mermaid
graph TD
    I(I) --> 1(1)
    1 --a--> 2(2)
    2 --b--> 3(3)
    3 --h--> 4{4}
    2 --c--> 4{4}    
    4 --d--> 5(5)
    4 --e--> 6(6)
    5 --f--> 6(6)
    6 --g--> F(F)
```

### Complejidad ciclo matica

Numero de regiones
$$
v(G) = R \\
v(G) = 3
$$

Numero de nodos y aristas
$$
v(G) = E - N + 2 \\
v(G) = 9 - 8 + 2
$$
  
Numero de decisiones
$$
v(G) = P + 1 \\
v(G) = 2 + 1
$$

### Casos de prueba

athlete, mark, discipline

| | Camino   | Entrada   | TC | Salida  |
| --- | --- | --- | --- | --- |
| 1 | I-1a-2b-3h-4d-5f-6g-F | `athlete`={Id=1,Nationality='USA', Name='Sydney Maclaughlin', Gender='f', Points=1000, PersonalBest=52.75, SeasonBest=null}`mark`=51.79 `discipline`='400MH'| athlete.SeasonBest=null -> seasonBest = true| true |
| 2 | I-1a-2c-4d-5f-6g-F | `athlete`={Id=1,Nationality='USA', Name='Sydney Maclaughlin', Gender='f', Points=1000, PersonalBest=52.75, SeasonBest=52}`mark`=51.79 `discipline`='400MH'| athlete.SeasonBest!=null -> seasonBet = true| true |
| 3 | I-1a-2c-4e-6g-F | `athlete`={Id=1,Nationality='USA', Name='Sydney Maclaughlin', Gender='f', Points=1000, PersonalBest=52.75, SeasonBest=52}`mark`=52.79 `discipline`='400MH'| athlete.SeasonBest!=null -> seasonBet = false| false |

TC1: Verificar que si la atleta {Id=1,Nationality='USA', Name='Sydney Maclaughlin', Gender='f', Points=1000, PersonalBest=52.75, SeasonBest=null}, inicialmente sin mejor marca de temporada, realiza una marca de 51.79 en la disciplina 400MH, devuelva true

TC2: Verificar que si la atleta {Id=1,Nationality='USA', Name='Sydney Maclaughlin', Gender='f', Points=1000, PersonalBest=52.75, SeasonBest=52}, con mejor marca de temporada 52, realiza una marca de 51.79 en la disciplina 400MH, mejor a su mejor marca de temporada previa, devuelva true

TC3: Verificar que si la atleta {Id=1,Nationality='USA', Name='Sydney Maclaughlin', Gender='f', Points=1000, PersonalBest=52.75, SeasonBest=52}, con mejor marca de temporada 52, realiza una marca de 52.79 en la disciplina 400MH, peor a su mejor marca de temporada previa, devuelva false

Camino 1
```mermaid
graph TD
    I(I):::c1 --> 1(1):::c1
    1 --a--> 2(2):::c1
    2 --b--> 3(3):::c1
    3 --h--> 4{4}:::c1
    2 --c--> 4{4}    
    4 --d--> 5(5):::c1
    4 --e--> 6(6)
    5 --f--> 6(6):::c1
    6 --g--> F(F):::c1
classDef c1 fill:#F2274C, stroke:#F2274C;
```
Camino 2
```mermaid
graph TD
    I(I):::c2 --> 1(1):::c2
    1 --a--> 2(2):::c2
    2 --b--> 3(3):::c2
    3 --h--> 4{4}:::c2
    2 --c--> 4{4}:::c2     
    4 --d--> 5(5)
    4 --e--> 6(6):::c2 
    5 --f--> 6(6)
    6 --g--> F(F):::c2
classDef c2 fill:#2964D9, stroke:#2964D9;
```
Camino 3
```mermaid
graph TD
    I(I):::c3 --> 1(1):::c3
    1 --a--> 2(2):::c3
    2 --b--> 3(3):::c3
    3 --h--> 4{4}:::c3
    2 --c--> 4{4}:::c3     
    4 --d--> 5(5):::c3
    4 --e--> 6(6) 
    5 --f--> 6(6):::c3
    6 --g--> F(F):::c3
classDef c3 fill:#B2A2FA, stroke:#B2A2FA;
```
### Pruebas unitarias

```csharp
//tc1
        [Fact]
        public void CheckSesasonBest_SeasonBestNull_ReturnsTrue()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52.75m,
                SeasonBest = null
            };
            var disciplineName = "400MH";
            var mark = 51.76m;
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckSeasonBest(sydney,mark,disciplineName);
            Assert.True(result);
        }
        //tc2
        [Fact]
        public void CheckSesasonBest_SeasonBestImproved_ReturnsTrue()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52m,
                SeasonBest = 51.79m
            };
            var disciplineName = "400MH";
            var mark = 51.76m;
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckSeasonBest(sydney, mark, disciplineName);
            Assert.True(result);
        }

        //tc3
        [Fact]
        public void CheckSesasonBest_SeasonBestNotImproved_ReturnsFalse()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52.75m,
                SeasonBest = 52m
            };
            var disciplineName = "400MH";
            var mark = 52.79m;
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckSeasonBest(sydney, mark, disciplineName);
            Assert.False(result);
        }
```