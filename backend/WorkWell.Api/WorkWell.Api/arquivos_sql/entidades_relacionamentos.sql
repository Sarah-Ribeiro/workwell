/* ===========================================================
   DIAGRAMA ER - SISTEMA WORKWELL
   Representação em ASCII
   ===========================================================

                +----------------+
                |     Users      |
                +----------------+
                | Id (PK)        |
                | Name           |
                | Email          |
                | Age            |
                | CreatedAt      |
                | BaselineHR     |
                | IsActive       |
                +--------+-------+
                         |
                 (1)     |     (N)
                         |
                +--------v-------+
                |    Devices     |
                +----------------+
                | Id (PK)        |
                | UserId (FK)    |
                | Name           |
                | Type           |
                | SerialNumber   |
                | IsActive       |
                | RegisteredAt   |
                +----------------+


                +----------------+
                |   HealthData   |
                +----------------+
                | Id (PK)        |
                | UserId (FK)    |
                | HeartRate      |
                | StressLevel    |
                | NoiseLevel     |
                | Temperature    |
                | CreatedAt      |
                | Notes          |
                +--------+-------+
                         |
                 (1)     |     (N)
                         |
                +--------v-------+
                |    Alerts      |
                +----------------+
                | Id (PK)        |
                | UserId (FK)    |
                | HealthDataId(FK)|
                | Type           |
                | Severity       |
                | Message        |
                | CreatedAt      |
                | IsResolved     |
                +----------------+
*/
