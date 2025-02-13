USE sharedExperiences;

-- 1. Get the data collected for each experience provider
-- Example output: Finlandsgade 17, 8200 Aarhus N +45 71555080 11111114
SELECT 
    BusinessAddress,
    PhoneNumber,
    CVR
FROM Providers;

-- 2. List the experiences available in the system
-- Example output: 
-- Night at Noah's Hotel Single room 730,50 DKK
-- Night at Noah's Hotel Double room 910,99 DKK
-- Flight AAR – VIE 1000,70 DKK
-- Vienna Historic Center Walking Tour 100,00 DKK
SELECT 
    Name,
    CAST(Price AS DECIMAL(10,2)) as Price
FROM Experiences;

-- 3. Get the list of shared experiences and their date in the system in descending order
-- Example output:
-- Trip to Austria 2024-07-02
-- Dinner Downtown 2024-04-07
-- Pottery Weekend 2024-03-22
SELECT 
    Name,
    Date
FROM SharedExperiences
ORDER BY Date DESC;

-- 4. Get the guests registered for a shared experience
-- Example output for "Trip to Austria":
-- Joan
-- Suzzane
-- Patrick
-- Anne
SELECT DISTINCT
    g.Name
FROM Guests g
JOIN SharedExperienceGuests seg ON g.GuestId = seg.GuestId
JOIN SharedExperiences se ON seg.SharedExperienceId = se.SharedExperienceId
WHERE se.Name = 'Trip to Austria';

-- 5. Get the experiences included in a shared experience
-- Example output for "Trip to Austria":
-- Flight AAR – VIE 
-- Night at Noah's Hotel Single room
-- Vienna Historic Center Walking Tour
SELECT 
    e.Name
FROM Experiences e
JOIN SharedExperienceExperiences see ON e.ExperienceId = see.ExperienceId
JOIN SharedExperiences se ON see.SharedExperienceId = se.SharedExperienceId
WHERE se.Name = 'Trip to Austria';

-- 6. Get the guests registered for one of the experiences in a shared experience
-- Example outputs:
-- For Vienna Historic Center Walking Tour in Trip to Austria:
SELECT DISTINCT
    g.Name
FROM Guests g
JOIN ExperienceRegistrations er ON g.GuestId = er.GuestId
JOIN Experiences e ON er.ExperienceId = e.ExperienceId
JOIN SharedExperiences se ON er.SharedExperienceId = se.SharedExperienceId
WHERE e.Name = 'Vienna Historic Center Walking Tour'
AND se.Name = 'Trip to Austria';

-- For Night at Noah's Hotel in Trip to Austria:
SELECT DISTINCT
    g.Name
FROM Guests g
JOIN ExperienceRegistrations er ON g.GuestId = er.GuestId
JOIN Experiences e ON er.ExperienceId = e.ExperienceId
JOIN SharedExperiences se ON er.SharedExperienceId = se.SharedExperienceId
WHERE e.Name = 'Night at Noah''s Hotel Single room'
AND se.Name = 'Trip to Austria';

-- 7. Get the minimum, average, and maximum price for the whole experience in the system
-- Example output: 100 685.5475 1000,70
SELECT 
    MIN(Price) as MinPrice,
    AVG(Price) as AvgPrice,
    MAX(Price) as MaxPrice
FROM Experiences;

-- 8. Get the number of guests and sum of sales for each experience available in the system
-- Example output:
-- Night at Noah's Hotel Single room 4 2922,00
-- Night at Noah's Hotel Double room 0 0
-- Flight AAR – VIE 4 4002,80
-- Vienna Historic Center Walking Tour 2 200
SELECT 
    e.Name,
    COUNT(DISTINCT er.GuestId) as NumberOfGuests,
    ISNULL(SUM(e.Price), 0) as TotalSales
FROM Experiences e
LEFT JOIN ExperienceRegistrations er ON e.ExperienceId = er.ExperienceId
GROUP BY e.Name;

-- 9. Custom query: Get available discounts for each experience and the current group size
-- This query shows for each experience how close they are to reaching discount thresholds
SELECT 
    e.Name as ExperienceName,
    COUNT(DISTINCT er.GuestId) as CurrentGroupSize,
    vd.MinGroupSize as NextDiscountThreshold,
    vd.DiscountPercentage as PotentialDiscount
FROM Experiences e
JOIN Providers p ON e.ProviderId = p.ProviderId
LEFT JOIN ExperienceRegistrations er ON e.ExperienceId = er.ExperienceId
LEFT JOIN VolumeDiscounts vd ON p.ProviderId = vd.ProviderId
WHERE vd.MinGroupSize > (
    SELECT COUNT(DISTINCT GuestId) 
    FROM ExperienceRegistrations 
    WHERE ExperienceId = e.ExperienceId
)
GROUP BY e.Name, vd.MinGroupSize, vd.DiscountPercentage;