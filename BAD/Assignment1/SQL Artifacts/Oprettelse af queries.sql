use sharedExperiences;

-- 1. Get the data collected for each experience provider
SELECT 
    BusinessAddress,
    PhoneNumber,
    CVR
FROM Providers;

-- 2. List the experiences available in the system
SELECT 
    Name,
    CAST(Price AS MONEY) as Price  -- Using MONEY type for currency
FROM Experiences;

-- 3. Get the list of shared experiences and their date in system
SELECT 
    Name,
    CONVERT(VARCHAR(10), Date, 120) as Date  -- T-SQL date conversion
FROM SharedExperiences
ORDER BY Date DESC;

-- 4. Get the guests registered for a shared experience
SELECT 
    g.Name
FROM Guests g
INNER JOIN SharedExperienceGuests seg ON g.GuestId = seg.GuestId
INNER JOIN SharedExperiences se ON seg.SharedExperienceId = se.SharedExperienceId
WHERE se.Name = N'Trip to Austria';  -- Using N prefix for Unicode strings

-- 5. Get the experiences included in a shared experience
SELECT 
    e.Name
FROM Experiences e
INNER JOIN SharedExperienceExperiences see ON e.ExperienceId = see.ExperienceId
INNER JOIN SharedExperiences se ON see.SharedExperienceId = se.SharedExperienceId
WHERE se.Name = N'Trip to Austria';

-- 6. Get the guests registered for specific experiences
SELECT 
    g.Name
FROM Guests g
INNER JOIN ExperienceRegistrations er ON g.GuestId = er.GuestId
INNER JOIN Experiences e ON er.ExperienceId = e.ExperienceId
INNER JOIN SharedExperiences se ON er.SharedExperienceId = se.SharedExperienceId
WHERE e.Name = N'Vienna Historic Center Walking Tour'
    AND se.Name = N'Trip to Austria';

-- 7. Get price statistics
SELECT 
    CAST(MIN(Price) AS MONEY) as MinPrice,
    CAST(AVG(Price) AS MONEY) as AvgPrice,
    CAST(MAX(Price) AS MONEY) as MaxPrice
FROM Experiences;

-- 8. Get guest count and sales summary
SELECT 
    e.Name,
    COUNT(er.GuestId) as NumberOfGuests,
    CAST(ISNULL(COUNT(er.GuestId) * e.Price, 0) AS MONEY) AS TotalSales
FROM Experiences e
LEFT OUTER JOIN ExperienceRegistrations er ON e.ExperienceId = er.ExperienceId
GROUP BY e.ExperienceId, e.Name, e.Price;


-- 9. Custom query: Volume discounts tracking
SELECT 
    e.Name AS ExperienceName,
    vd.MinGroupSize AS DiscountThreshold,
    vd.DiscountPercentage AS PotentialDiscount
FROM Experiences e
INNER JOIN Providers p ON e.ProviderId = p.ProviderId
INNER JOIN VolumeDiscounts vd ON p.ProviderId = vd.ProviderId
ORDER BY e.Name, vd.MinGroupSize;
