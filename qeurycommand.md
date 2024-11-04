mysql -u root -p

CREATE DATABASE IF NOT EXISTS PORTJARNOBEERTEN;

USE PORTJARNOBEERTEN;

CREATE USER 'port_lezer'@'localhost' IDENTIFIED BY 'veilig_wachtwoord';

GRANT SELECT ON PORTJARNOBEERTEN.* TO 'port_lezer'@'localhost';

CREATE USER 'port_beheerder'@'localhost' IDENTIFIED BY 'ander_veilig_wachtwoord';

GRANT ALL PRIVILEGES ON PORTJARNOBEERTEN.* TO 'port_beheerder'@'localhost';

FLUSH PRIVILEGES;

-- Maak eerst een voorbeeldtabel
CREATE TABLE Schepen (
  ID INT AUTO_INCREMENT PRIMARY KEY,
  Naam VARCHAR(100),
  Type VARCHAR(50),
  Capaciteit INT
);

-- Voeg wat voorbeeldgegevens toe
INSERT INTO Schepen (Naam, Type, Capaciteit) VALUES 
('Zeemeeuw', 'Vrachtschip', 5000),
('Windkracht', 'Tanker', 10000),
('Havengolf', 'Containerschip', 15000);

-- Toon alle schepen
SELECT * FROM Schepen;

-- Toon de namen van alle vrachtschepen
SELECT Naam FROM Schepen WHERE Type = 'Vrachtschip';

-- Toon de top 3 schepen met de hoogste capaciteit
SELECT Naam, Capaciteit FROM Schepen ORDER BY Capaciteit DESC LIMIT 3;

-- Maak nog een voorbeeldtabel
CREATE TABLE Ladingen (
  ID INT AUTO_INCREMENT PRIMARY KEY,
  SchipID INT,
  Inhoud VARCHAR(100),
  Gewicht INT,
  FOREIGN KEY (SchipID) REFERENCES Schepen(ID)
);

-- Voeg wat voorbeeldgegevens toe
INSERT INTO Ladingen (SchipID, Inhoud, Gewicht) VALUES 
(1, 'Graan', 3000),
(2, 'Olie', 8000),
(3, 'Containers', 12000);

-- Toon schepen met hun lading
SELECT s.Naam AS Schip, l.Inhoud AS Lading, l.Gewicht
FROM Schepen s
JOIN Ladingen l ON s.ID = l.SchipID;

-- Bereken de gemiddelde lading per scheepstype
SELECT s.Type, AVG(l.Gewicht) AS GemiddeldGewicht
FROM Schepen s
JOIN Ladingen l ON s.ID = l.SchipID
GROUP BY s.Type;

-- Vind schepen zonder lading
SELECT s.Naam AS SchipZonderLading
FROM Schepen s
LEFT JOIN Ladingen l ON s.ID = l.SchipID
WHERE l.ID IS NULL;

-- Voeg een nieuw schip toe
INSERT INTO Schepen (Naam, Type, Capaciteit)
VALUES ('Noordster', 'Sleepboot', 2000);

-- Werk de capaciteit van een bestaand schip bij
UPDATE Schepen
SET Capaciteit = Capaciteit + 1000
WHERE Naam = 'Zeemeeuw';

-- Verwijder een schip
DELETE FROM Schepen
WHERE Naam = 'Noordster';

-- Werk het type van een schip bij
UPDATE Schepen
SET Type = 'Bulkcarrier'
WHERE Naam = 'Windkracht';
