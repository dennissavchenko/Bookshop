INSERT INTO Publisher (Name, Address_Street, Address_HouseNumber, Address_City, Address_PostalCode, Address_Country, Email, PhoneNumber)
VALUES
    ('HMH Books', '125 High Street', '10A', 'Boston', '02110', 'USA', 'info@hmhbooks.com', '+1-800-555-1234'),
    ('Maple Leaf Press', '42 King St', '5B', 'Toronto', 'M5H 1A1', 'Canada', 'contact@mapleleafpress.ca', '+1-416-555-9876'),
    ('Aurora Publishing', '78 Søndergade', '12', 'Aarhus', '8000', 'Denmark', 'hello@aurorapub.dk', '+45-70-22-3344');

INSERT INTO AgeCategory (Tag, Description, MinimumAge)
VALUES
    ('Children', 'Books suitable for children', 0),
    ('Young Adult', 'Books suitable for teenagers', 13),
    ('Adult', 'Books suitable for adults', 18);

INSERT INTO Item (Name, Description, ImageUrl, PublishingDate, Language, Price, AmountInStock, PublisherId, AgeCategoryId, IsUsed, IsSealed, Condition, HasAnnotations)
VALUES
    ('Wool', 'A dystopian novel set in a post-apocalyptic world', 'https://example.com/images/wool.jpg', '2011-06-01', 'English', 15.99, 100, 1, 2, FALSE, TRUE, NULL, NULL),
    ('The Fault in Our Stars', 'A romantic novel about two teenagers with cancer', 'https://example.com/images/fault_in_our_stars.jpg', '2012-01-10', 'English', 12.99, 50, 2, 2, FALSE, TRUE, NULL, NULL),
    ('Harry Potter and the Philosophers Stone', 'The first book in the Harry Potter series', 'https://example.com/images/harry_potter.jpg', '1997-06-26', 'English', 9.99, 200, 3, 1, TRUE, NULL, 0, FALSE);

insert into Author (Name, Surname, DOB, Pseudonym)
VALUES
    ('Hugh', 'Howey', '1975-06-23', NULL),
    ('John', 'Green', '1977-08-24', NULL),
    ('J.K.', 'Rowling', '1965-07-31', NULL);

insert into BookAuthor (BookId, AuthorId)
VALUES
    (1, 1),  -- Wool by Hugh Howey
    (2, 2),  -- The Fault in Our Stars by John Green
    (3, 3);  -- Harry Potter and the Philosophers Stone by J.K. Rowling

INSERT INTO Genre (Name, Description)
VALUES
    ('Science Fiction', 'Fiction dealing with futuristic concepts such as advanced science and technology'),
    ('Romance', 'Fiction that focuses on romantic relationships'),
    ('Fantasy', 'Fiction set in an imaginary universe with magical elements');

INSERT INTO BookGenre (BookId, GenreId)
VALUES
    (1, 1),  -- Wool is Science Fiction
    (2, 2),  -- The Fault in Our Stars is Romance
    (3, 3);  -- Harry Potter and the Philosophers Stone is Fantasy

insert into Book (ItemId, NumberOfPages, CoverType) VALUES
                                                        (1, 500, 1),
                                                        (3, 223, 2);

insert into Magazine (ItemId, IsSpecialEdition) VALUES
    (2, FALSE);

insert into User (Name, Surname, Email, Username, Password)
VALUES
    ('Dennis', 'Savchenko', 'dsa@gmail.com', 'deddddnnissavchenko', '1234qwer');

insert into Customer (UserId, DOB, Address_Street, Address_HouseNumber, Address_City, Address_PostalCode, Address_Country) VALUES
    (1, '1990-05-15', 'Main St', '123', 'Springfield', '62701', 'USA');

insert into Review (CustomerId, ItemId, Rating, Text, TimeStamp) VALUES
                                                                     (1, 1, 5, 'An amazing read! The world-building is incredible.', '2023-10-01 10:00:00'),
                                                                     (1, 2, 4, 'A touching story with great characters.', '2023-10-02 11:30:00'),
                                                                     (1, 3, 5, 'A classic that never gets old!', '2023-10-03 12:45:00');





                                
