USE sharedExperiences;

-- Clean existing data (in reverse order of dependencies)
DELETE FROM VolumeDiscounts;
DELETE FROM ExperienceRegistrations;
DELETE FROM SharedExperienceGuests;
DELETE FROM SharedExperienceExperiences;
DELETE FROM SharedExperiences;
DELETE FROM Experiences;
DELETE FROM Guests;
DELETE FROM Providers;

-- Insert Providers
INSERT INTO Providers (BusinessAddress, PhoneNumber, CVR)
VALUES 
    ('Finlandsgade 17, 8200 Aarhus N', '+45 71555080', '11111114'); -- Noah's Hotel

-- Insert Guests
INSERT INTO Guests (Name, PhoneNumber, Age, PersonalId)
VALUES 
    ('Joan', '+45 11111111', 25, 'ID001'),
    ('Suzzane', '+45 22222222', 28, 'ID002'),
    ('Patrick', '+45 33333333', 30, 'ID003'),
    ('Anne', '+45 44444444', 27, 'ID004');

-- Insert Experiences
INSERT INTO Experiences (ProviderId, Name, Description, Price)
VALUES 
    (1, 'Night at Noah''s Hotel Single room', 'Single room accommodation', 730.50),
    (1, 'Night at Noah''s Hotel Double room', 'Double room accommodation', 910.99),
    (NULL, 'Flight AAR – VIE', 'Flight from Aarhus to Vienna', 1000.70),
    (NULL, 'Vienna Historic Center Walking Tour', 'Guided tour of Vienna', 100.00);

-- Insert SharedExperiences
INSERT INTO SharedExperiences (Name, Date)
VALUES 
    ('Trip to Austria', '2024-07-02'),
    ('Dinner Downtown', '2024-04-07'),
    ('Pottery Weekend', '2024-03-22');

-- Insert SharedExperienceExperiences
-- For Trip to Austria
INSERT INTO SharedExperienceExperiences (SharedExperienceId, ExperienceId)
VALUES 
    (1, 3), -- Flight AAR - VIE
    (1, 1), -- Single room
    (1, 4); -- Walking Tour

-- Insert SharedExperienceGuests
-- For Trip to Austria
INSERT INTO SharedExperienceGuests (SharedExperienceId, GuestId)
VALUES 
    (1, 1), -- Joan
    (1, 2), -- Suzzane
    (1, 3), -- Patrick
    (1, 4); -- Anne

-- Insert ExperienceRegistrations
-- For Vienna Historic Center Walking Tour in Trip to Austria
INSERT INTO ExperienceRegistrations (ExperienceId, GuestId, SharedExperienceId)
VALUES 
    (4, 1, 1), -- Joan for Walking Tour
    (4, 2, 1), -- Suzzane for Walking Tour
    -- Hotel registrations
    (1, 1, 1), -- Joan for Hotel
    (1, 2, 1), -- Suzzane for Hotel
    (1, 3, 1), -- Patrick for Hotel
    (1, 4, 1), -- Anne for Hotel
    -- Flight registrations
    (3, 1, 1), -- Joan for Flight
    (3, 2, 1), -- Suzzane for Flight
    (3, 3, 1), -- Patrick for Flight
    (3, 4, 1); -- Anne for Flight

-- Insert VolumeDiscounts for Noah's Hotel
INSERT INTO VolumeDiscounts (ProviderId, MinGroupSize, DiscountPercentage)
VALUES 
    (1, 10, 10.00), -- 10% discount for 10+ guests
    (1, 50, 20.00); -- 20% discount for 50+ guests