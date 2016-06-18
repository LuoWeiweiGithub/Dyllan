﻿USE [master]
GO
/****** Object:  Database [Douban]    Script Date: 6/18/2016 09:14:45 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'Douban')
DROP DATABASE [Douban]
GO
/****** Object:  Database [Douban]    Script Date: 6/18/2016 09:14:45 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Douban')
BEGIN
CREATE DATABASE [Douban]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Douban', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Douban.mdf' , SIZE = 8602624KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Douban_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Douban_log.ldf' , SIZE = 2160960KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END
GO
ALTER DATABASE [Douban] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Douban].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Douban] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Douban] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Douban] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Douban] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Douban] SET ARITHABORT OFF 
GO
ALTER DATABASE [Douban] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Douban] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Douban] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Douban] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Douban] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Douban] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Douban] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Douban] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Douban] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Douban] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Douban] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Douban] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Douban] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Douban] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Douban] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Douban] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Douban] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Douban] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Douban] SET RECOVERY FULL 
GO
ALTER DATABASE [Douban] SET  MULTI_USER 
GO
ALTER DATABASE [Douban] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Douban] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Douban] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Douban] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Douban', N'ON'
GO