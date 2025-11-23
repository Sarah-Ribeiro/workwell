-- ==========================================
-- TABELA: Users
-- ==========================================
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Age INTEGER CHECK (Age >= 18 AND Age <= 100),
    CreatedAt DATETIME NOT NULL,
    BaselineHeartRate INTEGER NOT NULL CHECK (BaselineHeartRate BETWEEN 40 AND 100),
    IsActive INTEGER NOT NULL DEFAULT 1
);


-- ==========================================
-- TABELA: Devices
-- ==========================================
CREATE TABLE Devices (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Type TEXT NOT NULL,
    SerialNumber TEXT NOT NULL UNIQUE,
    IsActive INTEGER NOT NULL DEFAULT 1,
    RegisteredAt DATETIME NOT NULL,

    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);


-- ==========================================
-- TABELA: HealthData
-- ==========================================
CREATE TABLE HealthData (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    HeartRate INTEGER NOT NULL CHECK (HeartRate BETWEEN 40 AND 200),
    StressLevel REAL NOT NULL CHECK (StressLevel BETWEEN 0 AND 1),
    NoiseLevel REAL,
    Temperature REAL,
    CreatedAt DATETIME NOT NULL,
    Notes TEXT,

    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);


-- ==========================================
-- TABELA: Alerts
-- ==========================================
CREATE TABLE Alerts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    HealthDataId INTEGER NOT NULL,
    Type TEXT NOT NULL,
    Severity TEXT NOT NULL,
    Message TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    IsResolved INTEGER NOT NULL DEFAULT 0,

    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (HealthDataId) REFERENCES HealthData(Id) ON DELETE CASCADE
);
