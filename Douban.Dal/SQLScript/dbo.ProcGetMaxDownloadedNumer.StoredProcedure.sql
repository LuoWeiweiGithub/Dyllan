USE [Douban]
GO
/****** Object:  StoredProcedure [dbo].[ProcGetMaxDownloadedNumer]    Script Date: 6/8/2016 20:34:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ProcGetMaxDownloadedNumer]
AS
BEGIN
	SELECT TOP 1 URLNumber FROM BookInfo ORDER BY URLNumber DESC
END

GO
