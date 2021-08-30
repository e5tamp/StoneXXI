USE [StoneX]
GO
CREATE FUNCTION [dbo].[GetCurrencyDaily] 
(
	@valute nvarchar(50), -- функция принимает ID или Имя валюты
	@date date
)
RETURNS money
AS
BEGIN
	DECLARE @currency money

	SELECT @currency = value FROM CurrencyDaily CD
	LEFT JOIN Currency C ON CD.currency_id = C.id 
	WHERE ((CD.currency_id = @valute) OR (C.name = @valute)) 
	AND CD.date = @date

	
	RETURN @currency

END
GO


