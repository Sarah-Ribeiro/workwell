-- Buscar todos os dispositivos de um usuário
SELECT * 
FROM Devices
WHERE UserId = 1;

-- Buscar dados de saúde de um usuário
SELECT *
FROM HealthData
WHERE UserId = 2
ORDER BY CreatedAt DESC;

-- Buscar último registro de saúde
SELECT *
FROM HealthData
WHERE UserId = 3
ORDER BY CreatedAt DESC
LIMIT 1;


