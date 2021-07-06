create procedure ShowSellerWithBiggerTotalSummOfSells
as
select top 1 se.Name, se.Surname from Sales as s join Sellers as se on s.SellerId = se.Id group by se.Id,se.Name,se.Surname order by Sum(s.Price) desc

exec ShowSellerWithBiggerTotalSummOfSells