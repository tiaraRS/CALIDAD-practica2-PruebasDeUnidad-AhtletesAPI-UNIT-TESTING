## AthleteService.DeleteAthleteAsync

### Código
Código original
```csharp
public async Task DeleteAthleteAsync(int athleteId, int disciplineId)
{
	await GetAthleteAsync(athleteId, disciplineId);
	await _athleteRepository.DeleteAthleteAsync(athleteId,disciplineId);
	var result = await _athleteRepository.SaveChangesAsync();
	if (!result)
	{
		throw new Exception("Database Error");
	}
}
```
Correccion
```csharp
public async Task<bool> DeleteAthleteAsync(int athleteId, int disciplineId)
{
	//1
	await GetAthleteAsync(athleteId, disciplineId);
	await _athleteRepository.DeleteAthleteAsync(athleteId,disciplineId);
	var result = await _athleteRepository.SaveChangesAsync();

	if (!result)//2
	{
		throw new Exception("Database Error");//3
	}
	return result;//4
	
}
```
### Grafo

```mermaid
graph TD
    I(I) --> 1
    1(1) --> 2
    2(2) --> 3
    2(2) --> 4
    3(3) --> F
    4(4) --> F
```

### Complejidad ciclo matica

Numero de regiones
$$ v(G) = R $$
$$v(G) = 2 $$

Numero de nodos y aristas
$$ v(G) = E - N + 2 $$
$$ v(G) = 6 - 6 + 2 $$
$$ v(G) = 2 $$
  
Numero de decisiones
$$ v(G) = P + 1 $$
$$ v(G) = 1 + 1$$
$$ v(G) = 2 $$

### Casos de prueba
| | Camino   | Entrada   | TC | Salida  |
| --- | --- | --- | --- | --- |
| 1 | I 1 2 3 4 F |  `_athleteRepository.SaveChangesAsync();` retorna `result = false`  | `result = false` | `throw new Exception("Database Error");` |
| 2 | I 1 2 5 F |  `_athleteRepository.SaveChangesAsync();` retorna `result = true`  | `result = true` | `true` |

1. Verificar que si `_athleteRepository.SaveChangesAsync();` retorna `result = false`, entonces `DeleteAthleteAsync`  lanza una excepción `throw new Exception("Database Error");`.
2. Verificar que si `_athleteRepository.SaveChangesAsync();` retorna `result = true` (con `athleteId=1`, `disciplineId=1`) entonces `DeleteAthleteAsync` retornara `true`
	
Camino 1
```mermaid
graph TD
    I(I):::c1 --> 1
    1(1):::c1 --> 2
    2(2):::c1 --> 3
    2(2) --> 4
    3(3):::c1 --> F:::c1
    4(4) --> F
    classDef c1 fill:#F2274C, stroke:#F2274C;
```

Camino 2
```mermaid
graph TD
    I(I):::c1 --> 1
    1(1):::c1 --> 2
    2(2):::c1 --> 3
    2(2) --> 4
    3(3) --> F:::c1
    4(4):::c1 --> F
    classDef c1 fill:#2964D9, stroke:#2964D9;
```

### Pruebas unitarias

```csharp

```