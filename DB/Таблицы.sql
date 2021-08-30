USE [StoneX]
GO

CREATE TABLE [dbo].[Currency](
	[id] [varchar](7) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[num_code] [nchar](3) NULL,
	[char_code] [nchar](3) NULL,
	[nominal] [int] NULL,
	[parent_code] [varchar](10) NULL,
	[eng_name] [varchar](50) NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------
USE [StoneX]
GO

CREATE TABLE [dbo].[CurrencyDaily](
    [value] [money] NOT NULL,
    [currency_id] [varchar](7) NOT NULL,
    [date] [date] NOT NULL,
 CONSTRAINT [PK_CurrencyDaily] PRIMARY KEY CLUSTERED 
(
    [currency_id] ASC,
    [date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CurrencyDaily]  WITH CHECK ADD  CONSTRAINT [FK_CurrencyDaily_Currency] FOREIGN KEY([currency_id])
REFERENCES [dbo].[Currency] ([id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CurrencyDaily] CHECK CONSTRAINT [FK_CurrencyDaily_Currency]
GO