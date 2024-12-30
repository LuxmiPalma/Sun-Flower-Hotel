--1. Hämta alla bokningar med Gäst- och Rumsinformation
SELECT 
    b.BookingId,
    g.Name AS GuestName,
    r.RoomNumber,
    r.Type AS RoomType,
    b.CheckInDate,
    b.CheckOutDate,
    e.Name AS EmployeeName
FROM 
    Bookings b
INNER JOIN Guests g ON b.GuestId = g.GuestId
INNER JOIN Rooms r ON b.RoomNumber = r.RoomNumber
INNER JOIN Employees e ON b.EmployeeId = e.EmployeeId;

--2. Hämta obetalda fakturor med Bokningsinformation
SELECT 
    pi.PaymentInvoiceId,
    pi.Amount,
    pi.PaymentStatus,
    pi.InvoiceDate,
    b.BookingId,
    g.Name AS GuestName,
    r.RoomNumber
FROM 
    PaymentInvoices pi
INNER JOIN Bookings b ON pi.BookingId = b.BookingId
INNER JOIN Guests g ON b.GuestId = g.GuestId
INNER JOIN Rooms r ON b.RoomNumber = r.RoomNumber
WHERE 
    pi.PaymentStatus = 'Pending';

--3. Visa alla tjänster kopplade till bokningar
SELECT 
    bs.BookingServiceId,
    b.BookingId,
    s.Name AS ServiceName,
    bs.Quantity,
    s.Price,
    (bs.Quantity * s.Price) AS TotalServiceCost
FROM 
    BookingServices bs
INNER JOIN Services s ON bs.ServiceId = s.ServiceId
INNER JOIN Bookings b ON bs.BookingId = b.BookingId;

--4. Hämta Feedback med Gäst- och Bokningsinformation
SELECT 
    f.FeedbackId,
    g.Name AS GuestName,
    f.Comments,
    f.Rating,
    e.Name AS EmployeeName
FROM 
    Feedbacks f
INNER JOIN Guests g ON f.GuestId = g.GuestId
LEFT JOIN Employees e ON f.EmployeeId = e.EmployeeId;

--5. Visa alla lediga rum för en viss period
SELECT 
    r.RoomNumber,
    r.Type AS RoomType,
    r.Price
FROM 
    Rooms r
WHERE 
    NOT EXISTS (
        SELECT 1
        FROM Bookings b
        WHERE 
            b.RoomNumber = r.RoomNumber
            AND '2024-12-18' < b.CheckOutDate 
            AND '2024-12-19' > b.CheckInDate
    );
--6. Summera totala intäkter från betalningar
SELECT 
    SUM(pi.Amount) AS TotalRevenue
FROM 
    PaymentInvoices pi
WHERE 
    pi.PaymentStatus = 'Completed';

--7. Hämta antal bokningar per rum
SELECT 
    r.RoomNumber,
    r.Type AS RoomType,
    COUNT(b.BookingId) AS TotalBookings
FROM 
    Rooms r
LEFT JOIN Bookings b ON r.RoomNumber = b.RoomNumber
GROUP BY 
    r.RoomNumber, r.Type
ORDER BY 
    TotalBookings DESC;

--8. Hämta gäster som har gjort bokningar med deras kontaktinformation
SELECT 
    g.GuestId,
    g.Name AS GuestName,
    g.ContactNumber,
    g.Email,
    COUNT(b.BookingId) AS TotalBookings
FROM 
    Guests g
LEFT JOIN Bookings b ON g.GuestId = b.GuestId
GROUP BY 
    g.GuestId, g.Name, g.ContactNumber, g.Email
ORDER BY 
    TotalBookings DESC;

--9. Visa fakturor med tillhörande bokning och tjänster
SELECT 
    pi.PaymentInvoiceId,
    pi.Amount,
    pi.PaymentStatus,
    b.BookingId,
    bs.ServiceName,
    bs.Quantity
FROM 
    PaymentInvoices pi
INNER JOIN Bookings b ON pi.BookingId = b.BookingId
LEFT JOIN BookingServices bs ON b.BookingId = bs.BookingId;

--10. Visa alla anställda och deras bokningar
SELECT 
    e.EmployeeId,
    e.Name AS EmployeeName,
    e.Role,
    COUNT(b.BookingId) AS TotalBookingsHandled
FROM 
    Employees e
LEFT JOIN Bookings b ON e.EmployeeId = b.EmployeeId
GROUP BY 
    e.EmployeeId, e.Name, e.Role
ORDER BY 
    TotalBookingsHandled DESC;

--GROUP BY
--1. Hämta antal bokningar per rum
SELECT 
    RoomNumber, 
    COUNT(BookingId) AS TotalBookings
FROM 
    Bookings
GROUP BY 
    RoomNumber
ORDER BY 
    TotalBookings DESC;

--2. Summera totala intäkter från alla betalningar
SELECT 
    SUM(Amount) AS TotalRevenue
FROM 
    PaymentInvoices
WHERE 
    PaymentStatus = 'Completed';

--3. Hämta antal bokningar per gäst
SELECT 
    GuestId, 
    COUNT(BookingId) AS TotalBookings
FROM 
    Bookings
GROUP BY 
    GuestId
ORDER BY 
    TotalBookings DESC;


--4. Hämta räkning av obetalda fakturor
SELECT 
    COUNT(PaymentInvoiceId) AS UnpaidInvoices
FROM 
    PaymentInvoices
WHERE 
    PaymentStatus = 'Pending';

--5.Visa alla tjänster och deras totalkostnad med kvantitet
SELECT 
    ServiceId, 
    SUM(Quantity) AS TotalQuantity
FROM 
    BookingServices
GROUP BY 
    ServiceId;
--6.Räkna totalt antal gäster
SELECT 
    COUNT(GuestId) AS TotalGuests
FROM 
    Guests;

	--7.Visa rummen med extra sängar (ExtraBeds > 0)
	SELECT 
    RoomNumber, 
    ExtraBeds
FROM 
    Rooms
WHERE 
    ExtraBeds > 0;

--8.Hämta fakturor skapade det senaste året
SELECT 
    PaymentInvoiceId, 
    Amount, 
    InvoiceDate
FROM 
    PaymentInvoices
WHERE 
    YEAR(InvoiceDate) = YEAR(GETDATE());

--LEFT JOIN ,Visa alla gäster och deras bokningar (inkludera även gäster utan bokningar)

SELECT 
    g.GuestId,
    g.Name AS GuestName,
    b.BookingId,
    b.RoomNumber,
    b.CheckInDate,
    b.CheckOutDate
FROM 
    Guests g
LEFT JOIN 
    Bookings b ON g.GuestId = b.GuestId
ORDER BY 
    g.GuestId;

--RIGHT JOIN – Visa alla bokningar och deras gäster (inkludera bokningar utan gästinformation)
SELECT 
    b.BookingId,
    b.RoomNumber,
    g.GuestId,
    g.Name AS GuestName,
    b.CheckInDate,
    b.CheckOutDate
FROM 
    Bookings b
RIGHT JOIN 
    Guests g ON g.GuestId = b.GuestId
ORDER BY 
    b.BookingId;



